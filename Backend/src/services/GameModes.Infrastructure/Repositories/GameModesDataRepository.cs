using GameModes.Contracts.V1;
using GameModes.Domain.Models;
using GameModes.Infrastructure.DbContext;
using GameModes.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameModes.Infrastructure.Repositories;

public class GameModesDataRepository : IGameModesDataRepository
{
    private readonly IDbContextFactory<GameModesDataContext> _dataContextFactory;

    public GameModesDataRepository(IDbContextFactory<GameModesDataContext> dataContextFactory)
    {
        _dataContextFactory = dataContextFactory;
    }

    public IReadOnlyList<TestModeMeta> GetModes()
    {
        using var context = _dataContextFactory.CreateDbContext();

        return context.GameModes
            .AsNoTracking()
            .Select(x => new TestModeMeta
            {
                Mode = x.Mode,
                Title = x.Title,
                SupportsPerQuestionFeedback = x.SupportsPerQuestionFeedback,
                SupportsFinalSubmitOnly = x.SupportsFinalSubmitOnly
            })
            .ToList();
    }

    public TestFiltersResponse GetFilters()
    {
        using var context = _dataContextFactory.CreateDbContext();

        var sets = context.TestSets.AsNoTracking().ToList();

        return new TestFiltersResponse
        {
            Difficulties = sets.Select(x => x.Difficulty).Distinct().Order().ToList(),
            CategoriesByMode = sets
                .GroupBy(x => x.Mode)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Category).Distinct().Order().ToList()),
            SectionDates = sets.Select(x => x.SectionDate).Distinct().Order().ToList(),
            Sort = ["relevance", "newest", "progress_desc", "progress_asc", "questions_desc", "questions_asc"]
        };
    }

    public IReadOnlyList<TestSetAggregate> GetSetsByMode(string mode)
    {
        using var context = _dataContextFactory.CreateDbContext();

        return context.TestSets
            .AsNoTracking()
            .Include(x => x.Questions)
            .Where(x => x.Mode == mode)
            .Select(MapSetToAggregate)
            .ToList();
    }

    public TestSetAggregate? GetSetByModeAndId(string mode, string setId)
    {
        using var context = _dataContextFactory.CreateDbContext();

        var set = context.TestSets
            .AsNoTracking()
            .Include(x => x.Questions)
            .FirstOrDefault(x => x.Mode == mode && x.Id == setId);

        return set == null ? null : MapSetToAggregate(set);
    }

    public TestSessionAggregate CreateSession(string mode, string setId)
    {
        using var context = _dataContextFactory.CreateDbContext();

        var session = new TestSessionEntity
        {
            SessionId = $"sess_{Guid.NewGuid():N}",
            Mode = mode,
            SetId = setId,
            Status = SessionStatuses.InProgress,
            CurrentIndex = 0,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        context.TestSessions.Add(session);
        context.SaveChanges();

        return MapSessionToAggregate(session, []);
    }

    public TestSessionAggregate? GetSession(string mode, string sessionId)
    {
        using var context = _dataContextFactory.CreateDbContext();

        var session = context.TestSessions
            .AsNoTracking()
            .Include(x => x.Answers)
            .FirstOrDefault(x => x.SessionId == sessionId && x.Mode == mode);

        return session == null ? null : MapSessionToAggregate(session, session.Answers);
    }

    public TestSessionAggregate? SubmitSession(string mode, string sessionId, List<SessionAnswerDto> answers, DateTimeOffset? finishedAt)
    {
        using var context = _dataContextFactory.CreateDbContext();

        var session = context.TestSessions
            .Include(x => x.Answers)
            .FirstOrDefault(x => x.SessionId == sessionId && x.Mode == mode);

        if (session == null || session.Status != SessionStatuses.InProgress)
            return null;

        if (session.Answers.Count > 0)
            context.SessionAnswers.RemoveRange(session.Answers);

        var answerEntities = answers.Select(x => new SessionAnswerEntity
        {
            Id = Guid.NewGuid().ToString("N"),
            SessionId = session.SessionId,
            Kind = x.Kind,
            QuestionId = x.QuestionId,
            UserSaidTrue = x.UserSaidTrue,
            SelectedIndex = x.SelectedIndex,
            SelectedLabel = x.SelectedLabel,
            UserText = x.UserText
        }).ToList();

        session.Status = SessionStatuses.Submitted;
        session.UpdatedAt = finishedAt ?? DateTimeOffset.UtcNow;

        context.SessionAnswers.AddRange(answerEntities);
        context.SaveChanges();

        return MapSessionToAggregate(session, answerEntities);
    }

    public (int CorrectCount, List<ResultItemDto> Items)? GetResultItems(string mode, string sessionId)
    {
        var session = GetSession(mode, sessionId);
        if (session == null)
            return null;

        var set = GetSetByModeAndId(mode, session.SetId);
        if (set == null)
            return null;

        var items = new List<ResultItemDto>();
        var correct = 0;

        foreach (var q in set.Questions)
        {
            var answer = session.Answers.FirstOrDefault(a => a.QuestionId == q.QuestionId);
            var item = new ResultItemDto
            {
                Kind = q.Kind,
                QuestionId = q.QuestionId,
                Definition = q.Definition,
                ExplainTerm = q.ExplainTerm,
                ExplainText = q.ExplainText
            };

            if (q.Kind == TestModes.TrueFalse)
            {
                var isPairingCorrect = true;
                var userSaidTrue = answer?.UserSaidTrue ?? false;
                item.Term = q.Term;
                item.TermTranslation = q.TermTranslation;
                item.IsPairingCorrect = isPairingCorrect;
                item.UserSaidTrue = userSaidTrue;
                if (userSaidTrue == isPairingCorrect) correct++;
            }
            else if (q.Kind == TestModes.Questions)
            {
                var correctIndex = 0;
                var selectedIndex = answer?.SelectedIndex ?? -1;
                item.Options = q.Options;
                item.CorrectIndex = correctIndex;
                item.SelectedIndex = selectedIndex;
                if (selectedIndex == correctIndex) correct++;
            }
            else if (q.Kind == TestModes.Selection)
            {
                var correctLabel = "Drag and drop";
                var selectedLabel = answer?.SelectedLabel ?? "__SKIPPED__";
                item.CorrectLabel = correctLabel;
                item.SelectedLabel = selectedLabel;
                if (selectedLabel == correctLabel) correct++;
            }
            else
            {
                var correctText = "Drag and drop";
                var userText = answer?.UserText ?? "__SKIPPED__";
                item.CorrectText = correctText;
                item.UserText = userText;
                if (string.Equals(userText.Trim(), correctText, StringComparison.OrdinalIgnoreCase)) correct++;
            }

            items.Add(item);
        }

        return (correct, items);
    }

    private static TestSetAggregate MapSetToAggregate(TestSetEntity setEntity)
    {
        return new TestSetAggregate
        {
            Meta = new SetMeta
            {
                Id = setEntity.Id,
                Mode = setEntity.Mode,
                Title = setEntity.Title,
                Category = setEntity.Category,
                Difficulty = setEntity.Difficulty,
                DurationLabel = setEntity.DurationLabel,
                Author = setEntity.Author,
                QuestionCount = setEntity.QuestionCount,
                ProgressPercent = setEntity.ProgressPercent,
                SectionDate = setEntity.SectionDate,
                TotalInCourse = setEntity.TotalInCourse
            },
            Questions = setEntity.Questions.Select(x => new QuestionDto
            {
                Kind = x.Kind,
                QuestionId = x.QuestionId,
                Definition = x.Definition,
                ExplainTerm = x.ExplainTerm,
                ExplainText = x.ExplainText,
                Term = x.Term,
                TermTranslation = x.TermTranslation,
                Options = x.Options,
                AnswerPool = x.AnswerPool
            }).ToList(),
            SetAnswerPool = setEntity.SetAnswerPool
        };
    }

    private static TestSessionAggregate MapSessionToAggregate(TestSessionEntity session, IReadOnlyCollection<SessionAnswerEntity> answers)
    {
        return new TestSessionAggregate
        {
            SessionId = session.SessionId,
            Mode = session.Mode,
            SetId = session.SetId,
            Status = session.Status,
            CurrentIndex = session.CurrentIndex,
            Answers = answers.Select(x => new SessionAnswerDto
            {
                Kind = x.Kind,
                QuestionId = x.QuestionId,
                UserSaidTrue = x.UserSaidTrue,
                SelectedIndex = x.SelectedIndex,
                SelectedLabel = x.SelectedLabel,
                UserText = x.UserText
            }).ToList(),
            CreatedAt = session.CreatedAt,
            UpdatedAt = session.UpdatedAt
        };
    }
}

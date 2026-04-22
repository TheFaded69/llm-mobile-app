using System.Collections.Concurrent;
using GameModes.Contracts.V1;
using GameModes.Domain;

namespace GameModes.Infrastructure.Repositories;

public class InMemoryGameModesDataRepository : IGameModesDataRepository
{
    private readonly List<TestSetAggregate> _sets;
    private readonly ConcurrentDictionary<string, TestSessionAggregate> _sessions = new();

    public InMemoryGameModesDataRepository()
    {
        _sets = SeedSets();
    }

    public IReadOnlyList<TestModeMeta> GetModes() =>
    [
        new() { Mode = TestModes.TrueFalse, Title = "True / False", SupportsPerQuestionFeedback = true, SupportsFinalSubmitOnly = true },
        new() { Mode = TestModes.Questions, Title = "Questions", SupportsPerQuestionFeedback = true, SupportsFinalSubmitOnly = true },
        new() { Mode = TestModes.Selection, Title = "Selection", SupportsPerQuestionFeedback = true, SupportsFinalSubmitOnly = true },
        new() { Mode = TestModes.Written, Title = "Written", SupportsPerQuestionFeedback = true, SupportsFinalSubmitOnly = true }
    ];

    public TestFiltersResponse GetFilters() => new()
    {
        Difficulties = ["Легкий", "Средний", "Сложный"],
        CategoriesByMode = new Dictionary<string, List<string>>
        {
            [TestModes.TrueFalse] = ["UX термины"],
            [TestModes.Questions] = ["Вопросы с выбором"],
            [TestModes.Selection] = ["Подбор из списка"],
            [TestModes.Written] = ["Письменный ответ"]
        },
        SectionDates = ["Март 2026", "Август 2026"],
        Sort = ["relevance", "newest", "progress_desc", "progress_asc", "questions_desc", "questions_asc"]
    };

    public IReadOnlyList<TestSetAggregate> GetSetsByMode(string mode) =>
        _sets.Where(x => x.Meta.Mode == mode).ToList();

    public TestSetAggregate? GetSetByModeAndId(string mode, string setId) =>
        _sets.FirstOrDefault(x => x.Meta.Mode == mode && x.Meta.Id == setId);

    public TestSessionAggregate CreateSession(string mode, string setId)
    {
        var session = new TestSessionAggregate
        {
            SessionId = $"sess_{Guid.NewGuid():N}",
            Mode = mode,
            SetId = setId,
            Status = SessionStatuses.InProgress,
            CurrentIndex = 0,
            Answers = [],
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _sessions.TryAdd(session.SessionId, session);
        return session;
    }

    public TestSessionAggregate? GetSession(string mode, string sessionId)
    {
        if (!_sessions.TryGetValue(sessionId, out var session))
            return null;

        return session.Mode == mode ? session : null;
    }

    public TestSessionAggregate? SubmitSession(string mode, string sessionId, List<SessionAnswerDto> answers, DateTimeOffset? finishedAt)
    {
        var session = GetSession(mode, sessionId);

        if (session == null || session.Status != SessionStatuses.InProgress)
            return null;

        session.Answers = answers;
        session.Status = SessionStatuses.Submitted;
        session.UpdatedAt = finishedAt ?? DateTimeOffset.UtcNow;

        return session;
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

    private static List<TestSetAggregate> SeedSets()
    {
        return
        [
            BuildSet(TestModes.Questions, "questions-ux-core", "Вопросы с выбором", options: ["Drag and drop", "Deployment", "Database", "Safe area"]),
            BuildSet(TestModes.TrueFalse, "ux-gestures", "UX термины", term: "Drag and drop", translation: "Перетаскивание"),
            BuildSet(TestModes.Selection, "selection-ux-core", "Подбор из списка", answerPool: ["Drag and drop", "Deployment", "Database"]),
            BuildSet(TestModes.Written, "written-ux-core", "Письменный ответ")
        ];
    }

    private static TestSetAggregate BuildSet(string mode, string id, string category, List<string>? options = null, string? term = null, string? translation = null, List<string>? answerPool = null)
    {
        var question = new QuestionDto
        {
            Kind = mode,
            QuestionId = "choice-1",
            Definition = "Изменение положения интерфейса с помощью перетягивания; дословно «тащи и бросай».",
            ExplainTerm = "Drag and drop",
            ExplainText = "Перетаскивание элементов интерфейса — drag and drop.",
            Options = options,
            Term = term,
            TermTranslation = translation
        };

        return new TestSetAggregate
        {
            Meta = new SetMeta
            {
                Id = id,
                Mode = mode,
                Title = "Термины UX и мобильной разработки",
                Category = category,
                Difficulty = "Средний",
                DurationLabel = "~ 10 мин",
                Author = "Команда курса",
                QuestionCount = 1,
                TotalInCourse = 12,
                ProgressPercent = 0,
                SectionDate = "Март 2026"
            },
            Questions = [question],
            SetAnswerPool = answerPool
        };
    }
}

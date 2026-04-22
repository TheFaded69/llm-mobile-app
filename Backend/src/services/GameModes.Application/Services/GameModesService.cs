using GameModes.Contracts.V1;
using GameModes.Infrastructure.Repositories;

namespace GameModes.Application.Services;

public class GameModesService
{
    private readonly IGameModesDataRepository _repository;

    public GameModesService(IGameModesDataRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyList<TestModeMeta> GetModes() => _repository.GetModes();

    public TestFiltersResponse GetFilters() => _repository.GetFilters();

    public PagedSetCardResponse GetSetsByMode(string mode, int page, int pageSize, string? query, string? difficulty, string? category, string? sectionDate, string? sort)
    {
        var sets = _repository.GetSetsByMode(mode)
            .Select(x => (SetCard)x.Meta)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
            sets = sets.Where(x => x.Title.Contains(query, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(difficulty))
            sets = sets.Where(x => x.Difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(category))
            sets = sets.Where(x => x.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(sectionDate))
            sets = sets.Where(x => x.SectionDate.Equals(sectionDate, StringComparison.OrdinalIgnoreCase));

        sets = sort switch
        {
            "newest" => sets.OrderByDescending(x => x.SectionDate),
            "progress_desc" => sets.OrderByDescending(x => x.ProgressPercent),
            "progress_asc" => sets.OrderBy(x => x.ProgressPercent),
            "questions_desc" => sets.OrderByDescending(x => x.QuestionCount),
            "questions_asc" => sets.OrderBy(x => x.QuestionCount),
            _ => sets.OrderBy(x => x.Title)
        };

        var total = sets.Count();
        var pageItems = sets.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new PagedSetCardResponse
        {
            Items = pageItems,
            Pagination = new PaginationMeta
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                HasNext = page * pageSize < total
            }
        };
    }

    public SetDetailsResponse? GetSetDetails(string mode, string setId)
    {
        var set = _repository.GetSetByModeAndId(mode, setId);
        if (set == null)
            return null;

        return new SetDetailsResponse
        {
            Set = set.Meta,
            Questions = set.Questions
        };
    }

    public SessionStateResponse? CreateSession(string mode, CreateSessionRequest request)
    {
        var set = _repository.GetSetByModeAndId(mode, request.SetId);
        if (set == null)
            return null;

        var session = _repository.CreateSession(mode, request.SetId);

        return ToSessionState(set, session);
    }

    public SessionStateResponse? GetSessionState(string mode, string sessionId)
    {
        var session = _repository.GetSession(mode, sessionId);
        if (session == null)
            return null;

        var set = _repository.GetSetByModeAndId(mode, session.SetId);
        if (set == null)
            return null;

        return ToSessionState(set, session);
    }

    public SubmitResponse? Submit(string mode, string sessionId, SubmitRequest request)
    {
        var session = _repository.SubmitSession(mode, sessionId, request.Answers, request.FinishedAt);
        if (session == null)
            return null;

        var result = _repository.GetResultItems(mode, sessionId);
        if (result == null)
            return null;

        return new SubmitResponse
        {
            SessionId = sessionId,
            Status = SessionStatuses.Submitted,
            Result = new ResultSummary
            {
                CorrectCount = result.Value.CorrectCount,
                Total = result.Value.Items.Count,
                Percent = result.Value.Items.Count == 0 ? 0 : (float)result.Value.CorrectCount / result.Value.Items.Count * 100
            }
        };
    }

    public ResultResponse? GetResult(string mode, string sessionId)
    {
        var session = _repository.GetSession(mode, sessionId);
        if (session == null)
            return null;

        var result = _repository.GetResultItems(mode, sessionId);
        if (result == null)
            return null;

        return new ResultResponse
        {
            SessionId = sessionId,
            Mode = mode,
            SetId = session.SetId,
            CorrectCount = result.Value.CorrectCount,
            Total = result.Value.Items.Count,
            Items = result.Value.Items
        };
    }

    public ReviewResponse? GetReview(string mode, string sessionId)
    {
        var session = _repository.GetSession(mode, sessionId);
        if (session == null)
            return null;

        var result = _repository.GetResultItems(mode, sessionId);
        if (result == null)
            return null;

        return new ReviewResponse
        {
            SessionId = sessionId,
            Mode = mode,
            SetId = session.SetId,
            Items = result.Value.Items
        };
    }

    private static SessionStateResponse ToSessionState(GameModes.Domain.Models.TestSetAggregate set, GameModes.Domain.Models.TestSessionAggregate session)
    {
        return new SessionStateResponse
        {
            SessionId = session.SessionId,
            Mode = session.Mode,
            Set = set.Meta,
            Status = session.Status,
            CurrentIndex = session.CurrentIndex,
            Questions = set.Questions,
            SetAnswerPool = set.SetAnswerPool,
            Answers = session.Answers,
            CreatedAt = session.CreatedAt,
            UpdatedAt = session.UpdatedAt
        };
    }
}

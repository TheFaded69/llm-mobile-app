namespace GameModes.Contracts.V1;

public static class TestModes
{
    public const string TrueFalse = "true-false";
    public const string Questions = "questions";
    public const string Selection = "selection";
    public const string Written = "written";

    public static readonly HashSet<string> All =
    [
        TrueFalse,
        Questions,
        Selection,
        Written
    ];
}

public static class SessionStatuses
{
    public const string InProgress = "in_progress";
    public const string Submitted = "submitted";
    public const string Expired = "expired";
}

public class TestModeMeta
{
    public string Mode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool SupportsPerQuestionFeedback { get; set; }
    public bool SupportsFinalSubmitOnly { get; set; }
}

public class TestFiltersResponse
{
    public List<string> Difficulties { get; set; } = [];
    public Dictionary<string, List<string>> CategoriesByMode { get; set; } = new();
    public List<string> SectionDates { get; set; } = [];
    public List<string> Sort { get; set; } = [];
}

public class PaginationMeta
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public bool HasNext { get; set; }
}

public class SetCard
{
    public string Id { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public string DurationLabel { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int QuestionCount { get; set; }
    public int ProgressPercent { get; set; }
    public string SectionDate { get; set; } = string.Empty;
}

public class PagedSetCardResponse
{
    public List<SetCard> Items { get; set; } = [];
    public PaginationMeta Pagination { get; set; } = new();
}

public class SetMeta : SetCard
{
    public int TotalInCourse { get; set; }
}

public class SetDetailsResponse
{
    public SetMeta Set { get; set; } = new();
    public List<QuestionDto> Questions { get; set; } = [];
}

public class QuestionDto
{
    public string Kind { get; set; } = string.Empty;
    public string QuestionId { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string ExplainTerm { get; set; } = string.Empty;
    public string ExplainText { get; set; } = string.Empty;
    public string? Term { get; set; }
    public string? TermTranslation { get; set; }
    public List<string>? Options { get; set; }
    public List<string>? AnswerPool { get; set; }
}

public class CreateSessionRequest
{
    public string SetId { get; set; } = string.Empty;
    public string? DeviceId { get; set; }
}

public class SessionAnswerDto
{
    public string Kind { get; set; } = string.Empty;
    public string QuestionId { get; set; } = string.Empty;
    public bool? UserSaidTrue { get; set; }
    public int? SelectedIndex { get; set; }
    public string? SelectedLabel { get; set; }
    public string? UserText { get; set; }
}

public class SessionStateResponse
{
    public string SessionId { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public SetMeta Set { get; set; } = new();
    public string Status { get; set; } = SessionStatuses.InProgress;
    public int CurrentIndex { get; set; }
    public List<QuestionDto> Questions { get; set; } = [];
    public List<string>? SetAnswerPool { get; set; }
    public List<SessionAnswerDto> Answers { get; set; } = [];
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public class SubmitRequest
{
    public List<SessionAnswerDto> Answers { get; set; } = [];
    public DateTimeOffset? FinishedAt { get; set; }
}

public class ResultSummary
{
    public int CorrectCount { get; set; }
    public int Total { get; set; }
    public float Percent { get; set; }
}

public class SubmitResponse
{
    public string SessionId { get; set; } = string.Empty;
    public string Status { get; set; } = SessionStatuses.Submitted;
    public ResultSummary Result { get; set; } = new();
}

public class ResultResponse
{
    public string SessionId { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public string SetId { get; set; } = string.Empty;
    public int CorrectCount { get; set; }
    public int Total { get; set; }
    public List<ResultItemDto> Items { get; set; } = [];
}

public class ReviewResponse
{
    public string SessionId { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public string SetId { get; set; } = string.Empty;
    public List<ResultItemDto> Items { get; set; } = [];
}

public class ResultItemDto
{
    public string Kind { get; set; } = string.Empty;
    public string QuestionId { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string ExplainTerm { get; set; } = string.Empty;
    public string ExplainText { get; set; } = string.Empty;
    public string? Term { get; set; }
    public string? TermTranslation { get; set; }
    public bool? IsPairingCorrect { get; set; }
    public bool? UserSaidTrue { get; set; }
    public List<string>? Options { get; set; }
    public int? CorrectIndex { get; set; }
    public int? SelectedIndex { get; set; }
    public string? CorrectLabel { get; set; }
    public string? SelectedLabel { get; set; }
    public string? CorrectText { get; set; }
    public string? UserText { get; set; }
}

public class ErrorResponse
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, object>? Details { get; set; }
}

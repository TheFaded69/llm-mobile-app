namespace GameModes.Contracts.V1;

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

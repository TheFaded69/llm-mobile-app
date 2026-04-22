namespace GameModes.Infrastructure.Entities;

public class TestSessionEntity
{
    public string SessionId { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public string SetId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int CurrentIndex { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public List<SessionAnswerEntity> Answers { get; set; } = [];
}

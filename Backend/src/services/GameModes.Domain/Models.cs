using GameModes.Contracts.V1;

namespace GameModes.Domain;

public class TestSetAggregate
{
    public required SetMeta Meta { get; set; }
    public required List<QuestionDto> Questions { get; set; }
    public List<string>? SetAnswerPool { get; set; }
}

public class TestSessionAggregate
{
    public required string SessionId { get; set; }
    public required string Mode { get; set; }
    public required string SetId { get; set; }
    public required string Status { get; set; }
    public required int CurrentIndex { get; set; }
    public required List<SessionAnswerDto> Answers { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

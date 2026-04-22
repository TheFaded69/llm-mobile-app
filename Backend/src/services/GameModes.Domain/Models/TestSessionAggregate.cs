using GameModes.Contracts.V1;

namespace GameModes.Domain.Models;

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

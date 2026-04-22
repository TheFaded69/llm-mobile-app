using GameModes.Contracts.V1;

namespace GameModes.Domain.Models;

public class TestSetAggregate
{
    public required SetMeta Meta { get; set; }
    public required List<QuestionDto> Questions { get; set; }
    public List<string>? SetAnswerPool { get; set; }
}

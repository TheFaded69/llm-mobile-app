namespace GameModes.Contracts.V1;

public class SetDetailsResponse
{
    public SetMeta Set { get; set; } = new();
    public List<QuestionDto> Questions { get; set; } = [];
}

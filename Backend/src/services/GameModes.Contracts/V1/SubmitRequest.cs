namespace GameModes.Contracts.V1;

public class SubmitRequest
{
    public List<SessionAnswerDto> Answers { get; set; } = [];
    public DateTimeOffset? FinishedAt { get; set; }
}

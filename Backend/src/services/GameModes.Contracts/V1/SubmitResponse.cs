namespace GameModes.Contracts.V1;

public class SubmitResponse
{
    public string SessionId { get; set; } = string.Empty;
    public string Status { get; set; } = SessionStatuses.Submitted;
    public ResultSummary Result { get; set; } = new();
}

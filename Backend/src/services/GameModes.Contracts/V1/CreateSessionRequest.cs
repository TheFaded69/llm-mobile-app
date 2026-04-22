namespace GameModes.Contracts.V1;

public class CreateSessionRequest
{
    public string SetId { get; set; } = string.Empty;
    public string? DeviceId { get; set; }
}

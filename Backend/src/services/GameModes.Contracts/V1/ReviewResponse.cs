namespace GameModes.Contracts.V1;

public class ReviewResponse
{
    public string SessionId { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public string SetId { get; set; } = string.Empty;
    public List<ResultItemDto> Items { get; set; } = [];
}

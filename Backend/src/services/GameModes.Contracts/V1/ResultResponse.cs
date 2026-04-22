namespace GameModes.Contracts.V1;

public class ResultResponse
{
    public string SessionId { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public string SetId { get; set; } = string.Empty;
    public int CorrectCount { get; set; }
    public int Total { get; set; }
    public List<ResultItemDto> Items { get; set; } = [];
}

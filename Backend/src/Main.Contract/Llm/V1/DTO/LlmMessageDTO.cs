namespace Main.Contract.Llm.V1.DTO;

public sealed class LlmMessageDTO
{
    public string Role { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}

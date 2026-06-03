using Main.Contract.Llm.V1.DTO;

namespace Main.Contract.Llm.V1.Responses;

public sealed class SendLlmMessageResponse
{
    public string Text { get; set; } = string.Empty;

    public string? ResponseId { get; set; }

    public string Model { get; set; } = string.Empty;

    public List<LlmMessageDTO> History { get; set; } = [];
}

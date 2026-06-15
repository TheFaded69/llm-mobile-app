using Main.Contract.Llm.V1.DTO;

namespace Main.Contract.Llm.V1.Requests;

public sealed class SendLlmMessageRequest
{
    public string SystemPrompt { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public List<LlmMessageDTO> History { get; set; } = [];
}

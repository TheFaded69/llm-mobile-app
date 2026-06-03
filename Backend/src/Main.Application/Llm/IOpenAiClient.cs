using Main.Application.Llm.Models;

namespace Main.Application.Llm;

public interface IOpenAiClient
{
    Task<LlmChatResponse> SendMessageAsync(
        string systemPrompt,
        IReadOnlyCollection<LlmMessage> history,
        string text,
        CancellationToken cancellationToken);

    Task<LlmSpeechResponse> CreateSpeechAsync(
        string text,
        string? voice,
        string? instructions,
        string? responseFormat,
        CancellationToken cancellationToken);
}

namespace Main.Application.Llm.Models;

public sealed record LlmChatResponse(string Text, string? ResponseId, string Model);

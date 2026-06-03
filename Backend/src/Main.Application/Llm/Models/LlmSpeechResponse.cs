namespace Main.Application.Llm.Models;

public sealed record LlmSpeechResponse(byte[] Audio, string ContentType, string FileExtension);

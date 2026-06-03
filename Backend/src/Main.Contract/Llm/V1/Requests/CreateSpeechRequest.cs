namespace Main.Contract.Llm.V1.Requests;

public sealed class CreateSpeechRequest
{
    public string Text { get; set; } = string.Empty;

    public string? Voice { get; set; }

    public string? Instructions { get; set; }

    public string? ResponseFormat { get; set; }
}

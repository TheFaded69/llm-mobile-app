namespace Main.Application.Llm;

public class OpenAiOptions
{
    public const string SectionName = "OpenAi";

    public string ApiKey { get; set; } = string.Empty;

    public string BaseUrl { get; set; } = "https://api.openai.com";

    public string ChatModel { get; set; } = "gpt-4o-mini";

    public string SpeechModel { get; set; } = "gpt-4o-mini-tts";

    public string SpeechVoice { get; set; } = "coral";

    public string SpeechResponseFormat { get; set; } = "mp3";
}

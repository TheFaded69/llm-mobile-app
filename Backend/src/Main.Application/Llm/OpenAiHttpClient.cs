using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Main.Application.Llm.Models;
using Microsoft.Extensions.Options;

namespace Main.Application.Llm;

public sealed class OpenAiHttpClient : IOpenAiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly HttpClient _httpClient;
    private readonly OpenAiOptions _options;

    public OpenAiHttpClient(HttpClient httpClient, IOptions<OpenAiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<LlmChatResponse> SendMessageAsync(
        string systemPrompt,
        IReadOnlyCollection<LlmMessage> history,
        string text,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(systemPrompt))
        {
            throw new ArgumentException("System prompt is required.", nameof(systemPrompt));
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Message text is required.", nameof(text));
        }

        var messages = new List<OpenAiChatMessage>
        {
            new("system", systemPrompt)
        };

        messages.AddRange(history.Select(message => new OpenAiChatMessage(message.Role, message.Content)));
        messages.Add(new OpenAiChatMessage("user", text));

        var request = new OpenAiChatCompletionRequest(_options.ChatModel, messages);
        var httpRequest = CreateJsonRequest(HttpMethod.Post, "/v1/chat/completions", request);

        using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
        var payload = await ReadJsonResponseAsync<OpenAiChatCompletionResponse>(response, cancellationToken);
        var assistantMessage = payload.Choices.FirstOrDefault()?.Message?.Content;

        if (string.IsNullOrWhiteSpace(assistantMessage))
        {
            throw new InvalidOperationException("OpenAI response does not contain an assistant message.");
        }

        return new LlmChatResponse(assistantMessage, payload.Id, payload.Model ?? _options.ChatModel);
    }

    public async Task<LlmSpeechResponse> CreateSpeechAsync(
        string text,
        string? voice,
        string? instructions,
        string? responseFormat,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Speech text is required.", nameof(text));
        }

        var format = string.IsNullOrWhiteSpace(responseFormat)
            ? _options.SpeechResponseFormat
            : responseFormat;

        var request = new OpenAiSpeechRequest(
            _options.SpeechModel,
            text,
            string.IsNullOrWhiteSpace(voice) ? _options.SpeechVoice : voice,
            instructions,
            format);

        var httpRequest = CreateJsonRequest(HttpMethod.Post, "/v1/audio/speech", request);

        using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(
                $"OpenAI request failed with status code {(int)response.StatusCode}: {error}",
                null,
                response.StatusCode);
        }

        var contentType = response.Content.Headers.ContentType?.MediaType ?? GetContentType(format);
        var audio = await response.Content.ReadAsByteArrayAsync(cancellationToken);

        return new LlmSpeechResponse(audio, contentType, format);
    }

    private HttpRequestMessage CreateJsonRequest<TBody>(HttpMethod method, string requestUri, TBody body)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new InvalidOperationException("OpenAI API key is not configured.");
        }

        var request = new HttpRequestMessage(method, requestUri)
        {
            Content = JsonContent.Create(body, options: JsonOptions)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

        return request;
    }

    private static async Task<T> ReadJsonResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(
                $"OpenAI request failed with status code {(int)response.StatusCode}: {error}",
                null,
                response.StatusCode);
        }

        var payload = await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken);
        return payload ?? throw new InvalidOperationException("OpenAI returned an empty response body.");
    }

    private static string GetContentType(string? responseFormat) => responseFormat?.ToLowerInvariant() switch
    {
        "opus" => "audio/opus",
        "aac" => "audio/aac",
        "flac" => "audio/flac",
        "wav" => "audio/wav",
        "pcm" => "audio/pcm",
        _ => "audio/mpeg"
    };

    private sealed record OpenAiChatCompletionRequest(
        [property: JsonPropertyName("model")] string Model,
        [property: JsonPropertyName("messages")] IReadOnlyCollection<OpenAiChatMessage> Messages);

    private sealed record OpenAiChatMessage(
        [property: JsonPropertyName("role")] string Role,
        [property: JsonPropertyName("content")] string Content);

    private sealed record OpenAiChatCompletionResponse(
        [property: JsonPropertyName("id")] string? Id,
        [property: JsonPropertyName("model")] string? Model,
        [property: JsonPropertyName("choices")] IReadOnlyCollection<OpenAiChoice> Choices);

    private sealed record OpenAiChoice(
        [property: JsonPropertyName("message")] OpenAiChatMessage? Message);

    private sealed record OpenAiSpeechRequest(
        [property: JsonPropertyName("model")] string Model,
        [property: JsonPropertyName("input")] string Input,
        [property: JsonPropertyName("voice")] string Voice,
        [property: JsonPropertyName("instructions")] string? Instructions,
        [property: JsonPropertyName("response_format")] string? ResponseFormat);
}

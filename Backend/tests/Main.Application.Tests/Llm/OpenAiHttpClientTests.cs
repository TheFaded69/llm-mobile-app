using System.Net;
using System.Text;
using System.Text.Json;
using Main.Application.Llm;
using Main.Application.Llm.Models;
using Microsoft.Extensions.Options;

namespace Main.Application.Tests.Llm;

public sealed class OpenAiHttpClientTests
{
    [Fact]
    public async Task SendMessageAsync_SendsSystemHistoryAndUserMessage_ReturnsAssistantText()
    {
        using var handler = new TestHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
                """
                {
                  "id": "chatcmpl_123",
                  "model": "gpt-4o-mini",
                  "choices": [
                    {
                      "message": {
                        "role": "assistant",
                        "content": "Привет!"
                      }
                    }
                  ]
                }
                """,
                Encoding.UTF8,
                "application/json")
        });
        var client = CreateClient(handler);

        var response = await client.SendMessageAsync(
            "Ты помощник для обучения языкам.",
            [new LlmMessage("assistant", "Здравствуйте!")],
            "Скажи привет",
            CancellationToken.None);

        Assert.Equal("Привет!", response.Text);
        Assert.Equal("chatcmpl_123", response.ResponseId);
        Assert.Equal("gpt-4o-mini", response.Model);
        Assert.Equal(HttpMethod.Post, handler.Request!.Method);
        Assert.Equal("/v1/chat/completions", handler.Request.RequestUri!.PathAndQuery);
        Assert.Equal("Bearer", handler.Request.Headers.Authorization!.Scheme);
        Assert.Equal("test-key", handler.Request.Headers.Authorization.Parameter);

        var body = await ReadJsonBodyAsync(handler.Request);
        Assert.Equal("gpt-4o-mini", body.RootElement.GetProperty("model").GetString());

        var messages = body.RootElement.GetProperty("messages").EnumerateArray().ToList();
        Assert.Collection(
            messages,
            message => AssertMessage(message, "system", "Ты помощник для обучения языкам."),
            message => AssertMessage(message, "assistant", "Здравствуйте!"),
            message => AssertMessage(message, "user", "Скажи привет"));
    }

    [Fact]
    public async Task CreateSpeechAsync_SendsTextToSpeechRequest_ReturnsAudioBytes()
    {
        var expectedAudio = new byte[] { 1, 2, 3 };
        using var handler = new TestHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(expectedAudio)
            {
                Headers =
                {
                    ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/wav")
                }
            }
        });
        var client = CreateClient(handler);

        var response = await client.CreateSpeechAsync(
            "Текст для озвучивания",
            "alloy",
            "Говори спокойно",
            "wav",
            CancellationToken.None);

        Assert.Equal(expectedAudio, response.Audio);
        Assert.Equal("audio/wav", response.ContentType);
        Assert.Equal("wav", response.FileExtension);
        Assert.Equal(HttpMethod.Post, handler.Request!.Method);
        Assert.Equal("/v1/audio/speech", handler.Request.RequestUri!.PathAndQuery);

        var body = await ReadJsonBodyAsync(handler.Request);
        Assert.Equal("gpt-4o-mini-tts", body.RootElement.GetProperty("model").GetString());
        Assert.Equal("Текст для озвучивания", body.RootElement.GetProperty("input").GetString());
        Assert.Equal("alloy", body.RootElement.GetProperty("voice").GetString());
        Assert.Equal("Говори спокойно", body.RootElement.GetProperty("instructions").GetString());
        Assert.Equal("wav", body.RootElement.GetProperty("response_format").GetString());
    }

    private static OpenAiHttpClient CreateClient(HttpMessageHandler handler)
    {
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.openai.com")
        };

        return new OpenAiHttpClient(
            httpClient,
            Options.Create(new OpenAiOptions
            {
                ApiKey = "test-key"
            }));
    }

    private static async Task<JsonDocument> ReadJsonBodyAsync(HttpRequestMessage request)
    {
        var body = await request.Content!.ReadAsStringAsync();
        return JsonDocument.Parse(body);
    }

    private static void AssertMessage(JsonElement message, string role, string content)
    {
        Assert.Equal(role, message.GetProperty("role").GetString());
        Assert.Equal(content, message.GetProperty("content").GetString());
    }

    private sealed class TestHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _handler;

        public TestHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
        {
            _handler = handler;
        }

        public HttpRequestMessage? Request { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;
            return Task.FromResult(_handler(request));
        }
    }
}

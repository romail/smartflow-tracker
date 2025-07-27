
namespace SmartFlow.Tracker.AI.Services
{
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using Microsoft.Extensions.Configuration;
    using SmartFlow.Tracker.Application.AI.Interfaces;


    public class OpenAIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _model;
        private readonly double _temperature;

        public OpenAIService(IConfiguration configuration)
        {
            var apiKey = configuration["OpenAI:ApiKey"]
                ?? throw new InvalidOperationException("OpenAI:ApiKey not found");

            _model = configuration["OpenAI:Model"] ?? "gpt-3.5-turbo";
            _temperature = double.TryParse(configuration["OpenAI:Temperature"], out var t) ? t : 0.7;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.openai.com/v1/")
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public async Task<IReadOnlyCollection<string>> GenerateTasksAsync(string description, CancellationToken cancellationToken = default)
        {
            var prompt = $"""
                Ты — ассистент по управлению задачами. Пользователь описал цель:
                "{description}"
                Верни 3–5 подзадач в формате JSON-массива строк, без пояснений.
                """;

            var request = new
            {
                model = _model,
                temperature = _temperature,
                messages = new[]
                {
                    new { role = "system", content = "Ты — помощник по проектному менеджменту." },
                    new { role = "user", content = prompt }
                }
            };

            var response = await _httpClient.PostAsync(
                "chat/completions",
                new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"),
                cancellationToken
            );

            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            var text = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return JsonSerializer.Deserialize<string[]>(text ?? "[]")!;
        }
    }
}

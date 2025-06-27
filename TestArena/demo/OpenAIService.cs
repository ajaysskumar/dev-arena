
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class OpenAIService
{
    private readonly HttpClient _httpClient;

    public OpenAIService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string?> GetMovieSummaryAsync(string movieTitle, string apiKey)
    {
        var url = "https://api.openai.com/v1/chat/completions";
        var requestBody = new
        {
            model = "gpt-4o-mini",
            messages = new[]
            {
                new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = $"Give me a summary of the movie {movieTitle}. Max under 250 tokens." }
            },
            max_tokens = 300
        };

        var json = JsonSerializer.Serialize(requestBody);
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("Authorization", $"Bearer {apiKey}");
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString); // For debugging purposes
        using var doc = JsonDocument.Parse(responseString);
        return doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();
    }
}
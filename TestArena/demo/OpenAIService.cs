
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

public class OpenAIService
{
    private readonly HttpClient _httpClient;

    public OpenAIService()
    {
        _httpClient = new HttpClient();
    }

    /// <summary>
    /// Represents movie details created by the model.
    /// </summary>
    public record MovieDetails(string Title, int Year, string Director, string[] Genres, string[] Actors);

    /// <summary>
    /// Create movie details using a movie name. The model is instructed to return a
    /// single JSON object with the structure below. The method validates and trims
    /// fields to the requested limits:
    /// - Title: max 100 chars
    /// - Year: valid year integer
    /// - Director: max 80 chars
    /// - Genres: up to 5 items, each max 30 chars
    /// - Actors: up to 10 items, each max 60 chars
    /// </summary>
    /// <param name="movieName">A movie name to get details for.</param>
    /// <param name="apiKey">OpenAI API key.</param>
    /// <returns>A <see cref="MovieDetails"/> instance, or null if the model could not produce valid JSON.</returns>
    public async Task<MovieDetails?> CreateMovieDetailsAsync(string movieName, string apiKey)
    {
        var url = "https://api.openai.com/v1/chat/completions";

        var systemInstruction =
            "You are a JSON generator. Given a movie name, produce a single JSON object ONLY (no surrounding text, no markdown) with the exact shape:\n" +
            "{ \"title\": string, \"year\": number, \"director\": string, \"genres\": [string,...], \"actors\": [string,...] }\n" +
            "Constraints: title must be max 100 characters; year must be a valid year number; director must be max 80 characters; genres must be an array with at most 5 items; each genre max 30 characters; actors must be an array with at most 10 items; each actor max 60 characters.\n" +
            "If you cannot create values that meet these constraints, truncate fields to the required length. Respond only with a single JSON object and nothing else.";

        var functions = new[]
        {
            new
            {
                name = "create_movie_details",
                description = "Create a movie details object with title, year, director, genres and actors.",
                parameters = new
                {
                    type = "object",
                    properties = new
                    {
                        title = new { type = "string", maxLength = 100, description = "Movie title (max 100 chars)" },
                        year = new { type = "integer", description = "Release year" },
                        director = new { type = "string", maxLength = 80, description = "Director name (max 80 chars)" },
                        genres = new
                        {
                            type = "array",
                            maxItems = 5,
                            items = new { type = "string", maxLength = 30 }
                        },
                        actors = new
                        {
                            type = "array",
                            maxItems = 10,
                            items = new { type = "string", maxLength = 60 }
                        }
                    },
                    required = new[] { "title", "year", "director", "genres", "actors" }
                }
            }
        };

        var requestBody = new
        {
            model = "gpt-4o-mini",
            messages = new[]
            {
                new { role = "system", content = systemInstruction },
                new { role = "user", content = $"Get movie details for: {movieName}\nRespond by calling the function 'create_movie_details' with the JSON object as arguments." }
            },
            functions = functions,
            function_call = "auto",
            temperature = 0.0,
            max_tokens = 600
        };

        var json = JsonSerializer.Serialize(requestBody);
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("Authorization", $"Bearer {apiKey}");
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(responseString);
        string? content = null;
        try
        {
            var message = doc.RootElement.GetProperty("choices")[0].GetProperty("message");
            if (message.TryGetProperty("function_call", out var fc) && fc.ValueKind == JsonValueKind.Object)
            {
                content = fc.GetProperty("arguments").GetString();
            }
            else if (message.TryGetProperty("content", out var msgContent) && msgContent.ValueKind == JsonValueKind.String)
            {
                content = msgContent.GetString();
            }
        }
        catch
        {
            var firstBrace = responseString.IndexOf('{');
            var lastBrace = responseString.LastIndexOf('}');
            if (firstBrace >= 0 && lastBrace > firstBrace)
            {
                content = responseString.Substring(firstBrace, lastBrace - firstBrace + 1);
            }
        }

        if (string.IsNullOrWhiteSpace(content)) return null;

        var start = content.IndexOf('{');
        var end = content.LastIndexOf('}');
        if (start >= 0 && end > start)
        {
            content = content.Substring(start, end - start + 1);
        }

        try
        {
            using var parsed = JsonDocument.Parse(content);
            var root = parsed.RootElement;

            var title = root.GetProperty("title").GetString() ?? string.Empty;
            var year = root.TryGetProperty("year", out var yr) && yr.ValueKind == JsonValueKind.Number
                ? yr.GetInt32()
                : 0;
            var director = root.TryGetProperty("director", out var dir) && dir.ValueKind == JsonValueKind.String
                ? dir.GetString() ?? string.Empty
                : string.Empty;

            var genres = new List<string>();
            if (root.TryGetProperty("genres", out var gen) && gen.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in gen.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.String)
                    {
                        genres.Add(item.GetString() ?? string.Empty);
                        if (genres.Count >= 5) break;
                    }
                }
            }

            var actors = new List<string>();
            if (root.TryGetProperty("actors", out var act) && act.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in act.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.String)
                    {
                        actors.Add(item.GetString() ?? string.Empty);
                        if (actors.Count >= 10) break;
                    }
                }
            }

            // Enforce length constraints by trimming if necessary
            title = title.Trim();
            if (title.Length > 100) title = title.Substring(0, 100);

            director = director.Trim();
            if (director.Length > 80) director = director.Substring(0, 80);

            for (int i = 0; i < genres.Count; i++)
            {
                var genre = genres[i].Trim();
                if (genre.Length > 30) genre = genre.Substring(0, 30);
                genres[i] = genre;
            }

            for (int i = 0; i < actors.Count; i++)
            {
                var actor = actors[i].Trim();
                if (actor.Length > 60) actor = actor.Substring(0, 60);
                actors[i] = actor;
            }

            return new MovieDetails(title, year, director, genres.ToArray(), actors.ToArray());
        }
        catch (JsonException)
        {
            return null;
        }
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
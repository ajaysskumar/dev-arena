
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
    public record Recipe(string Title, string TotalTime, string CountryOfOrigin, string[] Ingredients, string[] Steps);

    /// <summary>
        /// Create a recipe using a short description. The model is instructed to return a
        /// single JSON object with the structure below. The method validates and trims
        /// fields to the requested limits:
        /// - Title: max 75 chars
        /// - Ingredients: up to 12 items, each max 80 chars
        /// - Steps: up to 10 items, each max 300 chars
        /// </summary>
        /// <param name="shortDescription">A short prompt / description of the desired recipe.</param>
        /// <param name="apiKey">OpenAI API key.</param>
        /// <returns>A <see cref="Recipe"/> instance, or null if the model could not produce valid JSON.</returns>
        public async Task<Recipe?> CreateRecipeAsync(string shortDescription, string apiKey)
        {
            var url = "https://api.openai.com/v1/chat/completions";

            var systemInstruction =
                "You are a JSON generator. Given a short description, produce a single JSON object ONLY (no surrounding text, no markdown) with the exact shape:\n" +
                "{ \"title\": string, \"totalTime\": string, \"countryOfOrigin\": string, \"ingredients\": [string,...], \"steps\": [string,...] }\n" +
                "Constraints: title must be max 75 characters; totalTime must be a short human-readable string max 30 characters (e.g. \"45 minutes\"); countryOfOrigin must be max 50 characters; ingredients must be an array with at most 12 items; each ingredient max 80 characters; steps must be an array with at most 10 items; each step max 300 characters.\n" +
                "If you cannot create values that meet these constraints, truncate fields to the required length. Respond only with a single JSON object and nothing else.";

            var functions = new[]
            {
                new
                {
                    name = "create_recipe",
                    description = "Create a recipe object with title, ingredients and steps.",
                    parameters = new
                    {
                        type = "object",
                        properties = new
                        {
                            title = new { type = "string", maxLength = 75, description = "Recipe title (max 75 chars)" },
                            totalTime = new { type = "string", maxLength = 30, description = "Total time (e.g. '45 minutes')" },
                            countryOfOrigin = new { type = "string", maxLength = 50, description = "Country or region of origin" },
                            ingredients = new
                            {
                                type = "array",
                                maxItems = 12,
                                items = new { type = "string", maxLength = 80 }
                            },
                            steps = new
                            {
                                type = "array",
                                maxItems = 10,
                                items = new { type = "string", maxLength = 300 }
                            }
                        },
                        required = new[] { "title", "ingredients", "steps" }
                    }
                }
            };

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = systemInstruction },
                    new { role = "user", content = $"Create a recipe for: {shortDescription}\nRespond by calling the function 'create_recipe' with the JSON object as arguments." }
                },
                functions = functions,
                function_call = "auto",
                temperature = 0.0,
                max_tokens = 800
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
                var totalTime = root.TryGetProperty("totalTime", out var tt) && tt.ValueKind == JsonValueKind.String
                    ? tt.GetString() ?? string.Empty
                    : string.Empty;

                var countryOfOrigin = root.TryGetProperty("countryOfOrigin", out var co) && co.ValueKind == JsonValueKind.String
                    ? co.GetString() ?? string.Empty
                    : string.Empty;

                var ingredients = new List<string>();
                if (root.TryGetProperty("ingredients", out var ing) && ing.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in ing.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.String)
                        {
                            ingredients.Add(item.GetString() ?? string.Empty);
                            if (ingredients.Count >= 12) break;
                        }
                    }
                }

                var steps = new List<string>();
                if (root.TryGetProperty("steps", out var st) && st.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in st.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.String)
                        {
                            steps.Add(item.GetString() ?? string.Empty);
                            if (steps.Count >= 10) break;
                        }
                    }
                }

                // Enforce length constraints by trimming if necessary
                title = title.Trim();
                if (title.Length > 75) title = title.Substring(0, 75);

                totalTime = totalTime.Trim();
                if (totalTime.Length > 30) totalTime = totalTime.Substring(0, 30);

                countryOfOrigin = countryOfOrigin.Trim();
                if (countryOfOrigin.Length > 50) countryOfOrigin = countryOfOrigin.Substring(0, 50);

                for (int i = 0; i < ingredients.Count; i++)
                {
                    var line = ingredients[i].Trim();
                    if (line.Length > 80) line = line.Substring(0, 80);
                    ingredients[i] = line;
                }

                for (int i = 0; i < steps.Count; i++)
                {
                    var line = steps[i].Trim();
                    if (line.Length > 300) line = line.Substring(0, 300);
                    steps[i] = line;
                }

                return new Recipe(title, totalTime, countryOfOrigin, ingredients.ToArray(), steps.ToArray());
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
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

// Minimal in-house streaming API demo for blog article
public class StreamingApiDemo
{
    // Server that streams data
    public class StreamingServer
    {
        private HttpListener _listener;
        private bool _running;

        public async Task StartAsync(string prefix = "http://localhost:8080/")
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(prefix);
            _listener.Start();
            _running = true;

            Console.WriteLine($"Server listening on {prefix}");

            while (_running)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    _ = HandleRequest(context);
                }
                catch { }
            }
        }

        private async Task HandleRequest(HttpListenerContext context)
        {
            var response = context.Response;
            response.ContentType = "text/event-stream";
            response.Headers.Add("Cache-Control", "no-cache");
            response.Headers.Add("Connection", "keep-alive");

            // Stream data in Server-Sent Events format
            using var writer = new StreamWriter(response.OutputStream, Encoding.UTF8, leaveOpen: true);

            // Simulate streaming data
            var messages = new[]
            {
                "Starting data transmission",
                "Chunk 1: Processing request",
                "Chunk 2: Analyzing input",
                "Chunk 3: Generating response",
                "Chunk 4: Almost done",
                "Chunk 5: Complete!"
            };

            foreach (var msg in messages)
            {
                // Send in SSE format: data: <content>\n\n
                await writer.WriteLineAsync($"data: {msg}");
                await writer.WriteLineAsync();
                await writer.FlushAsync();
                await Task.Delay(1500); // Simulate processing delay
            }

            // Signal completion
            await writer.WriteLineAsync("data: [DONE]");
            await writer.FlushAsync();
            response.Close();
        }

        public void Stop()
        {
            _running = false;
            _listener?.Close();
        }
    }

    // Client that consumes the stream
    public static async Task ConsumerAsync()
    {
        using var client = new HttpClient();

        // Request the stream endpoint
        using var response = await client.GetAsync("http://localhost:8080/stream",
            HttpCompletionOption.ResponseHeadersRead);

        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        Console.WriteLine("\n--- Client receiving stream ---\n");

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line)) continue;

            // Parse SSE format
            if (line.StartsWith("data: "))
            {
                var data = line.Substring("data: ".Length);
                if (data == "[DONE]")
                {
                    Console.WriteLine("\n[Stream ended]");
                    break;
                }

                Console.WriteLine($"Received: {data}");
            }
        }
    }

    public static async Task Main(string[] args)
    {
        var server = new StreamingServer();

        // Start server in background
        var serverTask = server.StartAsync();
        await Task.Delay(1000); // Give server time to start

        // Run client to consume the stream
        try
        {
            await ConsumerAsync();
        }
        finally
        {
            server.Stop();
        }
    }
}

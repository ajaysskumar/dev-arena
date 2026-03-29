# AWS Bedrock Optimisation Techniques

## 1 — Model Selection for Cost

The single biggest lever. Claude models span a 10× cost range:

| Model | Speed | Cost | Best for |
|---|---|---|---|
| `claude-3-haiku` | Fastest | Cheapest | Classification, short answers, high-volume |
| `claude-3-5-sonnet` | Fast | Mid | Balanced default for most features |
| `claude-3-opus` | Slower | Most expensive | Complex reasoning, long-doc analysis |

**Rule:** Start integration with Haiku. Only move up the tier if quality evaluation fails.

---

## 2 — Token Budgeting

Every dollar you spend on Bedrock is measured in tokens. Total cost = `(InputTokens × inputPrice) + (OutputTokens × outputPrice)`.

### Set `MaxTokens` explicitly

Never leave `MaxTokens` unset — the model will use the full context window by default.

```csharp
InferenceConfig = new()
{
    MaxTokens = 256,   // Right-size to your use case
    Temperature = 0.3f // Lower = more deterministic = fewer wasted tokens on wandering
}
```

### Track usage per request

```csharp
var response = await client.ConverseAsync(request);
var usage = response.Usage;
logger.LogInformation(
    "Tokens — Input: {Input}, Output: {Output}, Total: {Total}",
    usage.InputTokens, usage.OutputTokens,
    usage.InputTokens + usage.OutputTokens);
```

### Implement a token budget guard

```csharp
const int MaxContextTokens = 180_000; // Claude's limit; leave headroom

int totalTokensUsed = conversationHistory
    .Sum(m => EstimateTokens(m)); // rough estimate: chars / 4

if (totalTokensUsed > MaxContextTokens * 0.8)
    SummariseOldestTurns(conversationHistory);
```

---

## 3 — System Prompt Caching (Prompt Caching — Claude)

If your system prompt is long (500+ tokens) and repeated across requests, Anthropic's prompt caching can save up to **90% of input token cost** for the cached prefix.

```csharp
var systemPrompt = new SystemContentBlock
{
    Text = longSystemPromptText,
    // Mark as cacheable — charged at cache write rate on first call,
    // cache read rate on subsequent calls
    CacheControl = new CachePointBlock { Type = CacheControlType.Ephemeral }
};

var request = new ConverseRequest
{
    ModelId = "anthropic.claude-3-5-sonnet-20240620-v1:0",
    System = new List<SystemContentBlock> { systemPrompt },
    Messages = messages
};
```

**When it applies:** System prompts > 1024 tokens, used across many sessions. Not effective for one-off requests.

---

## 4 — Streaming for Perceived Latency

Streaming does not reduce token cost or model latency, but it dramatically improves **perceived performance** for chat UIs and long generation tasks.

```csharp
var streamResponse = await client.ConverseStreamAsync(new ConverseStreamRequest
{
    ModelId = "anthropic.claude-3-haiku-20240307-v1:0",
    Messages = messages,
    InferenceConfig = new() { MaxTokens = 1024 }
});

var sb = new StringBuilder();
await foreach (var chunk in streamResponse.Stream.AsEnumerableAsync())
{
    if (chunk is ContentBlockDeltaEvent delta && delta.Delta?.Text is not null)
    {
        sb.Append(delta.Delta.Text);
        // Flush chunk to UI here (SignalR, SSE, etc.)
        await SendChunkToClient(delta.Delta.Text);
    }
    else if (chunk is MessageStopEvent stop)
    {
        // stop.AdditionalModelResponseFields contains usage if needed
        break;
    }
}
```

---

## 5 — Temperature and Top-P Tuning

| Setting | Value | Effect |
|---|---|---|
| `Temperature` | 0.0 | Fully deterministic, cheapest for classification |
| `Temperature` | 0.3–0.5 | Balanced — good default for chat |
| `Temperature` | 0.9–1.0 | Creative / generative tasks |
| `TopP` | 0.9 | Restricts vocabulary to top 90% probability mass |

Lower temperature → tighter output → fewer tokens wasted on rambling.

---

## 6 — Context Window Management for Long Conversations

Sending the full conversation history on every turn gets expensive fast.

### Strategy 1 — Rolling window
Keep only the last N turns:
```csharp
const int MaxTurns = 10;
if (conversationHistory.Count > MaxTurns * 2) // each turn = 2 messages
    conversationHistory = conversationHistory.TakeLast(MaxTurns * 2).ToList();
```

### Strategy 2 — Summarisation
When approaching the context limit, summarise older turns with a short model call, replace them with the summary:
```csharp
var summaryRequest = new ConverseRequest
{
    ModelId = "anthropic.claude-3-haiku-20240307-v1:0", // cheapest for summarisation
    Messages = new() { new Message
    {
        Role = ConversationRole.User,
        Content = new() { new ContentBlock
        {
            Text = $"Summarise this conversation in 3 sentences:\n\n{FormatHistory(oldTurns)}"
        }}
    }},
    InferenceConfig = new() { MaxTokens = 200 }
};
```

### Strategy 3 — External memory (RAG)
For very long sessions, store conversation chunks in a vector store (e.g., OpenSearch, pgvector) and retrieve semantically relevant history instead of sending everything.

---

## 7 — Batching and Async Throughput

When real-time response is not required (bulk classification, nightly summarisation):
- Use `Task.WhenAll` with bounded parallelism to respect Bedrock throttling limits.
- Set per-account throttling alerts in CloudWatch before scaling batch jobs.

```csharp
var semaphore = new SemaphoreSlim(5); // max 5 concurrent Bedrock calls
var tasks = items.Select(async item =>
{
    await semaphore.WaitAsync();
    try { return await InvokeBedrockAsync(item); }
    finally { semaphore.Release(); }
});
var results = await Task.WhenAll(tasks);
```

---

## 8 — Cost Monitoring

Set CloudWatch alarms on:
- `bedrock:InputTokenCount` exceeding daily budget
- `bedrock:OutputTokenCount` exceeding daily budget
- `bedrock:ThrottledRequests` > 0 sustained for 5 minutes (signals you need a quota increase)

Tag all Bedrock clients with `aws:RequestTag` to attribute costs per feature/team:
```csharp
// Use AWS Cost Allocation Tags on the IAM role or resource policy
// to split Bedrock costs by workload in Cost Explorer
```

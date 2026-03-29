---
name: aws-bedrock
description: 'AWS Bedrock tech stack best practices for .NET from development to production. USE FOR: implementing Bedrock Invoke API, Converse API, Converse with Tools, Guardrails, Claude model integration, AWS SDK setup, IAM authentication, EC2/Lambda/ECS credential patterns, token optimization, cost reduction, streaming responses, CloudWatch observability, distributed tracing, production hardening, error handling, retry policies, model selection. Trigger phrases: aws bedrock, converse api, invoke api, claude model, bedrock guardrails, bedrock tools, bedrock streaming, bedrock production, bedrock observability, bedrock tracing, bedrock cost, bedrock optimization, AmazonBedrockRuntimeClient, AWSSDK.BedrockRuntime. DO NOT USE FOR: general AI/ML theory, non-Bedrock AWS services, OpenAI-specific patterns.'
argument-hint: 'Describe the Bedrock feature or scenario (e.g., "Converse API with streaming", "production auth on ECS", "guardrails with PII redaction")'
---

# AWS Bedrock Skill

Expert guidance for building, optimising, and operating AWS Bedrock workloads in **.NET (C#)** from local development through to production.

## When to Use

Load this skill when the user asks about:
- Setting up Bedrock for local dev **or** production
- Choosing between Invoke API vs Converse API vs ConverseStream API
- Implementing Converse with Tools (structured output / function calling)
- Configuring Guardrails (content filters, topic denial, PII redaction)
- Authentication patterns: default credentials, session tokens, EC2 roles, ECS task roles, Lambda execution roles
- Token budgeting, streaming, and cost optimisation
- Observability: CloudWatch Metrics, Logs, X-Ray tracing
- Error handling, retry policies, and circuit breaking
- Model selection across Anthropic Claude, Amazon Titan, Meta Llama, Mistral

---

## Core Packages

```bash
dotnet add package AWSSDK.Bedrock          # management plane (list/describe models, guardrails)
dotnet add package AWSSDK.BedrockRuntime   # data plane (invoke, converse, stream)
```

Minimum version: `AWSSDK.BedrockRuntime 3.7.0` or later.

---

## Step-by-Step Procedures

### 1 — Setting Up (Local Development)

1. Run `aws configure` and enter your credentials.
2. The SDK automatically reads `~/.aws/credentials` — no code changes needed.
3. For quick credential validation, list available models:

```csharp
var bedrockClient = new AmazonBedrockClient(RegionEndpoint.USEast1);
var models = await bedrockClient.ListFoundationModelsAsync(new());
```

4. Before invoking a model, enable it in **AWS Console → Bedrock → Model access** (takes 5–15 min the first time).
5. Consult the [authentication reference](./references/authentication.md) for production auth patterns.

**Common local dev pitfalls → quick fixes:**

| Symptom | Fix |
|---|---|
| `UnauthorizedException` | Check `~/.aws/credentials` or run `aws sts get-caller-identity` |
| `ResourceNotFoundException` | Enable model access in the console |
| Model not available | Verify region — Claude 3 Haiku is `us-east-1` / `us-west-2` only |
| Package type conflicts | Alias ambiguous types (see Step 4 below) |

---

### 2 — Choosing the Right API

| Scenario | API to Use |
|---|---|
| Single-shot generation, summarisation, one-off prompts | `InvokeModelAsync` |
| Multi-turn chat with context retention | `ConverseAsync` |
| Multi-turn chat with **streaming** tokens | `ConverseStreamAsync` |
| Structured output / function calling | `ConverseAsync` + ToolConfig |
| Guardrails enforcement | Pass `GuardrailConfig` to Converse or Invoke |

**Rule of thumb:** Default to `ConverseAsync` for all new features — it supports all models through a single contract, handles message history natively, and is easier to migrate to streaming later.

---

### 3 — Converse API (Multi-Turn Chat)

```csharp
var client = new AmazonBedrockRuntimeClient(RegionEndpoint.USEast1);

var request = new ConverseRequest
{
    ModelId = "anthropic.claude-3-haiku-20240307-v1:0",
    Messages = conversationHistory, // List<Message> — you own the history
    InferenceConfig = new() { MaxTokens = 512, Temperature = 0.7f }
};

var response = await client.ConverseAsync(request);
var reply = response.Output.Message.Content[0].Text;

// Append to history for next turn
conversationHistory.Add(response.Output.Message);
```

**Context window management:**
- Track total token count across turns using `response.Usage` (`InputTokens`, `OutputTokens`).
- When approaching the model's context limit, summarise older turns instead of truncating silently.
- See [optimisation reference](./references/optimization.md) for token budgeting patterns.

---

### 4 — Converse with Tools (Structured Output / Function Calling)

When you need guaranteed JSON output or want the model to trigger real actions:

```csharp
var toolConfig = new ToolConfiguration
{
    Tools = new List<Tool>
    {
        new()
        {
            ToolSpec = new ToolSpecification
            {
                Name = "get_movie_info",
                Description = "Returns structured movie information",
                InputSchema = new ToolInputSchema
                {
                    Json = Document.FromObject(new
                    {
                        type = "object",
                        properties = new
                        {
                            title = new { type = "string" },
                            year  = new { type = "integer" },
                            rating = new { type = "number" }
                        },
                        required = new[] { "title", "year", "rating" }
                    })
                }
            }
        }
    }
};
```

**Processing the tool response:**
1. Check `response.StopReason == StopReason.ToolUse`.
2. Extract the tool call from `response.Output.Message.Content`.
3. Execute your real function with the parsed arguments.
4. Append both the assistant message and the tool result back into `conversationHistory`.
5. Call `ConverseAsync` again to get the final answer.

---

### 5 — Guardrails

Guardrails sit outside the model — they inspect both input and output independently.

**Three mechanisms:**

| Mechanism | What it does | Config property |
|---|---|---|
| Content Filters | Block hate, violence, insults, prompt injection at LOW/MEDIUM/HIGH | `ContentPolicyConfig` |
| Topic Denial | Refuse specific topics entirely (e.g., coding help in a support bot) | `TopicPolicyConfig` |
| PII Redaction | Anonymise emails, phones, names, IPs — ANONYMIZE or BLOCK | `SensitiveInformationPolicyConfig` |

**Namespace conflict fix** (both SDK packages define overlapping types):
```csharp
using BedrockGuardrailConfiguration =
    Amazon.BedrockRuntime.Model.GuardrailConfiguration;
using BedrockContentFilterType =
    Amazon.BedrockRuntime.Model.GuardrailContentFilterType;
```

**Production checklist for Guardrails:**
- [ ] Always publish a **versioned** guardrail before attaching to production (`PublishGuardrailAsync`).
- [ ] Use `DRAFT` version only in dev/staging.
- [ ] Record guardrail identifier + version in app config — never hardcode in source.
- [ ] Monitor `guardrail_intervened` metric in CloudWatch (see [observability reference](./references/observability.md)).

---

### 6 — Authentication (Production)

See [authentication reference](./references/authentication.md) for full patterns.

**Quick guide:**

| Environment | Recommended approach |
|---|---|
| Local dev | `aws configure` → SDK auto-reads `~/.aws/credentials` |
| EC2 | Instance Profile Role — no code changes, SDK auto-discovers |
| ECS/Fargate | Task Role — attach to task definition, SDK auto-discovers |
| Lambda | Execution Role — attach BedrockRuntime permissions, SDK auto-discovers |
| CI/CD (GitHub Actions) | OIDC → `AssumeRoleWithWebIdentity` — no long-lived secrets |
| Never for production | Static `BasicAWSCredentials` with hardcoded keys |

**Minimum IAM permissions for runtime invocation:**
```json
{
  "Effect": "Allow",
  "Action": [
    "bedrock:InvokeModel",
    "bedrock:InvokeModelWithResponseStream"
  ],
  "Resource": "arn:aws:bedrock:us-east-1::foundation-model/*"
}
```

---

### 7 — Optimisation

See [optimisation reference](./references/optimization.md) for deep dives.

**High-impact quick wins:**

| Technique | Impact |
|---|---|
| Choose the smallest model that meets quality bar | 3–10× cost reduction |
| Set `MaxTokens` to the minimum needed | Prevents runaway long responses |
| Stream long responses (`ConverseStreamAsync`) | Improves perceived latency |
| Cache repeated system prompts (Prompt Caching — Claude) | Up to 90% input token savings |
| Batch similar requests when real-time response is not required | Lower per-token cost tier |

**Streaming example:**
```csharp
var stream = await client.ConverseStreamAsync(new ConverseStreamRequest
{
    ModelId = "anthropic.claude-3-haiku-20240307-v1:0",
    Messages = messages,
    InferenceConfig = new() { MaxTokens = 1024 }
});

await foreach (var chunk in stream.Stream.AsEnumerableAsync())
{
    if (chunk is ContentBlockDeltaEvent delta)
        Console.Write(delta.Delta.Text);
}
```

---

### 8 — Observability

See [observability reference](./references/observability.md) for full setup.

**Always instrument:**
- **CloudWatch Metrics** — `InputTokens`, `OutputTokens`, `Latency`, `ThrottlingCount` per model
- **CloudWatch Logs** — structured JSON logs with `modelId`, `requestId`, `inputTokens`, `outputTokens`, `latencyMs`, `stopReason`
- **AWS X-Ray** — trace Bedrock calls as segments within your app's distributed trace
- **Guardrail intervention metric** — alert if intervention rate spikes (indicates abuse or misuse)

**Structured log shape:**
```json
{
  "timestamp": "2026-03-29T12:00:00Z",
  "modelId": "anthropic.claude-3-haiku-20240307-v1:0",
  "requestId": "abc-123",
  "inputTokens": 340,
  "outputTokens": 128,
  "latencyMs": 1230,
  "stopReason": "end_turn",
  "guardrailIntervened": false
}
```

---

### 9 — Error Handling and Resilience

```csharp
try
{
    var response = await client.ConverseAsync(request);
    // process response
}
catch (ThrottlingException ex)
{
    // Implement exponential backoff — do not retry immediately
    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
}
catch (ModelNotReadyException)
{
    // Model is being loaded — retry after a short delay
}
catch (ValidationException ex)
{
    // Request malformed — log and surface to caller, do not retry
    logger.LogError("Bedrock validation error: {Message}", ex.Message);
    throw;
}
```

**Resilience checklist:**
- [ ] Exponential backoff with jitter on `ThrottlingException`
- [ ] Circuit breaker for sustained throttling (Polly recommended)
- [ ] Timeout budget: set `HttpClient` timeout > model's expected latency + buffer
- [ ] Log `requestId` from every response for AWS support tracing

---

### 10 — Model Selection

See [model reference](./references/models.md) for detailed comparison.

**Quick selection guide:**

| Use case | Recommended model |
|---|---|
| Fast, low-cost chat / classification | `anthropic.claude-3-haiku-20240307-v1:0` |
| Balanced quality / cost (default) | `anthropic.claude-3-5-sonnet-20240620-v1:0` |
| Complex reasoning, long documents | `anthropic.claude-3-opus-20240229-v1:0` |
| Embeddings | `amazon.titan-embed-text-v2:0` |
| Image generation | `amazon.titan-image-generator-v2:0` |
| Open-weight, cost-sensitive | `meta.llama3-70b-instruct-v1:0` |

---

## Production Readiness Checklist

- [ ] Auth: EC2 / ECS / Lambda role — no hardcoded credentials
- [ ] IAM: least-privilege policy scoped to specific model ARNs
- [ ] Guardrails: published versioned guardrail attached, not DRAFT
- [ ] Observability: CloudWatch metrics, structured logs, X-Ray tracing enabled
- [ ] Resilience: retry with backoff, circuit breaker, timeout configured
- [ ] Cost control: `MaxTokens` set; model sized to task; alerts on token spend
- [ ] Secrets: `guardrailIdentifier`, model IDs in app config / Parameter Store — not in source
- [ ] Region: model availability verified; fallback region considered for HA

## References

- [Authentication patterns](./references/authentication.md)
- [Optimisation techniques](./references/optimization.md)
- [Observability setup](./references/observability.md)
- [Model selection guide](./references/models.md)

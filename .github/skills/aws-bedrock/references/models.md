# AWS Bedrock Model Selection Guide

## Decision Framework

Answer these questions in order:

1. **Do I need multimodal (images/video)?** → Titan or Claude 3+ Sonnet/Opus
2. **Do I need embeddings?** → Titan Embed
3. **Do I need image generation?** → Titan Image Generator
4. **Is cost the primary constraint?** → Claude 3 Haiku or Llama 3
5. **Is reasoning quality the primary constraint?** → Claude 3.5 Sonnet or Opus
6. **Do I need open-weight for compliance/on-prem later?** → Meta Llama or Mistral

---

## Model Catalogue (as of March 2026)

### Anthropic Claude (Primary recommendation for most .NET workloads)

| Model ID | Tier | Context Window | Best For |
|---|---|---|---|
| `anthropic.claude-3-haiku-20240307-v1:0` | Fast / Cheap | 200K tokens | Classification, short answers, high-volume chat |
| `anthropic.claude-3-5-haiku-20241022-v1:0` | Fast / Cheap+ | 200K tokens | Better Haiku with improved reasoning |
| `anthropic.claude-3-5-sonnet-20240620-v1:0` | Balanced | 200K tokens | Default for most production features |
| `anthropic.claude-3-5-sonnet-20241022-v2:0` | Balanced+ | 200K tokens | Sonnet v2 — improved coding and structured output |
| `anthropic.claude-3-opus-20240229-v1:0` | Premium | 200K tokens | Complex reasoning, long-document analysis |

**Region availability:**
- Claude 3 Haiku, Sonnet: `us-east-1`, `us-west-2`, `eu-west-1`, `ap-northeast-1`
- Claude 3 Opus: `us-east-1`, `us-west-2`
- Always verify with `ListFoundationModelsAsync` before hardcoding a region.

---

### Amazon Titan

| Model ID | Type | Best For |
|---|---|---|
| `amazon.titan-text-express-v1` | Text generation | Simple, cost-effective generation |
| `amazon.titan-text-lite-v1` | Text generation | Lowest cost generation |
| `amazon.titan-embed-text-v2:0` | Embeddings | Semantic search, RAG retrieval |
| `amazon.titan-image-generator-v2:0` | Image generation | Image synthesis from text prompts |

---

### Meta Llama 3

| Model ID | Size | Best For |
|---|---|---|
| `meta.llama3-8b-instruct-v1:0` | 8B | Low-cost, open-weight tasks |
| `meta.llama3-70b-instruct-v1:0` | 70B | Open-weight quality/cost balance |

**Use when:** Compliance requires open-weight model provenance, or you anticipate moving to self-hosted later.

---

### Mistral

| Model ID | Best For |
|---|---|
| `mistral.mistral-7b-instruct-v0:2` | European data residency requirements |
| `mistral.mixtral-8x7b-instruct-v0:1` | Mixture-of-experts; fast for diverse tasks |

---

## Quick Selection Matrix

| Use Case | Recommended Model |
|---|---|
| Customer support chatbot (high volume) | `claude-3-haiku` |
| Internal assistant / knowledge base Q&A | `claude-3-5-sonnet` |
| Code generation / debugging | `claude-3-5-sonnet-v2` |
| Legal / contract analysis (long docs) | `claude-3-opus` |
| Semantic search embeddings | `titan-embed-text-v2` |
| Image generation | `titan-image-generator-v2` |
| Bulk nightly summarisation | `claude-3-haiku` (cost) |
| Structured output / tool calling | `claude-3-5-sonnet` (most reliable) |
| Open-weight / compliance | `llama3-70b` |

---

## Model ID Constants (C#)

Define all model IDs as constants — never scatter string literals:

```csharp
public static class BedrockModelIds
{
    // Anthropic Claude
    public const string Claude3Haiku   = "anthropic.claude-3-haiku-20240307-v1:0";
    public const string Claude35Haiku  = "anthropic.claude-3-5-haiku-20241022-v1:0";
    public const string Claude35Sonnet = "anthropic.claude-3-5-sonnet-20240620-v1:0";
    public const string Claude35SonnetV2 = "anthropic.claude-3-5-sonnet-20241022-v2:0";
    public const string Claude3Opus    = "anthropic.claude-3-opus-20240229-v1:0";

    // Amazon Titan
    public const string TitanEmbedV2   = "amazon.titan-embed-text-v2:0";
    public const string TitanImageV2   = "amazon.titan-image-generator-v2:0";

    // Meta Llama 3
    public const string Llama3_8B      = "meta.llama3-8b-instruct-v1:0";
    public const string Llama3_70B     = "meta.llama3-70b-instruct-v1:0";
}
```

---

## Verifying Model Availability at Runtime

```csharp
var bedrockClient = new AmazonBedrockClient(RegionEndpoint.USEast1);
var models = await bedrockClient.ListFoundationModelsAsync(new ListFoundationModelsRequest());

var available = models.ModelSummaries
    .Where(m => m.ModelLifecycle.Status == ModelLifecycleStatus.ACTIVE)
    .Select(m => m.ModelId)
    .ToList();
```

---

## Model Migration Notes

When migrating from one Claude version to another:
1. Test with a sample of real prompts — rephrasing can subtly change output behaviour.
2. Update the model ID constant only — no other code change required when using Converse API.
3. Monitor `StopReason` distribution after migration — unexpected `max_tokens` stops indicate the new model is more verbose.
4. Update guardrail version if it contained model-specific content patterns.

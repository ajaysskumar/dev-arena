# AWS Bedrock Observability

## What to Instrument

| Signal | Tool | What to Capture |
|---|---|---|
| Metrics | CloudWatch Metrics | Token counts, latency, throttle rate, guardrail interventions |
| Logs | CloudWatch Logs | Structured per-request log with model ID, tokens, latency, stop reason |
| Traces | AWS X-Ray | End-to-end trace from API Gateway / ALB through to Bedrock call |
| Alerts | CloudWatch Alarms | Throttling, error rate, cost budget thresholds |

---

## 1 — Structured Logging

Every Bedrock call should produce a structured log entry. Use `Microsoft.Extensions.Logging` with a JSON formatter in production.

```csharp
public async Task<string> InvokeWithLogging(ConverseRequest request)
{
    var stopwatch = Stopwatch.StartNew();
    try
    {
        var response = await _client.ConverseAsync(request);
        stopwatch.Stop();

        _logger.LogInformation(
            "BedrockInvoke {ModelId} {RequestId} {InputTokens} {OutputTokens} {LatencyMs} {StopReason}",
            request.ModelId,
            response.ResponseMetadata.RequestId,
            response.Usage.InputTokens,
            response.Usage.OutputTokens,
            stopwatch.ElapsedMilliseconds,
            response.StopReason);

        return response.Output.Message.Content[0].Text;
    }
    catch (ThrottlingException ex)
    {
        _logger.LogWarning(
            "BedrockThrottled {ModelId} {RetryAfterMs}",
            request.ModelId, ex.RetryAfterSeconds * 1000);
        throw;
    }
    catch (Exception ex)
    {
        stopwatch.Stop();
        _logger.LogError(ex,
            "BedrockError {ModelId} {LatencyMs}",
            request.ModelId, stopwatch.ElapsedMilliseconds);
        throw;
    }
}
```

**Structured log shape (JSON output):**
```json
{
  "timestamp": "2026-03-29T12:00:00.000Z",
  "level": "Information",
  "message": "BedrockInvoke",
  "modelId": "anthropic.claude-3-haiku-20240307-v1:0",
  "requestId": "abc-123-def-456",
  "inputTokens": 340,
  "outputTokens": 128,
  "latencyMs": 1230,
  "stopReason": "end_turn"
}
```

---

## 2 — Custom CloudWatch Metrics

Push custom metrics for dashboards and alerting using `AmazonCloudWatchClient`:

```csharp
public async Task RecordBedrockMetrics(
    string modelId, int inputTokens, int outputTokens, double latencyMs, bool guardrailIntervened)
{
    var request = new PutMetricDataRequest
    {
        Namespace = "MyApp/Bedrock",
        MetricData = new List<MetricDatum>
        {
            new() { MetricName = "InputTokens",  Unit = StandardUnit.Count, Value = inputTokens,   Dimensions = ModelDimension(modelId) },
            new() { MetricName = "OutputTokens", Unit = StandardUnit.Count, Value = outputTokens,  Dimensions = ModelDimension(modelId) },
            new() { MetricName = "Latency",      Unit = StandardUnit.Milliseconds, Value = latencyMs, Dimensions = ModelDimension(modelId) },
            new() { MetricName = "GuardrailIntervened", Unit = StandardUnit.Count,
                    Value = guardrailIntervened ? 1 : 0, Dimensions = ModelDimension(modelId) }
        }
    };
    await _cloudWatchClient.PutMetricDataAsync(request);
}

private static List<Dimension> ModelDimension(string modelId) =>
    new() { new Dimension { Name = "ModelId", Value = modelId } };
```

---

## 3 — AWS X-Ray Tracing

Add X-Ray tracing to capture Bedrock calls as subsegments within your application trace.

**Install:**
```bash
dotnet add package AWSXRayRecorder.Core
dotnet add package AWSXRayRecorder.Handlers.AwsSdk
```

**Configure once at startup** (instruments all AWS SDK calls automatically):
```csharp
// Program.cs
AWSSDKHandler.RegisterXRayForAllServices();
```

**Manual subsegment for richer annotation:**
```csharp
AWSXRayRecorder.Instance.BeginSubsegment("BedrockConverse");
try
{
    AWSXRayRecorder.Instance.AddAnnotation("modelId", request.ModelId);
    var response = await _client.ConverseAsync(request);
    AWSXRayRecorder.Instance.AddMetadata("inputTokens", response.Usage.InputTokens);
    AWSXRayRecorder.Instance.AddMetadata("outputTokens", response.Usage.OutputTokens);
    return response;
}
catch (Exception ex)
{
    AWSXRayRecorder.Instance.AddException(ex);
    throw;
}
finally
{
    AWSXRayRecorder.Instance.EndSubsegment();
}
```

**For Lambda:** Use `AWSXRayRecorder.InitializeInstance()` in the Lambda handler constructor.

---

## 4 — Built-in CloudWatch Metrics (Automatic)

AWS Bedrock emits these metrics automatically to `AWS/Bedrock` namespace — no instrumentation needed:

| Metric | Description |
|---|---|
| `InputTokenCount` | Input tokens per model per minute |
| `OutputTokenCount` | Output tokens per model per minute |
| `InvocationLatency` | End-to-end latency (p50, p90, p99) |
| `ThrottledRequests` | Throttled request count |
| `ModelInvocations` | Total invocation count |

View them in **CloudWatch → Metrics → AWS/Bedrock**, grouped by `ModelId`.

---

## 5 — CloudWatch Alarms

Create these alarms for production:

| Alarm | Metric | Threshold | Action |
|---|---|---|---|
| Throttling alert | `ThrottledRequests` | > 10 / 5 min | SNS → PagerDuty / Slack |
| High cost alert | `InputTokenCount` | > daily budget | SNS → finance / engineering |
| Guardrail spike | `GuardrailIntervened` (custom) | > 5% of requests | SNS → security team |
| Latency regression | `InvocationLatency` p99 | > 5000 ms | SNS → on-call |

---

## 6 — CloudWatch Dashboard Template

Create a dashboard with these widgets:
1. **Token usage over time** — `InputTokenCount` + `OutputTokenCount` stacked, grouped by `ModelId`
2. **Latency distribution** — p50 / p90 / p99 `InvocationLatency`
3. **Throttle rate** — `ThrottledRequests` per minute
4. **Error rate** — custom metric from application logs
5. **Guardrail interventions** — custom metric rate as % of total invocations

---

## 7 — Bedrock Model Invocation Logging

Enable model invocation logging in **Bedrock Console → Settings → Model invocation logging** to get full request/response payloads in S3 or CloudWatch Logs.

> ⚠️ This captures full request + response text — ensure PII guardrails are applied BEFORE logging, or exclude sensitive log streams from long-term retention.

Useful for:
- Debugging unexpected model behaviour in production
- Audit trail for regulated industries
- Prompt quality analysis and fine-tuning datasets

---

## 8 — Observability Checklist

- [ ] Structured JSON logs with `requestId`, `modelId`, `inputTokens`, `outputTokens`, `latencyMs`, `stopReason`
- [ ] X-Ray tracing enabled and SDK registered
- [ ] Custom CloudWatch namespace for token and latency metrics
- [ ] Alarms on throttling, latency p99, and guardrail intervention rate
- [ ] Dashboard created covering tokens, latency, throttling
- [ ] Model invocation logging retention policy set (comply with data retention requirements)
- [ ] Logs scrubbed of PII before retention (or guardrail redaction enforced before logging)

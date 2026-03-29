# AWS Bedrock Authentication Patterns

## Priority Order (Most Secure → Least Secure)

1. **EC2 Instance Profile Role** ← Use for EC2
2. **ECS Task Role** ← Use for ECS/Fargate
3. **Lambda Execution Role** ← Use for Lambda
4. **OIDC/AssumeRoleWithWebIdentity** ← Use for CI/CD
5. **Environment variables** ← Acceptable for containers with secret injection
6. **`~/.aws/credentials`** ← Local dev only
7. **Session credentials (STS)** ← Local dev only
8. **Static `BasicAWSCredentials`** ← Never in production

---

## Pattern 1 — SDK Default Credential Chain (Recommended)

For all environments where a role is attached (EC2, ECS, Lambda, CodeBuild):

```csharp
// No credential code needed — SDK resolves automatically via:
// 1. IMDS (EC2 instance role)
// 2. ECS_CONTAINER_CREDENTIALS_FULL_URI (ECS task role)
// 3. AWS_WEB_IDENTITY_TOKEN_FILE (IRSA / GitHub OIDC)
// 4. ~/.aws/credentials (local dev fallback)
var client = new AmazonBedrockRuntimeClient(RegionEndpoint.USEast1);
```

**This is the recommended pattern for all environments.** Adding explicit credential code when roles are in place is an anti-pattern.

---

## Pattern 2 — EC2 Instance Profile Role

### Creating the role

1. IAM Console → Roles → Create Role
2. Trusted entity: **AWS service → EC2**
3. Attach inline policy:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "bedrock:InvokeModel",
        "bedrock:InvokeModelWithResponseStream",
        "bedrock:ListFoundationModels"
      ],
      "Resource": "*"
    }
  ]
}
```

4. Name the role (e.g., `ec2-bedrock-runtime-role`)
5. Attach to EC2 instance: Actions → Security → Modify IAM Role

### No code change required — SDK automatically uses IMDS:
```csharp
var client = new AmazonBedrockRuntimeClient(RegionEndpoint.USEast1);
```

---

## Pattern 3 — ECS Task Role

1. Create IAM Role with trusted entity: **AWS service → Elastic Container Service Task**
2. Attach the same Bedrock permissions as above
3. In your task definition, set `taskRoleArn` to this role's ARN
4. SDK auto-discovers via `ECS_CONTAINER_CREDENTIALS_FULL_URI` env variable

---

## Pattern 4 — Lambda Execution Role

1. Create or update the Lambda execution role
2. Attach Bedrock permissions inline or via managed policy
3. SDK auto-discovers — no code change needed

---

## Pattern 5 — GitHub Actions OIDC (CI/CD)

No long-lived secrets. GitHub federated identity assumes an AWS role per workflow run.

```yaml
permissions:
  id-token: write
  contents: read

steps:
  - uses: aws-actions/configure-aws-credentials@v4
    with:
      role-to-assume: arn:aws:iam::123456789012:role/github-bedrock-role
      aws-region: us-east-1
```

Trust policy on the AWS role:
```json
{
  "Effect": "Allow",
  "Principal": { "Federated": "arn:aws:iam::123456789:oidc-provider/token.actions.githubusercontent.com" },
  "Action": "sts:AssumeRoleWithWebIdentity",
  "Condition": {
    "StringEquals": {
      "token.actions.githubusercontent.com:aud": "sts.amazonaws.com",
      "token.actions.githubusercontent.com:sub": "repo:<org>/<repo>:ref:refs/heads/main"
    }
  }
}
```

---

## Pattern 6 — Session Credentials (Local Dev Only)

Use for quick local testing with temporary STS tokens. **Never commit tokens to source control.**

```csharp
var credentials = new SessionAWSCredentials(
    Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")!,
    Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")!,
    Environment.GetEnvironmentVariable("AWS_SESSION_TOKEN")!
);
var client = new AmazonBedrockRuntimeClient(credentials, RegionEndpoint.USEast1);
```

---

## Pattern 7 — Static IAM Credentials (Avoid)

Only acceptable in isolated scratch environments. Store in environment variables — never in source code.

```csharp
// ⚠️ Not for production — use roles instead
var credentials = new BasicAWSCredentials(
    Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")!,
    Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")!
);
```

---

## Least-Privilege IAM Policy

Scope permissions to specific model ARNs in production:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "bedrock:InvokeModel",
        "bedrock:InvokeModelWithResponseStream"
      ],
      "Resource": [
        "arn:aws:bedrock:us-east-1::foundation-model/anthropic.claude-3-haiku-20240307-v1:0",
        "arn:aws:bedrock:us-east-1::foundation-model/anthropic.claude-3-5-sonnet-20240620-v1:0"
      ]
    }
  ]
}
```

Add `bedrock:ListFoundationModels` (resource `*`) only if the app needs to enumerate models at runtime.

---

## Troubleshooting

| Error | Cause | Fix |
|---|---|---|
| `UnauthorizedException` | No credentials resolved | Run `aws sts get-caller-identity` to debug |
| `AccessDeniedException on bedrock:InvokeModel` | Missing policy | Attach Bedrock permissions to the role |
| `ResourceNotFoundException` for model | Model access not enabled | Enable in Console → Bedrock → Model access |
| Credentials expire mid-session | Temporary credentials timed out | Use roles (auto-refresh) not session tokens |
| `NoCredentialProviders` | SDK chain exhausted | Check IMDS reachability on EC2; check env vars |

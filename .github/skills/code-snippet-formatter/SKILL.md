---
name: code-snippet-formatter
description: Formats raw code for use inside blog articles in this Blazor project. USE FOR: converting raw code blocks into <CodeSnippet> components, escaping < and > characters, adding snippet numbers and descriptions, choosing the correct language attribute. DO NOT USE FOR: writing the article itself, reviewing article quality.
applyTo: "TestArena/Blog/**/*.razor"
---

# Code Snippet Formatter Skill

You are formatting raw code for inclusion in a **TestArena** blog article Razor file (`Index.razor`).

## What This Skill Does

Converts raw code blocks into correctly formatted `<CodeSnippet>` components, applying all escaping and attribute rules used across existing articles.

---

## BLOCKING REQUIREMENT

Before formatting, read ONE existing article that contains `<CodeSnippet>` usage to confirm current patterns:
- `TestArena/Blog/AI/Bedrock/Index.razor`
- `TestArena/Blog/TDD/WorkingWithDependencies.razor`
- `TestArena/Blog/Frontend/events-demo/Index.razor`

---

## The CodeSnippet Component

```razor
<CodeSnippet Language="csharp" Description="Brief description of what this snippet shows" Number="1">
// Your code here
</CodeSnippet>
```

### Attributes

| Attribute | Required | Values | Notes |
|-----------|----------|--------|-------|
| `Language` | Yes | `csharp`, `javascript`, `typescript`, `json`, `xml`, `html`, `css`, `bash`, `ini`, `razor`, `sql` | Match the language of the code |
| `Description` | Recommended | Any string | Short phrase describing what the snippet does |
| `Number` | Recommended | `1`, `2`, `3`... | Sequential across the whole article |

---

## Escaping Rules (Mandatory)

Inside `<CodeSnippet>` content, **all angle brackets must be escaped**:

| Character | Escaped Form |
|-----------|-------------|
| `<`       | `&lt;`      |
| `>`       | `&gt;`      |
| `&`       | `&amp;` (only when not already an entity) |

### Examples

**Raw code:**
```csharp
if (x < 10 && y > 0)
{
    List<int> numbers = new List<int>();
}
```

**Formatted:**
```razor
<CodeSnippet Language="csharp" Description="Conditional with generics" Number="1">
if (x &lt; 10 &amp;&amp; y &gt; 0)
{
    List&lt;int&gt; numbers = new List&lt;int&gt;();
}
</CodeSnippet>
```

**Raw JSX/HTML:**
```jsx
return <div className="container"><p>Hello</p></div>;
```

**Formatted:**
```razor
<CodeSnippet Language="javascript" Description="JSX component render" Number="2">
return &lt;div className="container"&gt;&lt;p&gt;Hello&lt;/p&gt;&lt;/div&gt;;
</CodeSnippet>
```

---

## Language Selection Guide

| Code Type | Language Attribute |
|-----------|-------------------|
| C# / .NET | `csharp` |
| JavaScript | `javascript` |
| TypeScript | `typescript` |
| React JSX | `javascript` |
| JSON config | `json` |
| XML / XAML | `xml` |
| HTML markup | `html` |
| CSS / SCSS | `css` |
| Shell / CLI | `bash` |
| INI / config files | `ini` |
| Blazor Razor | `razor` |
| SQL queries | `sql` |

---

## Numbering Convention

- Number snippets **sequentially** starting at `1` across the **entire article** (not per section)
- If adding a snippet to an existing article, check the last used number and continue from there

---

## Multi-Snippet Workflow

When given multiple code blocks in one request:
1. Number them sequentially: `Number="1"`, `Number="2"`, etc.
2. Apply escaping to each independently
3. Output all formatted snippets ready to paste into the article
4. Note which section each snippet belongs to (if context is clear)

---

## Indentation

- Keep the original indentation of the code
- The opening `<CodeSnippet ...>` and closing `</CodeSnippet>` tags should align with surrounding `<Section>` content
- Do NOT add extra indentation inside the snippet — code renders verbatim

---

## What Good Output Looks Like

Given this raw input:

> Format this C# code as snippet #3 about "filtering a list with LINQ":
> ```csharp
> var filtered = items.Where(x => x.Age > 18).ToList();
> ```

Produce:

```razor
<CodeSnippet Language="csharp" Description="Filtering a list with LINQ" Number="3">
var filtered = items.Where(x =&gt; x.Age &gt; 18).ToList();
</CodeSnippet>
```

---

## Callout Box Companion

When a snippet needs a tip or warning, pair it with a `<CalloutBox>`:

```razor
<CodeSnippet Language="csharp" Description="Reading credentials from environment" Number="2">
string key = Environment.GetEnvironmentVariable("API_KEY")
    ?? throw new InvalidOperationException("API_KEY is not set.");
</CodeSnippet>
<CalloutBox Type="tip" Title="Use Environment Variables">
    <p>Never hardcode secrets. Always read them from environment variables or a secrets manager.</p>
</CalloutBox>
```

Types: `info`, `warning`, `tip`

---

## Invoke Me When

- You have raw code to add to a blog article
- You need to escape angle brackets in existing article code
- You want to check if code snippets in an article are numbered correctly
- You need to choose the right `Language` attribute for an unfamiliar code type

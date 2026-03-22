---
name: blog-quality-review
description: Reviewing, auditing, or checking the quality of an existing blog article before publishing. Runs a structured checklist covering file placement, SiteMap registration, Razor structure, content quality, heading levels, component usage, and series continuity.
---

# Blog Quality Review Skill

You are a technical blog quality reviewer for the **TestArena** Blazor project. Your job is to audit a blog article written as a Razor component and report quality issues clearly before publishing.

## BLOCKING REQUIREMENT
Before producing any review output, read the article file in full using `read_file`. Also cross-check `TestArena/Blog/Common/NavigationUtils/SiteMap.cs` and `wwwroot/sitemap.xml`. Do not skip these reads.

---

## How to Conduct the Review

1. **Identify the article** — if not provided, ask for the file path or topic name, then find `Index.razor` under `TestArena/Blog/`.
2. **Read the article** using `read_file`.
3. **Check SiteMap.cs** — read `TestArena/Blog/Common/NavigationUtils/SiteMap.cs` and search for the article's URL.
4. **Check sitemap.xml** — use `grep_search` for the article URL in `wwwroot/sitemap.xml`.
5. **Run every item** in the checklist below.
6. **Compile and output the report** in the format specified at the bottom.

---

## Review Checklist

### 1. File & Placement
- [ ] Article lives inside `TestArena/Blog/<Category>/<SubFolder>/Index.razor`
- [ ] The category folder is appropriate for the topic

### 2. SiteMap Registration
- [ ] `SiteMap.cs` contains an entry whose `RelativePath` exactly matches the `@page` directive
- [ ] The entry has a meaningful title, a published date, a banner image path, and relevant tags
- [ ] `wwwroot/sitemap.xml` contains a `<url>` block with the matching URL

### 3. Razor Structure
- [ ] `@page` directive is present and the path is kebab-case
- [ ] Required `@using` statements are present:
  - `@using TestArena.Blog.Common`
  - `@using TestArena.Blog`
  - `@using System.Linq`
  - `@using TestArena.Blog.Common.NavigationUtils`
- [ ] `@code` block exists and resolves `currentPage` via `SiteMap.Pages.FirstOrDefault(x => x.RelativePath == "...")`
- [ ] The `RelativePath` string in `@code` exactly matches the `@page` directive
- [ ] `<BlogContainer>` wraps all content
- [ ] `<Header>` is present and binds `Title`, `Image`, `PublishedOn`, `Authors`, and `isOlderThumbnailFormat` from `currentPage`

### 4. Content Quality
- [ ] Opens with a relatable problem or hook
- [ ] Follows **What / When / How** format
- [ ] Word count is approximately **1200–1800 words** (estimate from reading)
- [ ] Ends with a summary section
- [ ] Writing is beginner-friendly — no unexplained jargon
- [ ] Real-world analogies are present

### 5. Headings
- [ ] All top-level sections use `<Section Heading="..." Level="4">`
- [ ] All sub-sections use `<Section Heading="..." Level="5">`
- [ ] **No raw `<h1>` `<h2>` `<h3>` tags exist** — these are a defect

### 6. Component Usage
- [ ] Multi-line code samples use `<CodeSnippet Language="...">` (not bare `<pre>` or `<code>`)
- [ ] `<` and `>` inside code content are escaped as `&lt;` and `&gt;`
- [ ] `<CalloutBox>` is used for tips, warnings, or important notes where appropriate
- [ ] `<BlogImage>` or image placeholders are present where illustrations would help
- [ ] `<EndNotes>` is present if external sources are cited

### 7. Series Continuity (if applicable)
- [ ] If part of a series, the previous article is referenced and linked
- [ ] Tags in SiteMap are consistent with sibling articles in the same series

---

## Report Format

Output the review using this exact structure:

```
## Blog Quality Review: <Article Title>
File: <relative path to Index.razor>

### ✅ Passing Checks
- <list each check that passes>

### ❌ Issues Found
| # | Check | Severity | Detail |
|---|-------|----------|--------|
| 1 | <check name> | High/Medium/Low | <specific description of the problem> |

### ⚠️ Suggestions
- <optional improvements that are not blocking>

### Summary
<2–3 sentence overall verdict and the single most important next step>
```

**Severity levels:**
- **High** — blocks publishing: missing SiteMap entry, broken `@page`, missing `<Header>`, wrong `RelativePath`
- **Medium** — degrades UX or SEO: wrong heading levels, bare `<pre>` for code, missing sitemap.xml entry
- **Low** — style or readability: analogies missing, word count slightly off, missing `<CalloutBox>` where one would help

If there are no issues, say so clearly in the Summary and confirm the article is ready to publish.

---
name: blog-article
description: Writing, creating, or drafting a new technical blog article in this Blazor project. Covers end-to-end workflow: choosing the URL, updating SiteMap.cs, updating sitemap.xml, creating Index.razor, and writing 1200-1800 word content.
---

# Blog Article Skill

You are writing a technical blog article for the **TestArena** Blazor project hosted at `devcodex.in`.

## BLOCKING REQUIREMENT
Before doing ANYTHING else, read `copilot/blog-article.md` in full using `read_file`. That file is the **single source of truth** for all formatting, structure, and placement rules. Follow every instruction in it exactly.

---

## Workflow

Execute these steps **in order**. Do not skip steps or reorder them.

### Step 1 — Read the full guidelines
Read `copilot/blog-article.md`. Internalize all rules before proceeding.

### Step 2 — Read a reference article
Read one of the following reference articles to understand the exact Razor component patterns used:
- `TestArena/Blog/Frontend/events-demo/Index.razor`
- `TestArena/Blog/AI/Bedrock/Index.razor`
- `TestArena/Blog/IntegrationTesting/` (any `Index.razor` in this folder)

### Step 3 — Determine article URL and folder
- Decide the URL in kebab-case: `/blog/<category>/<article-slug>`
- Choose the correct category folder under `TestArena/Blog/`:
  - Topic-specific: `React/`, `TDD/`, `AI/Bedrock/`, `Security/`, `SqlServer/` etc.
  - General frontend topics → `Frontend/`
  - Not fitting a category → create a new folder with a clear name
- The article file will be: `TestArena/Blog/<Category>/<SubFolder>/Index.razor`

### Step 4 — Update SiteMap.cs
Read `TestArena/Blog/Common/NavigationUtils/SiteMap.cs` to understand the existing `PageInfo` constructor pattern (title, relative path, date, image path, tags array). Add a new entry at the **end** of the `Pages` list following the exact same format.

### Step 5 — Update sitemap.xml
Read `wwwroot/sitemap.xml` and add a `<url>` block for the new article at the end, using:
- `<loc>https://devcodex.in<article-url></loc>`
- `<lastmod>` set to today's date in `YYYY-MM-DD` format
- `<changefreq>monthly</changefreq>`

### Step 6 — Create Index.razor
Create `TestArena/Blog/<Category>/<SubFolder>/Index.razor` with:

**Required structure (in this order):**
1. `@page "/blog/<category>/<slug>"`
2. `@using` statements:
   ```
   @using TestArena.Blog.Common
   @using TestArena.Blog
   @using System.Linq
   @using TestArena.Blog.Common.NavigationUtils
   ```
3. `@code` block:
   ```csharp
   @code {
       PageInfo currentPage = SiteMap.Pages.FirstOrDefault(x => x.RelativePath == "/blog/<category>/<slug>")!;
   }
   ```
4. `<BlogContainer>` wrapping all content
5. `<Header>` bound to `currentPage` properties
6. Article body

### Step 7 — Write the article body

Follow the **What / When / How** format:
1. **Hook** — Open with a relatable problem or scenario that the reader has experienced
2. **What** — Explain what the topic is; include origin if relevant
3. **When** — Explain the scenarios or use cases where this topic applies
4. **How** — Step-by-step implementation with prerequisites → setup → code examples → explanation

**Content rules (non-negotiable):**
- Target **1200–1800 words**
- Use `<Section Heading="..." Level="4">` for top-level sections
- Use `<Section Heading="..." Level="5">` for sub-sections
- **Never use raw `<h1>` `<h2>` `<h3>` tags** — these are a bug
- Use `<CodeSnippet Language="csharp">` (or appropriate language) for all multi-line code samples
- Escape `<` as `&lt;` and `>` as `&gt;` inside code content
- Use `<CalloutBox Type="info|warning|tip" Title="...">` for notes and callouts
- Use `<BlogImage>` or descriptive image placeholders where visuals help
- Use `<EndNotes>` if citing external sources
- If this is part of a series, reference and link the previous article
- End with a **Summary** section
- Include simple, relatable real-world analogies to explain abstract concepts
- Writing must be beginner-friendly — avoid unexplained jargon

---

## Quality Gate

After writing the article, perform a self-check:
- [ ] `SiteMap.cs` has a new entry with matching `RelativePath`
- [ ] `sitemap.xml` has the new URL
- [ ] `@page` directive URL matches both of the above
- [ ] `@code` block `RelativePath` string matches `@page`
- [ ] No `<h1>`–`<h3>` tags used anywhere
- [ ] All code in `<CodeSnippet>` components with escaped `<` `>`
- [ ] Word count is between 1200–1800 words
- [ ] Article ends with a Summary section

---
name: sitemap-update
description: Registering a new page or article in SiteMap.cs and sitemap.xml. Covers adding a PageInfo entry to SiteMap.cs and a url block to sitemap.xml.
---

# Sitemap Update Skill

You are registering a new blog article in the **TestArena** project's navigation and SEO infrastructure.

## BLOCKING REQUIREMENT
Before making any edits, read both target files in full. Do not guess at the format — observe the existing patterns and match them exactly.

---

## What This Skill Does

Registers a new blog article in exactly two places:
1. `TestArena/Blog/Common/NavigationUtils/SiteMap.cs` — the in-app navigation index
2. `wwwroot/sitemap.xml` — the SEO sitemap served to search engines

---

## Workflow

### Step 1 — Gather information
Confirm you have (or ask for) all of the following:
- **Article title** — human-readable, used in navigation and SEO
- **Relative URL** — kebab-case, e.g. `/blog/tdd/intro-to-tdd`
- **Published date** — `YYYY-MM-DD` format
- **Banner image path** — relative to `wwwroot/`, e.g. `images/blog/tdd/intro/banner.png`
- **Tags** — 2–4 short strings relevant to the topic, e.g. `["TDD", ".NET"]`

### Step 2 — Read SiteMap.cs
Read `TestArena/Blog/Common/NavigationUtils/SiteMap.cs` in full. Observe:
- How `PageInfo` constructor is called (positional arguments: title, path, date, image, tags)
- The indentation and formatting of existing entries
- The last entry in the `Pages` list (new entry goes after it)

### Step 3 — Add entry to SiteMap.cs
Add a new `PageInfo` entry at the **end** of the `Pages` list, directly before the closing `]`. Match the formatting of existing entries exactly:

```csharp
new("Article Title Here",
    "/blog/category/slug",
    new DateTime(YYYY, MM, DD),
    "images/blog/category/slug/banner.png",
    ["Tag1", "Tag2"]),
```

### Step 4 — Read sitemap.xml
Read `wwwroot/sitemap.xml` in full. Observe:
- The `<url>` block format
- The root domain (`https://devcodex.in`)
- The date format used in `<lastmod>` (`YYYY-MM-DD`)
- The last `<url>` entry (new entry goes after it, before `</urlset>`)

### Step 5 — Add entry to sitemap.xml
Add a new `<url>` block at the end of the file, directly before `</urlset>`:

```xml
<url>
  <loc>https://devcodex.in/blog/category/slug</loc>
  <lastmod>YYYY-MM-DD</lastmod>
  <changefreq>monthly</changefreq>
</url>
```

### Step 6 — Verify
After editing, confirm:
- [ ] The `RelativePath` in `SiteMap.cs` exactly matches the URL in `sitemap.xml` (after the domain)
- [ ] Both files compile/parse cleanly (no missing commas, unclosed tags, etc.)
- [ ] The new entry is the last one in each file before the closing delimiter

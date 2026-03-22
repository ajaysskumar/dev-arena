# GitHub Copilot Custom Instructions

## Purpose
This file provides custom instructions for GitHub Copilot to enhance the developer experience in this repository.

## Instruction
**Prompt:**  
Whenever asked for writing an article on any topic, follow the instructions present in markdown file copilot/blog-article.md

## Skills

Skills provide specialised workflows for common tasks in this repository.
**BLOCKING REQUIREMENT:** When a skill applies to the user's request, you MUST load and read the SKILL.md file IMMEDIATELY as your first action, BEFORE generating any response or taking any action. Use `read_file` to load the skill. NEVER skip loading a skill when it applies.

<skills>
<skill>
  <name>blog-article</name>
  <description>**WORKFLOW SKILL** — Writing, creating, or drafting a new technical blog article in this Blazor project. USE FOR: "write an article on X", "create a blog post about Y", "draft a new article". Covers end-to-end workflow: choosing the URL, updating SiteMap.cs, updating sitemap.xml, creating Index.razor, and writing 1200-1800 word content. DO NOT USE FOR: editing an existing article's content without creating a new one, reviewing article quality.</description>
  <file>.github/skills/blog-article/SKILL.md</file>
</skill>
<skill>
  <name>blog-quality-review</name>
  <description>**WORKFLOW SKILL** — Reviewing, auditing, or checking the quality of an existing blog article before publishing. USE FOR: "review this article", "check article quality", "is this article ready to publish", "audit the blog post", "verify the article". Runs a structured checklist covering file placement, SiteMap registration, Razor structure, content quality, heading levels, component usage, and series continuity. DO NOT USE FOR: writing a new article, making edits to an article.</description>
  <file>.github/skills/blog-quality-review/SKILL.md</file>
</skill>
<skill>
  <name>sitemap-update</name>
  <description>**WORKFLOW SKILL** — Registering a new page or article in SiteMap.cs and sitemap.xml. USE FOR: "add this page to the sitemap", "register this article", "update the sitemap", "add a SiteMap entry". Covers adding a PageInfo entry to SiteMap.cs and a url block to sitemap.xml. DO NOT USE FOR: writing the article content itself, reviewing article quality.</description>
  <file>.github/skills/sitemap-update/SKILL.md</file>
</skill>
</skills>
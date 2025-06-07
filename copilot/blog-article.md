
---

**Instructions for Writing a Blog Article**

1. **Refer ReferenceFile**
2. Write a beginner friendly and detailed article on **Topic** inside the given **TargetFile**. It is very important that article must be **1200 - 1800 words long**.
3. Article should be detailed enough to cover all aspects of the topic. Like why can it be useful and what problems it can solve. Must be **1200 - 1500 words long**.
4. The article should be in format of **What, When and How** format.
5. The article should follow a proper flow and should not have break continuity.
6. Use **h3 and smaller** for headings.
7. If topic reference is not already there then update `SiteMap.cs` to include the new article details. Refer previous entries in the `Pages` collection. Use the `Pages` property to extract the `currentPage` in the **TargetFile** and use the `currentPage` object to populate the `Header` blazor component reference inside **TargetFile**. Take inspiration from **ReferenceFile**.
8. Use **C#** as language to show relevant code samples.
9. If **Topic** is a tech topic then focus should be more on the tech side of things e.g. use code samples to explain scenarios instead of focusing on theory too much.
10. Use a small relatable/real world problem/analogy to drive the narration of the article.
11. Analogy should be simple enough to reach wider audience.
12. For code sample use `<Root>/TestArena/Blog/Common/CodeSnippet.razor` component. Refer the usage pattern from the **ReferenceFile**.
13. Add entry for the article in `#file:sitemap.xml`.
14. At the end, leave a summary.

---
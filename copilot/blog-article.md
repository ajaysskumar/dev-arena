# Instructions for Writing a Blog Article

### General Guidelines

1. **Initial Setup**
   - Refer **ReferenceFile** for guidance and formatting. If rerefernece file is not provided then follow any of the following below folders
     - ROOT_FOLDER/TestArena/Blog/Frontend/events-demo/Index.razor
     - ROOT_FOLDER/TestArena/Blog/PactNet/*
     - ROOT_FOLDER/TestArena/Blog/IntegrationTesting/*
   - Decide the URL of the article
   - Update `SiteMap.cs` to include article entry in the maintained inde array
     - Reference previous `Pages` collection entries
   - Update the `sitemap.xml` to include the new blog's URL
2. **Article Structure**
   1. Write beginner-friendly content on **Topic** in **TargetFile**
      1. If TargetFile is not explicitely mentioned then follow any of the below file for reference:
         1. ROOT_FOLDER/TestArena/Blog/Frontend/events-demo/Index.razor
   2. Article length should be **1200-1800 words**
   3. Use **What, When and How** format
      1. Start with something interest problem which states the need of the solution/topic of the blog
      2. Write a few words about origin(If applicable)
      3. State and explain when or in which scenarios, the solution/blog topic in context is needed
      4. Explain how can it be implemented. Also explain the setup/pre-requisites required.
   4. Maintain flow and continuity
   5. Use **h4 and smaller** headings and **h5 or smaller** for sub headings This is must to follow.
   6. Include a summary the the end
   7. Also include any code references or gihub links that may be needed
3. **Article placement**
   1. Create a new folder inside ROOT/Blog/<Article_Name>. In some cases the topic Article_Name folder will already be present. Use that based on the the nature of the topic.
      1. Example is its specifi to React then Use `React` Folder
      2. If its something general to fronend, then use `Frontend` folder
   2. Inside new folder, create file `Index.razor`
   3. This file will contain the article content
   4. If not reference article is provided, use the `Template.razor` for reference by default
4. **Content Requirements**
   1. Provide comprehensive topic coverage
   2. Each article will have a code section in the beginning after the library references which would look like below
      1. @code{
         PageInfo currentPage = SiteMap.Pages.FirstOrDefault(x => x.RelativePath == "/blog/frontend/events-demo")!;
         }
      2. This needed to make sure the correct header values are set using the `currentPage`
   3. Include usefulness and problem-solving aspects
   4. Target word count: **1200-1800 words**
   5. Use notes where applicable to include edge cases or special notes
   6. Inlcude images for illustration where ever needed. If images are not available, just put image placeholders.
   7. If this article is part of the series then use previous article in the series
   8. For any code snippets used, replace `<` with &lt; and `>` with &gt;
5. **Code and Examples**
   1. Use **C#** by default for code samples. This is not rule, but a suggestion, for js articles js language should be used.
   2. For tech topics, prioritize code examples over theory
   3. Implement `<Root>/TestArena/Blog/Common/CodeSnippet.razor` component
   4. Follow **ReferenceFile** usage patterns
6. **Narrative Approach**
   1. Include simple, relatable real-world analogies
   2. Ensure analogies are accessible to wide audience

# Instructions for Writing a Blog Article

### General Guidelines

1. **Initial Setup**
   - **Refer ReferenceFile** for guidance and formatting
2. **Article Structure**
   1. Write beginner-friendly content on **Topic** in **TargetFile**
   2. Article length: **1200-1800 words**
   3. Use **What, When and How** format
   4. Maintain flow and continuity
   5. Use **h3 and smaller** headings
3. **Article placement**
   1. Create a new folder inside ROOT/Blog/<Article_Name>
   2. Inside new folder, create file `Index.razor`
   3. This file will contain the article content
   4. If not reference article is provided, use the `Template.razor` for reference by default
4. **Content Requirements**
   1. Provide comprehensive topic coverage
   2. Include usefulness and problem-solving aspects
   3. Target word count: **1200-1500 words**
5. **Technical Implementation**
   1. Update `SiteMap.cs` if topic is new
   2. Reference previous `Pages` collection entries
   3. Use `currentPage` object for `Header` component
   4. Follow **ReferenceFile** patterns
6. **Code and Examples**
   1. Use **C#** for code samples
   2. For tech topics, prioritize code examples over theory
   3. Implement `<Root>/TestArena/Blog/Common/CodeSnippet.razor` component
   4. Follow **ReferenceFile** usage patterns
7. **Narrative Approach**
   1. Include simple, relatable real-world analogies
   2. Ensure analogies are accessible to wide audience
8. **Documentation**
   1. Add article entry in `#file:sitemap.xml`
   2. Include summary at article end

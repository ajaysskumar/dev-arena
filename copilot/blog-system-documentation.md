# TestArena Blog System Documentation

This document provides an overview of the blog system in TestArena, focusing on common components and usage patterns to help you add new blog articles.

## Project Structure

The TestArena blog system is a Blazor-based platform with a well-organized structure:

```
TestArena/
├── Blog/
│   ├── AI/
│   ├── Common/            # Shared components
│   ├── HtmlBlogs/
│   ├── IntegrationTesting/
│   ├── PactNet/
│   ├── React/
│   ├── Security/
│   ├── SoftwarePractices/
│   │   └── SOLID/        # SOLID principles articles
│   └── Template.razor    # Base template for new articles
├── copilot/
│   └── blog-article.md   # Instructions for writing blogs
```

## Common Components

The system uses several reusable components found in `TestArena/Blog/Common`:

### 1. BlogContainer

Wraps the entire blog content and adds consistent styling:

```cshtml
<BlogContainer>
    <!-- Your blog content goes here -->
</BlogContainer>
```

### 2. Header

Displays the blog title, publication date, and author information:

```cshtml
<Header 
    Title="@currentPage.Header"
    Image="@currentPage.ArticleImage" 
    PublishedOn="@currentPage.PublishedOn" 
    Authors="Ajay Kumar">
</Header>
```

### 3. Section

Creates structured content sections with configurable heading levels:

```cshtml
<Section Heading="What is the Single Responsibility Principle (SRP)?" Level="3">
    <p>Content goes here...</p>
</Section>
```

### 4. CodeSnippet

Displays formatted code examples with descriptions and numbering:

```cshtml
<CodeSnippet Number="1" Language="csharp" Description="A class doing too much (violating SRP)">
public class User
{
    public string Name { get; set; }
    // Code example
}
</CodeSnippet>
```

### 5. BlogImage

Renders images with captions and numbering:

```cshtml
<BlogImage 
    ImagePath="/images/blog/integration-testing/intro/Demo API swagger definition.webp" 
    Description="Demo API swagger definition" 
    Number="3" />
```

### 6. List

Creates formatted lists with custom headings:

```cshtml
<List 
    Heading="Services that we will be setting up for this exercise" 
    HeadingLevel="6" 
    ChildContents="listItems"/>
```

### 7. BlogReferenceCard

Links to related articles:

```cshtml
<BlogReferenceCard 
    Title="Integration testing for dotnet core APIs: Introduction"
    Description="This is going to be a multi-part series on integration tests."
    Url="/blog/integration-testing-in-dotnet-intro" 
    ImageUrl="/images/blog/integration-testing/intro/banner.png"
    Source="devcodex.in" />
```

### 8. SocialShare

Enables readers to share blog posts on social media:

```cshtml
<SocialShare 
    Title="@currentPage.Header" 
    Description="A concise description of your blog post" 
    HashTags="dotnet,blazor,webdev" />
```

### 9. EndNotes

Adds conclusion with repository link:

```cshtml
<EndNotes RepositoryLink="https://github.com/ajaysskumar/pact-net-example" />
```

## Site Navigation Management

All blog metadata is managed in `TestArena/Blog/Common/NavigationUtils/SiteMap.cs`:

```csharp
new("SOLID Principles: Understanding the Single Responsibility Principle",
    "/blog/software-practices/solid/single-responsibility-principle",
    new DateTime(2025, 5, 10),
    "images/blog/software-practices/solid/srp/banner.png",
    ["Software Practices", "SOLID", "SRP"]),
```

Each entry includes:
- Title (Header)
- URL (RelativePath)
- Publication date
- Banner image path
- Tags for categorization

## Adding a New Blog Article

### 1. Create File Structure

Create a new folder in the appropriate category directory:
```
TestArena/Blog/YourCategory/YourArticleName/Index.razor
```

### 2. Basic Article Template

```cshtml
@page "/blog/your-article-path"
@using TestArena.Blog.Common
@using TestArena.Blog.Common.NavigationUtils

@code{
    PageInfo currentPage = SiteMap.Pages.FirstOrDefault(x => x.RelativePath == "/blog/your-article-path")!;
}

<BlogContainer>
    <Header 
        Title="@currentPage.Header"
        Image="@currentPage.ArticleImage" 
        PublishedOn="@currentPage.PublishedOn" 
        Authors="Ajay Kumar">
    </Header>

    <Section Heading="🔍 What is [Your Topic]?" Level="3">
        <!-- Content following What-When-How format -->
    </Section>

    <!-- Add more sections as needed -->
</BlogContainer>
```

### 3. Update SiteMap.cs

Add your article metadata to the Pages collection in `TestArena/Blog/Common/NavigationUtils/SiteMap.cs`:

```csharp
new("Your Article Title",
    "/blog/your-article-path",
    new DateTime(2025, 7, 1), // Publication date
    "images/blog/your-category/your-article/banner.png",
    ["Tag1", "Tag2"]),
```

### 4. Content Structure Pattern

Follow the "What, When, How" format seen in existing articles:
1. Start with explanatory section (What is the topic)
2. When to use this approach
3. How to implement it (with practical code examples)
4. Common pitfalls to avoid
5. Summary or conclusion

### 5. Writing Style Guidelines

- Include real-world analogies (like the pizza shop example in OCP article)
- Use emoji icons for section headings to improve readability
- Structure content with clear hierarchy
- Include practical code examples
- Target 1200-1800 words
- Highlight key points in bold
- Use lists for multiple related points

## Examples from Existing Blogs

The SOLID principles articles follow an excellent pattern:
- Clear explanation of the principle
- Real-world analogies
- Code examples showing violations and proper implementations
- Practical application scenarios
- Common pitfalls
- Connection to other principles
- Wrap-up summary

Integration testing articles follow a sequence approach:
- Explanation of the testing concept
- Step-by-step implementation
- Code examples
- Visual aids with screenshots
- Links to previous articles in the series

## Conclusion

Follow these patterns consistently when adding new articles to maintain the high quality and uniform style across TestArena's documentation. Remember to update the `TestArena/Blog/Common/NavigationUtils/SiteMap.cs` file and use the appropriate common components to ensure your article integrates seamlessly with the rest of the blog system.

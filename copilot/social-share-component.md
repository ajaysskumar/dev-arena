# Using the SocialShare Component

The `SocialShare` component allows readers to share your blog posts on various social media platforms. It's designed to be simple to use and provides sharing options for Twitter, Facebook, LinkedIn, and direct link copying.

## How to Use the SocialShare Component

Add the component to your blog post by placing it before the `EndNotes` component:

```cshtml
@using TestArena.Blog.Common

<SocialShare 
    Title="@currentPage.Header" 
    Description="Your blog post description here" 
    HashTags="dotnet,blazor,tag1,tag2" />
```

### Parameters

- **Title**: The title to use when sharing (defaults to the blog title if not specified)
- **Description**: A short description of the blog post (for platforms that support it)
- **HashTags**: Comma-separated tags without spaces or hashtag symbols (e.g., "dotnet,blazor,webdev")

### Example Usage

```cshtml
@page "/blog/your-blog-path"
@using TestArena.Blog.Common
@using TestArena.Blog.Common.NavigationUtils

@code {
    PageInfo currentPage = SiteMap.Pages.FirstOrDefault(x => x.RelativePath == "/blog/your-blog-path")!;
}

<BlogContainer>
    <Header
        Title="@currentPage.Header"
        Image="@currentPage.ArticleImage"
        PublishedOn="@currentPage.PublishedOn"
        Authors="Ajay Kumar" />
    
    <!-- Blog content here -->
    
    <SocialShare 
        Title="@currentPage.Header" 
        Description="A concise description of your blog post" 
        HashTags="dotnet,blazor,webdev" />
        
    <EndNotes RepositoryLink="https://github.com/yourusername/your-repo" />
</BlogContainer>
```

## Styling

The component comes with built-in styling for:
- Share button layout
- Button hover effects
- Color schemes for different social platforms
- Animation for the "Copied!" notification

The styling is contained in `SocialShare.razor.css` and is automatically applied when the component is used.

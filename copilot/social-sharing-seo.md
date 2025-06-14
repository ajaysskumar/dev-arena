# Social Sharing and SEO in TestArena Blog System

This document explains how to implement proper social media sharing and SEO for your blog posts using OpenGraphTags and SocialShare components.

## Open Graph Meta Tags for Proper Link Sharing

When sharing blog links on social media platforms like LinkedIn, Twitter, or Facebook, it's important to ensure that the correct title, description, and image are displayed. This is accomplished using Open Graph meta tags.

### Using the OpenGraphTags Component

The `OpenGraphTags` component dynamically updates the meta tags in your page's `<head>` section:

```cshtml
<OpenGraphTags 
    Title="Your Blog Post Title"
    Description="A concise description of your blog post"
    ImageUrl="/images/blog/your-category/your-image.png" />
```

Place this component at the beginning of your blog post, before the `<BlogContainer>` tag.

### Parameters

- **Title**: The title to be displayed when the link is shared
- **Description**: A brief description of your blog post (150-200 characters recommended)
- **ImageUrl**: Path to the image that should appear with the shared link

### Using with Current Page Info

You can leverage the existing `currentPage` object to automatically populate the title and image:

```cshtml
@code {
    PageInfo currentPage = SiteMap.Pages.FirstOrDefault(x => x.RelativePath == "/blog/your-path")!;
    private string description = "Your custom description here";
}

<OpenGraphTags 
    Title="@currentPage.Header"
    Description="@description"
    ImageUrl="@currentPage.ArticleImage" />
```

## Social Share Buttons

The `SocialShare` component provides buttons for readers to share your articles on various platforms:

```cshtml
<SocialShare 
    Title="@currentPage.Header" 
    Description="Your description here" 
    HashTags="dotnet,blazor,tag1,tag2" />
```

Place this component before the `EndNotes` component at the end of your blog post.

### Parameters

- **Title**: The title to be used in social media shares
- **Description**: A description for platforms that support it
- **HashTags**: Comma-separated hashtags without spaces (e.g., "dotnet,blazor")

## Using BlogTemplate for New Articles

For new articles, consider using the `BlogTemplate` component which combines both OpenGraphTags and the standard blog layout:

```cshtml
@page "/blog/your-article-path"
@using TestArena.Blog.Common
@using TestArena.Blog.Common.NavigationUtils

@code {
    PageInfo currentPage = SiteMap.Pages.FirstOrDefault(x => x.RelativePath == "/blog/your-article-path")!;
    private string description = "A concise description for social media shares";
}

<BlogTemplate CurrentPage="@currentPage" Description="@description">
    <Section Heading="Introduction" Level="4">
        Your content here...
    </Section>
    
    <!-- More sections -->
    
    <SocialShare 
        Title="@currentPage.Header" 
        HashTags="dotnet,blazor,webdev" />
        
    <EndNotes RepositoryLink="https://github.com/yourusername/your-repo" />
</BlogTemplate>
```

This approach ensures that both SEO meta tags and social sharing functionality are properly implemented in your articles.

## Best Practices

1. Always provide a unique, descriptive meta description for each blog post
2. Use high-quality images that will look good when shared (recommended 1200×630 pixels)
3. Include relevant hashtags to increase discoverability on social platforms
4. Place the social share buttons at the end of your content but before the end notes

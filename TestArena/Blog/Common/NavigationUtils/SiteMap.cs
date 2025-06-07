using System.Collections.Generic;

namespace TestArena.Blog.Common.NavigationUtils
{
    public static class SiteMap
    {
        public static List<PageInfo> Pages { get; } =
        [
            new("Intro to PACT for .NET Core: API contract testing",
            "/blog/contract-testing-pact-net-intro",
            new DateTime(2024, 12, 14),
            "images/blog/pact/intro/banner.png",
            ["PACT", "API"]),

        new("Intro to PACT for .NET Core: Events based systems",
            "/blog/contract-testing-in-pact-with-events",
            new DateTime(2024, 12, 28),
            "images/blog/pact/events-demo/banner.webp",
            ["PACT", "Events"]),

        new("Intro to PACT for .NET Core: Integration with PactFlow",
            "/blog/integration-with-pactflow",
            new DateTime(2025, 1, 11),
            "images/blog/pact/pact-broker/banner.png",
            ["PACT", "PactFlow"]),

        new("Integration testing for dotnet core APIs: Introduction",
            "/blog/integration-testing-in-dotnet-intro",
            new DateTime(2025, 1, 25),
            "images/blog/integration-testing/intro/banner.png",
            ["Integration Testing", ".NET"]),

        new("Integration testing for dotnet core APIs: Handling database",
            "/blog/integration-testing-in-dotnet-with-database",
            new DateTime(2025, 2, 8),
            "images/blog/integration-testing/handling-database/banner.png",
            ["Integration Testing", "Database"]),

        new("Integration testing for dotnet core APIs: Handling APIs behind authentication",
            "/blog/integration-testing-in-dotnet-with-auth",
            new DateTime(2025, 2, 22),
            "images/blog/integration-testing/handling-auth/banner.png",
            ["Integration Testing", "Authentication"]),

        new("Integration testing for dotnet core APIs: Handling 3rd party service calls using wiremock",
            "/blog/integration-testing-in-dotnet-with-external-services",
            new DateTime(2025, 3, 8),
            "images/blog/integration-testing/handling-external-services/banner.png",
            ["Integration Testing", "WireMock"]),

        new("Securing .NET Applications: Preventing Threats Through Package Audits",
            "/blog/package-vulnerability-detection-in-dotnet",
            new DateTime(2025, 3, 22),
            "images/blog/vulnerability/auditing/banner.png",
            ["Security", "Vulnerability", ".NET"]),
        new("Custom Elements in HTML",
            "/blog/custom-elements-in-html",
            new DateTime(2025, 4, 5),
            "images/blog/html-blogs/custom-element/banner.png",
            ["HTML", "Web Components", "Custom Elements"]),
        new("Module Federation in React",
            "/blog/module-federation-demo-in-react",
            new DateTime(2025, 4, 19),
            "images/blog/react/module-federation/banner.png",
            ["micro-fronted", "react", "module-federation"]),
        new("Integration testing for dotnet core APIs: Working with AWS flows",
            "/blog/integration-testing-in-dotnet-with-aws",
            new DateTime(2025, 5, 03),
            "images/blog/integration-testing/handling-aws/banner.png",
            ["Integration Testing", "Localstack", "AWS"]),
        new("SOLID Principles: Understanding the Single Responsibility Principle",
            "/blog/software-practices/solid/single-responsibility-principle",
            new DateTime(2025, 5, 10),
            "images/blog/software-practices/solid/srp/banner.png",
            ["Software Practices", "SOLID", "SRP"]),

        new("SOLID Principles: Understanding the Open/Closed Principle",
            "/blog/software-practices/solid/open-close-principle",
            new DateTime(2025, 5, 24),
            "images/blog/software-practices/solid/ocp/banner.png",
            ["Software Practices", "SOLID", "OCP"]),

        new("SOLID Principles: Understanding the Liskov Substitution Principle",
            "/blog/software-practices/solid/liskov-substitution-principle",
            new DateTime(2025, 5, 31),
            "images/blog/software-practices/solid/lsp/banner.png",
            ["Software Practices", "SOLID", "LSP"]),

        new("SOLID Principles: Understanding the Interface Segregation Principle",
            "/blog/software-practices/solid/interface-segregation-principle",
            new DateTime(2025, 6, 7),
            "images/blog/software-practices/solid/isp/banner.png",
            ["Software Practices", "SOLID", "ISP"]),

        new("Connection Pooling in SQL Server: What, When, and How",
            "/blog/connection-pooling-sql-server",
            new DateTime(2025, 6, 14),
            "images/blog/connection-pooling-intro/banner.png",
            ["SQL Server", "Connection Pooling", ".NET", "Database"]),

        new("Understanding and Using OpenAI APIs: A Comprehensive Guide",
            "/blog/ai/openai",
            new DateTime(2025, 6, 21),
            "images/blog/ai/open-ai-rest-demo/banner.png",
            ["AI", "OpenAI", "API", ".NET", "C#"]),
        ];

        public static IEnumerable<PageInfo> PublishedArticles => Pages.Where(p => p.IsPublished && p.PublishedOn <= DateTime.UtcNow);
    }

    public class PageInfo(string header, string relativePath, DateTime publishedOn, string articleImage, List<string> tags, bool isPublished = true)
    {
        public string Header { get; } = header;
        public string RelativePath { get; } = relativePath;
        public List<string> Tags { get; } = tags;
        public DateTime PublishedOn { get; } = publishedOn;
        public string ArticleImage { get; } = articleImage;
        public string ArticleImageThumbnail { get; } = GetThumbnailName(articleImage);
        public bool IsPublished { get; set; } = isPublished;

        private static string GetThumbnailName(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return imagePath;

            var lastDot = imagePath.LastIndexOf('.');
            if (lastDot <= 0)
                return imagePath;

            return imagePath[..lastDot] + "-thumbnail" + imagePath[lastDot..];
        }
    }
}

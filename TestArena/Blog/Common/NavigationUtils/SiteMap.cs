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
            "images/blog/pact/intro/header_landscape.png",
            ["PACT", "API"]),

        new("Intro to PACT for .NET Core: Events based systems",
            "/blog/contract-testing-in-pact-with-events",
            new DateTime(2024, 12, 28),
            "images/blog/pact/events-demo/contract-testing-events.webp",
            ["PACT", "Events"]),

        new("Intro to PACT for .NET Core: Integration with PactFlow",
            "/blog/integration-with-pactflow",
            new DateTime(2025, 1, 11),
            "images/blog/pact/pact-broker/blog_header.png",
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
        new("Understanding the Single Responsibility Principle",
            "/blog/software-practices-solid-srp",
            new DateTime(2025, 5, 10),
            "images/blog/software-practices/solid/srp/banner.png",
            ["Software Practices", "SOLID", "SRP"], false),
        ];
    }

    public class PageInfo(string header, string relativePath, DateTime publishedOn, string articleImage, List<string> tags, bool isPublished = true)
    {
        public string Header { get; } = header;
        public string RelativePath { get; } = relativePath;
        public List<string> Tags { get; } = tags;
        public DateTime PublishedOn { get; } = publishedOn;
        public string ArticleImage { get; } = articleImage;

        public bool IsPublished { get; set; } = isPublished;
    }
}

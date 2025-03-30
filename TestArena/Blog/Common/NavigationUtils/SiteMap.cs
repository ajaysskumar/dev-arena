using System.Collections.Generic;

namespace TestArena.Blog.Common.NavigationUtils
{
    public static class SiteMap
    {
        public static List<PageInfo> Pages { get; } = new List<PageInfo>
        {
            new("Intro to PACT for .NET Core: API contract testing", "/blog/contract-testing-pact-net-intro"),
            new("Intro to PACT for .NET Core: Events based systems", "/blog/contract-testing-in-pact-with-events"),
            new("Intro to PACT for .NET Core: Integration with PactFlow", "/blog/integration-with-pactflow"),
            new("Integration testing for dotnet core APIs: Introduction", "/blog/integration-testing-in-dotnet-intro"),
            new("Integration testing for dotnet core APIs: Handling database", "/blog/integration-testing-in-dotnet-with-database"),
            new("Integration testing for dotnet core APIs: Handling APIs behind authentication", "/blog/integration-testing-in-dotnet-with-auth"),
            new("Integration testing for dotnet core APIs: Handling 3rd party service calls using wiremock", "/blog/integration-testing-in-dotnet-with-external-services"),
        };
    }

    public class PageInfo
    {
        public string Header { get; }
        public string RelativePath { get; }

        public PageInfo(string header, string relativePath)
        {
            Header = header;
            RelativePath = relativePath;
        }
    }
}

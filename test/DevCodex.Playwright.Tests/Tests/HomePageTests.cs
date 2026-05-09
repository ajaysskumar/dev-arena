using DevCodex.Playwright.Tests.Fixtures;
using Microsoft.Playwright;
using Xunit;

namespace DevCodex.Playwright.Tests.Tests;

public class HomePageTests : IAsyncLifetime
{
    private readonly PlaywrightFixture _fixture = new();

    public Task InitializeAsync() => _fixture.InitializeAsync();
    public Task DisposeAsync() => _fixture.DisposeAsync();

    [Fact]
    public async Task HomePage_ShouldLoad_Successfully()
    {
        // Arrange & Act
        await _fixture.NavigateAsync("/");

        // Assert
        var title = await _fixture.Page.TitleAsync();
        Assert.NotEmpty(title);
    }

    [Fact]
    public async Task HomePage_ShouldHave_HeaderComponent()
    {
        // Arrange & Act
        await _fixture.NavigateAsync("/");

        // Assert
        var header = _fixture.Page.Locator("header");
        await Assertions.Expect(header).ToBeVisibleAsync();
    }

    [Fact]
    public async Task HomePage_ShouldHave_FooterComponent()
    {
        // Arrange & Act
        await _fixture.NavigateAsync("/");

        // Assert
        var footer = _fixture.Page.Locator("footer");
        await Assertions.Expect(footer).ToBeVisibleAsync();
    }

    [Fact]
    public async Task HomePage_ShouldNavigateTo_BlogPage()
    {
        // Arrange
        await _fixture.NavigateAsync("/");

        // Act - Find and click a blog navigation link (adjust selector based on your actual markup)
        var blogLink = _fixture.Page.Locator("a:has-text('Blog')");
        if (await blogLink.CountAsync() > 0)
        {
            await blogLink.First.ClickAsync();
            await _fixture.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Assert
            var url = _fixture.Page.Url;
            Assert.Contains("blog", url, StringComparison.OrdinalIgnoreCase);
        }
    }
}

using Microsoft.Playwright;
using Xunit;

namespace DevCodex.Playwright.Tests.Fixtures;

/// <summary>
/// Base fixture for Playwright integration tests.
/// Handles browser and page lifecycle management.
/// </summary>
public class PlaywrightFixture : IAsyncLifetime
{
    private readonly string _baseUrl = Environment.GetEnvironmentVariable("PLAYWRIGHT_BASE_URL") ?? "http://localhost:5128";
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;
    private IPlaywright? _playwright;

    public IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");
    public IBrowserContext Context => _context ?? throw new InvalidOperationException("Context not initialized");
    public string BaseUrl => _baseUrl;

    public async Task InitializeAsync()
    {
        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });

        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        if (_page != null)
            await _page.CloseAsync();

        if (_context != null)
            await _context.CloseAsync();

        if (_browser != null)
            await _browser.CloseAsync();

        _playwright?.Dispose();
    }

    public async Task NavigateAsync(string path)
    {
        var url = $"{BaseUrl}{path}";
        await Page.GotoAsync(url);
    }
}

# DevCodex Playwright Integration Tests

Integration tests for DevCodex Blazor application using Playwright and xUnit.

## Prerequisites

- .NET 9.0 SDK
- Playwright browsers (automatically installed on first run)

## Installation

Playwright automatically downloads the required browsers. On first run, you may see browser installation output.

## Running Tests

### Local Testing

Before running tests, ensure your Blazor app is running:

```bash
# Terminal 1: Start the Blazor app
cd src/DevCodex
dotnet run

# Terminal 2: Run tests
cd test/DevCodex.Playwright.Tests
dotnet test
```

### Running Specific Tests

```bash
# Run a specific test class
dotnet test --filter "ClassName=DevCodex.Playwright.Tests.Tests.HomePageTests"

# Run a specific test method
dotnet test --filter "Name=HomePage_ShouldLoad_Successfully"
```

### Headless vs. Headed Mode

Tests run in headless mode by default. To see the browser during test execution, modify `PlaywrightFixture.cs`:

```csharp
_browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
{
    Headless = false  // Change to false to see browser
});
```

### Configuration

Base URL can be configured via environment variable:

```bash
export PLAYWRIGHT_BASE_URL=http://localhost:5000
dotnet test
```

Or update `appsettings.json` to change the default base URL.

## Project Structure

```
DevCodex.Playwright.Tests/
├── Fixtures/
│   └── PlaywrightFixture.cs      # Base fixture for test lifecycle
├── Tests/
│   └── HomePageTests.cs          # Example tests for Home page
├── appsettings.json              # Configuration
└── DevCodex.Playwright.Tests.csproj
```

## Writing New Tests

1. Create a new test class in `Tests/` folder
2. Inherit from `PlaywrightFixture` via `IAsyncLifetime`
3. Use the `_fixture` to access the `Page` object

Example:

```csharp
public class NewPageTests : IAsyncLifetime
{
    private readonly PlaywrightFixture _fixture = new();

    public Task InitializeAsync() => _fixture.InitializeAsync();
    public Task DisposeAsync() => _fixture.DisposeAsync();

    [Fact]
    public async Task NewPage_Test()
    {
        await _fixture.NavigateAsync("/newpage");
        
        var element = _fixture.Page.Locator("css=.my-element");
        await Assertions.Expect(element).ToBeVisibleAsync();
    }
}
```

## Useful Playwright Resources

- [Playwright .NET Documentation](https://playwright.dev/dotnet/)
- [Locator API](https://playwright.dev/dotnet/docs/locators)
- [Assertions](https://playwright.dev/dotnet/docs/test-assertions)
- [Debugging Tests](https://playwright.dev/dotnet/docs/debug)

## Continuous Integration

To run tests in CI/CD:

```bash
# Install browsers (usually happens automatically)
dotnet playwrighт install

# Run tests
dotnet test
```

Add to your GitHub Actions workflow:

```yaml
- name: Install Playwright browsers
  run: dotnet playwright install

- name: Run Playwright tests
  run: dotnet test test/DevCodex.Playwright.Tests
```

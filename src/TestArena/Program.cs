using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TestArena;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
if (!builder.RootComponents.Any())
{
  builder.RootComponents.Add<App>("#app");
  builder.RootComponents.Add<HeadOutlet>("head::after");
}

string sentryDsn = "https://27e4650525e825223a8f27ceaa4415fd@o4509733189844992.ingest.us.sentry.io/4509735408893952";

try
{
  // Initialize Sentry manually for Blazor WebAssembly
  SentrySdk.Init(options =>
  {
    options.Dsn = sentryDsn;
    options.TracesSampleRate = 1.0;
    options.Debug = true;
    options.Environment = builder.HostEnvironment.Environment;
  });
  
  ConfigureServices(builder.Services, builder.HostEnvironment.BaseAddress);
  
  await builder.Build().RunAsync();
}
catch (System.Exception ex)
{
  Console.WriteLine($"An error occurred during the application startup: {ex.Message}");
  SentrySdk.CaptureException(ex);
}

// 👇 extract the service-registration process to the static local function.
static void ConfigureServices(IServiceCollection services, string baseAddress)
{
  services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });
  services.AddSingleton<OpenAIService>();
}
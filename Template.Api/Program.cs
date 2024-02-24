using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
// remove default logging providers
builder.Logging.ClearProviders();
// Serilog configuration		
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
// Register Serilog
builder.Logging.AddSerilog(logger);

var app = builder.Build();

//HealthCheck Middleware
app.MapHealthChecks("/health/startup");
app.MapHealthChecks("/health/ready");
app.MapHealthChecks("/health/live");

var version = Environment.GetEnvironmentVariable("VERSION") ?? "1.0.0";
app.Logger.LogInformation($"Application Version: {version}");
app.MapGet("/", () => $"Application Version: {version}");
		
app.Run();
using Microsoft.FeatureManagement;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.AddFeatureManagement();

// Configure logging
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
		


var colours = new[]
{
    "Red", "Orange", "Yellow", "Green", "Blue", "Indigo", "Violet"
};
 
app.MapGet("/rainbow", async (IFeatureManager manager) =>
{
    if (!await manager.IsEnabledAsync("rainbow"))
    {
        return Results.NotFound();
    }
 
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new RainbowForecast
        (
            DateTime.Now.AddDays(index),
            colours[Random.Shared.Next(colours.Length)]
        ))
        .ToArray();
 
    return Results.Ok(forecast);
});
 
app.Run();

record RainbowForecast(DateTime Date, string? Colour)
{
}
using Microsoft.FeatureManagement;

public static class RainbowsEndpoints
{
    public static void RegisterRainbowsEndpoints(this WebApplication app)
    {
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
    }

    record RainbowForecast(DateTime Date, string? Colour)
    {
    }
}
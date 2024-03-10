using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.FeatureManagement;

public static class RainbowsEndpoints
{
    public static void RegisterRainbowsEndpoints(this WebApplication app)
    {
        app.MapGet("/rainbow", GetAllRainbows);
    }

    public static async Task<Results<Ok<RainbowForecast[]>, NotFound>> GetAllRainbows(IFeatureManager manager)
    {
        var colours = new[]
        {
            "Red", "Orange", "Yellow", "Green", "Blue", "Indigo", "Violet"
        };
        
        if (!await manager.IsEnabledAsync("rainbow"))
            {
                return TypedResults.NotFound();
            }
        
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new RainbowForecast
                (
                    DateTime.Now.AddDays(index),
                    colours[Random.Shared.Next(colours.Length)]
                ))
                .ToArray();
        
            return TypedResults.Ok(forecast);
    }


}
public record RainbowForecast(DateTime Date, string? Colour)
{
}
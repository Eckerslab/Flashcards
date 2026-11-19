using Microsoft.AspNetCore.Mvc;

namespace Flashcards.Auth.Controllers;

/// <summary>
///     Provides an example endpoint that returns randomly generated weather forecasts.
/// </summary>
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    /// <summary>
    /// The weather descriptions from which a random one is chosen for each forecast.
    /// </summary>
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    /// <summary>
    /// Gets randomly generated weather forecasts for the next five days.
    /// </summary>
    /// <returns>The weather forecasts for the next five days.</returns>
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}
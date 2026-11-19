namespace Flashcards.Auth;

/// <summary>
///     Represents a weather forecast for a single day.
/// </summary>
public class WeatherForecast
{
    /// <summary>
    /// Gets or sets the date of the forecast.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Gets or sets the temperature in degrees Celsius.
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// Gets the temperature in degrees Fahrenheit, calculated from <see cref="TemperatureC" />.
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// Gets or sets a short description of the weather.
    /// </summary>
    public string? Summary { get; set; }
}

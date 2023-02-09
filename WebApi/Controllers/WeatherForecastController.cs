using System.Net.Mime;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> logger;
    private readonly WeatherForecastFeatureFlags flags;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
        IOptions<WeatherForecastFeatureFlags> options)
    {
        flags = options.Value;
        this.logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json)]
    public IEnumerable<WeatherForecast> Get()
    {
        throw new NotImplementedException();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        })
        .ToArray();
    }

    //[HttpPost]
    //public IActionResult Post([FromBody, Bind("a", "b")]MyModel model)
    //{
    //    return Ok();
    //}

    [HttpPost]
    public IActionResult Create(
        [Bind("lastName,firstName")]
        Instructor instructor)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return Ok();
    }

    public class WeatherForecastFeatureFlags
    {
        public bool ShouldUseCelsius { get; set; }
    }
}

public class Instructor
{
    public int ID { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
}
public class MyModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public bool IsAdmin { get; set; }
}

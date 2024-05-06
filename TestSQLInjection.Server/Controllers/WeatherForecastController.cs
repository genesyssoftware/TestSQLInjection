using Microsoft.AspNetCore.Mvc;
using TestSQLInjection.Server.Models;
using TestSQLInjection.Server.Services;

namespace TestSQLInjection.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPut(Name = "PutWeatherForecast")]
        public async Task<IActionResult> Put([FromBody] string cityName)
        {
            var _wetherService = new WeatherService(); ;
            var test3 = await _wetherService.TestSQLInjection1(cityName);
            Console.WriteLine("resut 1 " + test3.Count().ToString());

            var test4 = await  _wetherService.TestSQLInjection2(cityName);
            Console.WriteLine("resut 2 " + test4.Count().ToString());

            return Ok($"Received value: {cityName}");

        }
      
    }
}

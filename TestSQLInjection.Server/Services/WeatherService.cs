using Microsoft.EntityFrameworkCore;
using TestSQLInjection.Server.Extensions;
using TestSQLInjection.Server.Models;

namespace TestSQLInjection.Server.Services
{
    public class WeatherService
    {
        private readonly DbContext _ctx;
        public async Task<List<WeatherForecast>> TestSQLInjection1(string cityName)
        {

            var results = await _ctx.Database.SqlQuery<WeatherForecast>($"SELECT * FROM dbo.WeatherForecast WHERE summary = '" + cityName + "'").ToListAsync();

            return results;
        }

        public async Task<List<WeatherForecast>> TestSQLInjection2(string cityName)
        {

            var results = await _ctx.Database.SqlQuery<WeatherForecast>($"SELECT * FROM dbo.WeatherForecast WHERE summary = '{cityName}'").ToListAsync();

            return results;
        }
    }

  
}

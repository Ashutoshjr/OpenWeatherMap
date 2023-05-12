using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenWeatherMap.Models;
using OpenWeatherMap.Services;

namespace OpenWeatherMap.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {

        private readonly IWeatherService _weatherservice;

        public WeatherController(IWeatherService weatherservice)
        {
            _weatherservice = weatherservice;
        }


        [HttpGet("{location}")]
        public async Task<CityWeatherData> GetWeather(string location)
        {
            return await _weatherservice.GetWeatherData(location);
        }

    }
}

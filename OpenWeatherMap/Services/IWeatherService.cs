using OpenWeatherMap.Models;

namespace OpenWeatherMap.Services
{
    public interface IWeatherService
    {
        Task<CityWeatherData> GetWeatherData(string location);
    }
}

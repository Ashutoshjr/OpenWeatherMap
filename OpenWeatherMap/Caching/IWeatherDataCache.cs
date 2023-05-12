using OpenWeatherMap.Models;

namespace OpenWeatherMap.Caching
{
    public interface IWeatherDataCache
    {
        void AddOrUpdate(string key, CityWeatherData weatherData, TimeSpan expiration);
        bool TryGet(string key, out CityWeatherData weatherData);
    }
}

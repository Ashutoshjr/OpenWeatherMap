using OpenWeatherMap.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OpenWeatherMap.Caching
{
    public class WeatherDataCache : IWeatherDataCache
    {

        private readonly ConcurrentDictionary<string, CacheItem<CityWeatherData>> cache;

        public WeatherDataCache()
        {
            cache = new ConcurrentDictionary<string, CacheItem<CityWeatherData>>();
        }

        public void AddOrUpdate(string key, CityWeatherData weatherData, TimeSpan expiration)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (weatherData == null)
            {
                throw new ArgumentNullException(nameof(weatherData));
            }

            var cacheItem = new CacheItem<CityWeatherData>(weatherData, DateTime.UtcNow.Add(expiration));
            cache[key] = cacheItem;
        }

        public bool TryGet(string key, out CityWeatherData weatherData)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (cache.TryGetValue(key, out var cacheItem))
            {
                if (cacheItem.Expiration >= DateTime.UtcNow)
                {
                    weatherData = cacheItem.Value;
                    return true;
                }
                else
                {
                    // Remove expired item from cache
                    cache.Remove(key, out CacheItem<CityWeatherData> value);
                }
            }

            weatherData = null;
            return false;
        }

        private class CacheItem<T>
        {
            public T Value { get; }
            public DateTime Expiration { get; }

            public CacheItem(T value, DateTime expiration)
            {
                Value = value;
                Expiration = expiration;
            }
        }
    }


}

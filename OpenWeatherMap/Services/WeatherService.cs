using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenWeatherMap.Caching;
using OpenWeatherMap.Models;
using OpenWeatherMap.Utility;

namespace OpenWeatherMap.Services
{
    public class WeatherService : IWeatherService
    {

        public int Temperature { get; set; }
        public string Location { get; set; }
        public double WindSpeed { get; set; }
        public string Description { get; set; }

        private readonly IWeatherDataCache _weatherDataCache;
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;

        public WeatherService(IOptions<AppSettings> appSettingsOptions, IWeatherDataCache cache, IHttpClientFactory httpClientFactory)
        {
            _weatherDataCache = cache;
            _httpClient = httpClientFactory.CreateClient();
            _appSettings = appSettingsOptions.Value;
        }

        public async Task<CityWeatherData> GetWeatherData(string location)
        {
            try
            {
                string apiUrl = $"{_appSettings.Schema}://{_appSettings.BaseUrl}{_appSettings.EndPointPath}?q={location}&appid={_appSettings.OpenWeatherMapApiKey}";

                if (_weatherDataCache.TryGet(location, out CityWeatherData weatherData))
                    return weatherData;

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var weatherResult = JsonConvert.DeserializeObject<WeatherData>(jsonResponse);

                    var cityWeatherResult = new CityWeatherData
                    {
                        Description = weatherResult.Weather.Select(s => s.Description).FirstOrDefault(),
                        Location = weatherResult.Name,
                        Temperature = weatherResult.Main.Temp,
                        WindSpeed = weatherResult.Wind.Speed
                    };

                    // Cache the retrieved weather data
                    _weatherDataCache.AddOrUpdate(location, cityWeatherResult, TimeSpan.FromMinutes(_appSettings.DataCacheExpiration)); // Adjust the expiration as needed

                    return cityWeatherResult;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return new CityWeatherData();
        }

    }
}

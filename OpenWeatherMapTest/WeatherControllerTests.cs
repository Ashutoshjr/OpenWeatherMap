using OpenWeatherMap.Controllers;
using System.Net;
using FluentAssertions;
using OpenWeatherMap.Services;
using System.Net.Http;
using Moq;
using OpenWeatherMap.Utility;
using OpenWeatherMap.Caching;
using OpenWeatherMap.Models;
using Microsoft.Extensions.Options;

namespace OpenWeatherMapTest
{
    public class WeatherControllerTests
    {
        [Fact]
        public async void GetWeather_ValidLocation_ReturnsWeatherData()
        {
            // Arrange
            string location = "Munich";
            string expectedWeatherData = "moderate rain";

            // Create a mock HTTP client
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var mockHttpClient = new Mock<HttpClient>();

            httpClientFactoryMock.Setup(client => client.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

            // Create a mock for IWeatherDataCache
            var weatherDataCacheMock = new Mock<IWeatherDataCache>();

            // Create a mock CityWeatherData object with desired data
            CityWeatherData cityWeatherData = new CityWeatherData
            {
                Temperature = 25,
                Location = "Munich",
                WindSpeed = 10,
                Description = "moderate rain",
            };
            weatherDataCacheMock.Setup(cache => cache.TryGet(It.IsAny<string>(),out cityWeatherData))
                .Returns((string location, CityWeatherData weatherData) =>
                {
                    if (location == "Munich")
                        return true; // Return true if weather data exists in the cache for the location
                    else if (location == "Paris")
                        return true; // Return true if weather data exists in the cache for the location
                    return false;
                });


            var appSettingsMock = new Mock<IOptions<AppSettings>>();
            var appSettings = new AppSettings();

            appSettingsMock.Setup(x => x.Value).Returns(appSettings);

            var weatherService = new WeatherService(appSettingsMock.Object, weatherDataCacheMock.Object, httpClientFactoryMock.Object);

            // Act
            var weatherData = await weatherService.GetWeatherData(location);

            // Assert
            weatherData.Description.Should().Be(expectedWeatherData);
          
        }


        [Fact]
        public async void GetWeather_InValidLocation_ReturnsNull()
        {
            // Arrange
            string location = "London";
            string? expectedWeatherData = null;

            // Create a mock HTTP client
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var mockHttpClient = new Mock<HttpClient>();
            httpClientFactoryMock.Setup(client => client.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

            // Create a mock for IWeatherDataCache
            var weatherDataCacheMock = new Mock<IWeatherDataCache>();
            CityWeatherData cityWeatherData = new CityWeatherData
            {
                Temperature = 25,
                Location = "Munich",
                WindSpeed = 10,
                Description = "moderate rain",
            };
            weatherDataCacheMock.Setup(cache => cache.TryGet(It.IsAny<string>(), out cityWeatherData))
                .Returns((string location, CityWeatherData weatherData) =>
                {
                    if (location == "Munich")
                        return true; // Return true if weather data exists in the cache for the location
                    return false;
                });


            var appSettingsMock = new Mock<IOptions<AppSettings>>();
            var appSettings = new AppSettings();

            appSettingsMock.Setup(x => x.Value).Returns(appSettings);

            var weatherService = new WeatherService(appSettingsMock.Object, weatherDataCacheMock.Object, httpClientFactoryMock.Object);

            // Act
            var weatherData = await weatherService.GetWeatherData(location);

            // Assert
            weatherData.Description.Should().Be(expectedWeatherData);

        }
    }
}
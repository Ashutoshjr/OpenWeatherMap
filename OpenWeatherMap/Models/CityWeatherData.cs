namespace OpenWeatherMap.Models
{
    public class CityWeatherData
    {

        public Double Temperature { get; set; }
        public string Location { get; set; }
        public Double WindSpeed { get; set; }
        public string? Description { get; set; }

    }
}

namespace OpenWeatherMap.Utility
{
    public class AppSettings
    {
        public string OpenWeatherMapApiKey { get; set; }
        public string Schema { get; set; }
        public string BaseUrl { get; set; }
        public string EndPointPath { get; set; }
        public double DataCacheExpiration { get; set; }
    }
}

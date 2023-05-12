using Microsoft.Extensions.Configuration;
using OpenWeatherMap.Caching;
using OpenWeatherMap.Factory;
using OpenWeatherMap.Services;
using OpenWeatherMap.Utility;

namespace OpenWeatherMap
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
            

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IWeatherService,WeatherService>();
            builder.Services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            builder.Services.AddSingleton<IWeatherDataCache, WeatherDataCache>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseRouting();

            app.MapControllers();

            app.Run();
        }
    }
}
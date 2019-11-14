using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebClient.Services
{
    public class WeatherService : IWeatherService
    {
        private HttpClient httpClient;
        public WeatherService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri("http://www.weather.com.cn");
            this.httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public string GetCityCode()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("JsonFile/weatherCode.json");
            var result = builder.Build().GetSection("citycode").ToString() ;
            return result;
        }

        public async Task<WeatherInfo> GetData()
        {
            var data = await this.httpClient.GetAsync("/data/sk/101010100.html");
            string result = await data.Content.ReadAsStringAsync();
            WeatherForecast w = JsonConvert.DeserializeObject<WeatherForecast>(result);
            return w.weatherinfo;
        }
    }
}

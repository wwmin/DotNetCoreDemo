﻿using System;
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

        public async Task<string> GetData()
        {
            var data = await this.httpClient.GetAsync("/data/sk/101010100.html");
            var result = await data.Content.ReadAsStringAsync();
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCaching.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace webRedisCache.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IEasyCachingProvider cachingProvider;
        private readonly IEasyCachingProviderFactory easyCachingProviderFactory;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IEasyCachingProviderFactory cachingProviderFactroy)
        {
            _logger = logger;
            this.easyCachingProviderFactory = cachingProviderFactroy;
            this.cachingProvider = cachingProviderFactroy.GetCachingProvider("RedisExample");
        }

        [HttpGet]
        public IActionResult Get()
        {
            var rng = new Random();

            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
            this.cachingProvider.Set("weather use easycaching", JsonConvert.SerializeObject(result), TimeSpan.FromSeconds(60));
            return Ok(result);
        }

        [HttpGet("cache")]
        public IActionResult GetCache()
        {
            var item = this.cachingProvider.Get<string>("weather use easycaching");
            return Ok(item);
        }
    }
}

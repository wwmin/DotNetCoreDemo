using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebClient.Services;

namespace WebClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IWeatherService weatherService;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
        {
            this.weatherService = weatherService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            string result = string.Empty;
            try
            {
                result = await weatherService.GetData();
            }
            catch
            {
            }
            return new JsonResult(new { result });
        }
    }
}

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

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
        {
            this.weatherService = weatherService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            WeatherInfo result = null;
            try
            {
                result = await weatherService.GetData();
            }
            catch
            {
                return BadRequest();
            }
            return new JsonResult(result);
        }

        [HttpGet("cityCode")]
        public ActionResult GetCityCode()
        {
            string result = null;
            try
            {
                result = weatherService.GetCityCode();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return new JsonResult(result);
        }
    }
}

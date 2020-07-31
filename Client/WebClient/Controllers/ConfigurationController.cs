using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WebClient.Model;

namespace WebClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly Greeting greeting;
        public ConfigurationController(IConfiguration configuration, IOptions<Greeting> options)
        {
            this.configuration = configuration;
            greeting = options.Value;
        }
        /// <summary>
        /// Using Configuration values
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetWayOne")]
        public async Task<IActionResult> GetWayOne()
        {
            var city = configuration.GetValue<string>("Greeting:Information:City");
            var country = configuration.GetValue<string>("Greeting:Information:Country");
            var infomation = await Task.Run(() => $"欢迎来到{country}-{city}");
            return Ok(infomation);
        }
        /// <summary>
        /// Using Configuration GetSection
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetWayTwo")]
        public async Task<IActionResult> GetWayTwo()
        {
            var information = configuration.GetSection("Greeting:Information");
            var city = information["City"];
            var country = information["Country"];
            var infomation = await Task.Run(() => $"欢迎来到{country}-{city}");
            return Ok(infomation);
        }


        /// <summary>
        /// Using Bind Configuration
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetWayThree")]
        public async Task<IActionResult> GetWayThree()
        {
            var greeting = new Greeting();
            configuration.Bind("Greeting:Information", greeting);
            var infomation = await Task.Run(() => $"欢迎来到{greeting.Country}-{greeting.City}");
            return Ok(infomation);
        }

        /// <summary>
        /// Using IOption pattern for configuration
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetWayFour")]
        public async Task<IActionResult> GetWayFour()
        {
            var infomation = await Task.Run(() => $"欢迎来到{greeting.Country}-{greeting.City}");
            return Ok(infomation);
        }
    }
}

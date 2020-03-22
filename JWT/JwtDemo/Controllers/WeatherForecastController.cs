using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JwtDemo.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JwtDemo.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IAuthorizationService _authorizationService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IAuthorizationService authorizationService)
        {
            _logger = logger;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var document = new Document { Creator = "Read" };

            var user = HttpContext.User;
            var isAuth = (await _authorizationService.AuthorizeAsync(user, document, Operations.Read)).Succeeded;
            if (!isAuth)
            {
                //无权限
                return null;
            }
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("user")]
        public IActionResult getUser()
        {
            var user = HttpContext.User;
            var name = user.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name);
            return Ok(name?.Value);
        }

        [Authorize]
        [HttpGet("getId")]
        public IActionResult GetId()
        {
            return Ok(new Random().Next(1));
        }
    }
}

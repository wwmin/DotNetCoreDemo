using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using webRedisCache.Common;

namespace webRedisCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly RedisHostOptions _redisOptions;
        private readonly RedisHostOptions _redisSecretOptions;
        public ConfigController(ILogger<WeatherForecastController> logger, IOptions<RedisHostOptions> redisOptions, IOptions<RedisHostOptions> redisSecretOptions)
        {
            _logger = logger;
            _redisOptions = redisOptions.Value;
            _redisSecretOptions = redisSecretOptions.Value;
        }

        [HttpGet("redis")]
        public IActionResult GetRedisConfig()
        {
            var result = _redisOptions;
            return Ok(result);
        }

        [HttpGet("redis/secret")]
        public IActionResult GetRedisSecretConfig()
        {
            var result = _redisSecretOptions;
            return Ok(result);
        }

        [HttpGet("redis/secret/custom")]
        public IActionResult GetRedisSecretConfigByCustomMethod()
        {
            // 读取自定义json文件
            RedisHostOptions redisOption = JsonConfigHelper.GetAppSettings<RedisHostOptions>("JsonFile/secret.json", "redis");
            return Ok(redisOption);
        }
    }
}
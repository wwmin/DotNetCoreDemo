using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ.WebApi.MQModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyNetQ.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MQController : ControllerBase
    {
        private readonly ILogger<MQController> _log;
        private readonly IBus _bus;
        public MQController(ILogger<MQController> log, IBus bus)
        {
            _log = log;
            _bus = bus;
        }

        [HttpPost("order")]
        public async Task Post([FromBody]OrderMessage message)
        {
            _log.LogInformation(message.text + "," + message.id);
            await _bus.PublishAsync(message);
        }
    }
}
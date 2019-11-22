using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ.WebApi.MQModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyNetQ.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MQController : ControllerBase
    {
        private readonly IBus _bus;
        public MQController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost("order")]
        public async Task Post([FromBody]OrderMessage message)
        {
            await _bus.PublishAsync(message);
        }
    }
}
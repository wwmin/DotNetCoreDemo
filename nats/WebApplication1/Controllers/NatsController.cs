using AlterNats;

using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NatsController : ControllerBase
    {


        private readonly ILogger<NatsController> _logger;
        private readonly INatsCommand _command;

        public NatsController(ILogger<NatsController> logger, INatsCommand command)
        {
            _logger = logger;
            _command = command;
        }

        [HttpGet, Route("subscribe")]
        public void Subscribe() => _command.SubscribeAsync("foo", (Person x) => Console.WriteLine($"received {x}"));


        [HttpGet, Route("publish")]
        public void Publish([FromQuery] Person x) => _command.PublishAsync("foo", x);
    }
}
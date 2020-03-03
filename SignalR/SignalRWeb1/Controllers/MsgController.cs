using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRWeb1.Hubs;
using static SignalRWeb1.Models.Input.MsgInput;

namespace SignalRWeb1.Controllers
{
    /// <summary>
    /// 发送数据
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MsgController : ControllerBase
    {
        private readonly ILogger<MsgController> _log;
        private readonly IHubContext<PostHub> _postHub;
        public MsgController(ILogger<MsgController> log, IHubContext<PostHub> hubContext)
        {
            _log = log;
            _postHub = hubContext;
        }


        /// <summary>
        /// 发送数据to connectionId
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("sendByConnectionId")]
        public async Task<IActionResult> SendByConnectionId([FromBody] SendToConnectionIdMsgInput input)
        {
            await _postHub.Clients.Client(input.ConnectionId).SendAsync("Show", input.Message);
            return Ok();
        }


        /// <summary>
        /// 发送数据to group
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("sendByGroup")]
        public async Task<IActionResult> SendByGroup([FromBody] SendToGroupMsgInput input)
        {
            await _postHub.Clients.Group(input.GroupId).SendAsync("Show", input.Message);
            return Ok();
        }
    }
}
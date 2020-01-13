using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapWeb1.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ConsumerController : ControllerBase
    {
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="time"></param>
        [NonAction]
        [CapSubscribe("test.show.time")]
        public void ReceiveMessage(DateTime time)
        {
            Console.WriteLine("message time is:" + time);
        }

        /// <summary>
        /// 包含头信息的消息
        /// </summary>
        /// <param name="time"></param>
        /// <param name="header"></param>
        [CapSubscribe("test.show.time.header")]
        public void ReceiveMessageWithHeader(DateTime time, [FromCap]CapHeader header)
        {
            Console.WriteLine("message time is:" + time);
            Console.WriteLine("message first header:" + header["my.header.first"]);
            Console.WriteLine("message second header:" + header["my.header.second"]);
        }
    }
}
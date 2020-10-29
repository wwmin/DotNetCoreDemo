using freeSqlWeb1.Infrastructures.Utils;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace freeSqlWeb1.Infrastructures.MiddleWares
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var value = context.Request.Query["value"].ToString();
            if (int.TryParse(value, out var number))
            {
               
            }
            RedisChannel rc = new RedisChannel("request.value", RedisChannel.PatternMode.Auto);
            RedisUtil.MessagePub(rc, number);
        }
    }
}

using EasyNetQ;
using Messages;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.MiddleWare
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IBus _bus;
        public RequestMiddleware(RequestDelegate next)
        {
            _next = next;
            _bus = RabbitHutch.CreateBus("host=localhost");
        }

        public async Task Invoke(HttpContext context)
        {
            var value = context.Request.Query["value"].ToString();

            if (int.TryParse(value, out var number))
            {
                _bus.Publish(new TextMessage
                {
                    Text = value
                }); ;
                await context.Response.WriteAsync($"the number is {number}");
            }
            else
            {
                context.Items["value"] = value;
                await _next(context);
            }
        }
    }
}

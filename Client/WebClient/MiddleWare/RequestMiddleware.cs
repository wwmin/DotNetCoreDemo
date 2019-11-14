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
        public RequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var value = context.Request.Query["value"].ToString();
            if(int.TryParse(value,out var number))
            {
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

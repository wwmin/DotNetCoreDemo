using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace quartz.api
{
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<Startup>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<EqidCounterResetJob>();
            services.AddSingleton<QuartzStartup>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //nginx等代理服务器在转发时，会使用X-Forwarded-For 请求头。该请求头会记录从请求者ip到层层代理服务器ip的信息。
            //使用X - Forwarded - For会更新 HttpContext.Connection.RemoteIpAddress
            //使用X - Forwarded - Proto会更新 HttpContext.Request.Scheme
            //使用X - Forwarded - Host会更新 HttpContext.Request.Host
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            var a = 1;
            int b = a | 2;

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace quartz.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var webHost = CreateHostBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configureDelagate) =>
                {
                    //configureDelegate默认会按照如下顺序加载ChainedConfiguration、appsetting.*.json，Environment、CommandLine
                    configureDelagate.AddJsonFile($"appsettings.secrets.json", optional: true, reloadOnChange: true);
                })
                .ConfigureLogging((hostingContext, loggingBuilder) =>
                {
                    loggingBuilder.AddConsole(x => x.IncludeScopes = true).AddDebug();
                })
                .UseNLog()
                .Build();
            webHost.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

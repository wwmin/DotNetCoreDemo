using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot;
using Ocelot.DependencyInjection;
using OcelotBasic;

namespace OcelotBasic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            //new WebHostBuilder()
            //   .UseKestrel()
            //   .UseContentRoot(Directory.GetCurrentDirectory())
            //   .ConfigureAppConfiguration((hostingContext, config) =>
            //   {
            //       config
            //           .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
            //           .AddJsonFile("appsettings.json", true, true)
            //           .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
            //           .AddJsonFile("ocelot.json")
            //           .AddEnvironmentVariables();
            //   })
            //   .ConfigureServices(s => {
            //       s.AddOcelot();
            //   })
            //   .ConfigureLogging((hostingContext, logging) =>
            //   {
            //       //add your logging
            //   })
            //   .UseIISIntegration()
            //   .Configure(app =>
            //   {
            //       app.UseOcelot().Wait();
            //   })
            //   .Build()
            //   .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true,
                            true)
                        //.AddOcelot("ocelot.json", true, true)
                        //.AddOcelot(hostingContext.HostingEnvironment)
                        //.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json")
                        //.AddOcelot((IWebHostEnvironment) hostingContext.HostingEnvironment)
                        .AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

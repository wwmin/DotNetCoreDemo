using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.ServiceProcess;

namespace FileMonitorService
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Nlog
            var logger = LogManager.GetLogger("*");
            logger.Info("init");

            var serviceProvider = BuildDi();
            var runner = serviceProvider.GetRequiredService<Runner>();
            runner.DoAction("run action");
            #endregion

            //ServiceBase[] services = new ServiceBase[] { new FileMonitorService() };
            //ServiceBase.Run(services);
            Console.ReadKey();
        }

        private static IServiceProvider BuildDi()
        {
            return new ServiceCollection().AddLogging(builder =>
            {
                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageProperties = true,
                    CaptureMessageTemplates = true
                }) ;
            }).AddTransient<Runner>()
            .BuildServiceProvider();
        }
    }
}

using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quartz.api
{
    using IOCContainer = IServiceProvider;
    public class QuartzStartup
    {

        public IScheduler Scheduler { get; set; }

        private readonly ILogger _logger;
        private readonly IJobFactory iocJobFactory;
        public QuartzStartup(IOCContainer IocContainer, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<QuartzStartup>();
            iocJobFactory = new IOCJobFactory(IocContainer);
            var schedulerFactory = new StdSchedulerFactory();
            Scheduler = schedulerFactory.GetScheduler().Result;
            Scheduler.JobFactory = iocJobFactory;
        }

        public void ScheduleJob()
        {
            _logger.LogInformation("Schedule job load as application start.");
            Scheduler.Start().Wait();
            var EqidCounterResetJob = JobBuilder.Create<EqidCounterResetJob>()
                .WithIdentity("EqidCounterResetJob")
                .Build();
            var EqidCounterResetJobTrigger = TriggerBuilder.Create()
                .WithIdentity("EqidCounterResetCron")
                .StartNow()
                .WithCronSchedule("0/30 * * * * ?")
                .Build();
            Scheduler.ScheduleJob(EqidCounterResetJob, EqidCounterResetJobTrigger).Wait();

        }

        public void EndScheduler()
        {
            if (Scheduler == null) return;
            if (Scheduler.Shutdown(waitForJobsToComplete: true).Wait(30_0000)) Scheduler = null;
            _logger.LogError("Schedule job upload as application stopped");
        }
    }
}

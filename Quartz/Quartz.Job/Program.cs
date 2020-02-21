using System;
using Quartz.Impl;
using Quartz.Job.Job;

namespace Quartz.Job
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Quartz! Main");

            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = factory.GetScheduler().Result;

            //将job和trigger注册到scheduler中
            scheduler.ScheduleJob(MyJob.GetJobDetail(), MyJob.GetTrigger());

            //start让调度线程启动[调度线程可以从job store中获取快要执行的trigger,然后获取trigger关联的job,执行job]
            scheduler.Start();
            Console.ReadKey();
        }
    }
}

using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace receive
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvder());

            RunProgram().GetAwaiter().GetResult();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to close the application");
            Console.ResetColor();
            Console.ReadKey();
        }

        private static async Task RunProgram()
        {
            try
            {
                // Grab the Scheduler instance from the Factory
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                IScheduler scheduler = await factory.GetScheduler();

                // and start it off
                await scheduler.Start();

                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<HelloJob>()
                    .WithIdentity("job1", "group1")
                    .Build();

                // Trigger the job to run now, and then repeat every 10 seconds
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(1)
                        .RepeatForever())
                    .Build();

                // Tell quartz to schedule the job using our trigger
                await scheduler.ScheduleJob(job, trigger);

                // some sleep to show what's happening
                //await Task.Delay(TimeSpan.FromSeconds(60));

                // and last shut down the scheduler when you are ready to close your program
                //await scheduler.Shutdown();
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }

        private class ConsoleLogProvder : ILogProvider
        {
            public Logger GetLogger(string name)
            {
                return (level, func, exception, parameters) =>
                {
                    if (level >= LogLevel.Info && func != null)
                    {
                        Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                    }
                    return true;
                };
            }

            public IDisposable OpenNestedContext(string message)
            {
                throw new NotImplementedException();
            }

            public IDisposable OpenMappedContext(string key, string value)
            {
                throw new NotImplementedException();
            }
        }
    }
    public class HelloJob : IJob
    {
        static MessageQueue mq = MSMQHelper.CreateQueue();
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("Greetings from HelloJob!");
            Task task = new Task(Init);
            task.Start();
        }

        public static void Init()
        {
            var file = MSMQHelper.ImportQueue(mq);
            //执行备份的文件
            //Process proc = new Process();
            //proc.StartInfo.WorkingDirectory = file;
            //proc.StartInfo.FileName = "command.bat";
            //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;//这里设置DOS窗口不显示
            //proc.StartInfo.CreateNoWindow = false;
            //proc.Start();
            Console.WriteLine("项目" + file + "执行时间:" + DateTime.Now.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quartz.Job.Job
{
    public class MyJob : IJob
    {
  
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Hello Quartz!");
                Thread.Sleep(1000 * 2);//模拟执行任务
                //JobDetail的key就是job的分组和job的名字
                Console.WriteLine($"JobDetail的组和名字:{context.JobDetail.Key},{context.JobDetail.Description}");
                Console.WriteLine();
            });
        }

        public static IJobDetail GetJobDetail()
        {
            return JobBuilder.Create<MyJob>()//获取JobBuilder
                .WithIdentity("job2")//添加job的名字和分组
                .WithDescription("任务描述")//添加描述
                .Build();//生成IJobDetail
        }

        public static ITrigger GetTrigger()
        {
            return TriggerBuilder.Create()//获取TriggerBuilder
                .StartNow()
                //.StartAt(DateBuilder.TodayAt(01, 00, 00))//开始时间,今天的1点(hh,mm,ss) , 可使用StartNow()
                .ForJob(GetJobDetail())//将触发器关联给指定的job
                .WithPriority(10)//优先级,当触发时间一样时,优先级大的触发器先执行
                .WithIdentity("t2")//添加名字和分组
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(3)/*调度方案,周期 n秒执行一次*/.WithRepeatCount(10)/*重复执行次数,-1为无限次*/.Build()).Build();
        }
    }
}

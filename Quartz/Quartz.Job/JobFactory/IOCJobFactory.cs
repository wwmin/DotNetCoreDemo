using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quartz.Job.JobFactory
{
    /// <summary>
    /// IOCJobFactory: 在Timer出发的时候产生对应的Job实例
    /// </summary>
    public class IOCJobFactory : IJobFactory
    {
        protected readonly IServiceProvider Container;
        public IOCJobFactory(IServiceProvider container)
        {
            Container = container;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return Container.GetService(bundle.JobDetail.JobType) as IJob;
            //throw new NotImplementedException();
        }

        public void ReturnJob(IJob job)
        {
            //throw new NotImplementedException();
        }
    }
}

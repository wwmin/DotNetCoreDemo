using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quartz.api
{
    using IOCContainer = IServiceProvider;
    public class IOCJobFactory : IJobFactory
    {
        protected readonly IOCContainer Container;

        public IOCJobFactory(IOCContainer container)
        {
            Container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            //throw new NotImplementedException();
            return Container.GetService(bundle.JobDetail.JobType) as IJob;
        }

        //Allows the job factory to destroy/cleanup the job if needed.
        public void ReturnJob(IJob job)
        {
            //throw new NotImplementedException();

        }
    }
}

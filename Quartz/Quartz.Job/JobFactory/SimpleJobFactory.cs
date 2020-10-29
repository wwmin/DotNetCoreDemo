using log4net;
using Microsoft.Extensions.Logging;
using Quartz.Spi;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quartz.Job.JobFactory
{
    public class SimpleJobFactory : IJobFactory
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(SimpleJobFactory));

        /// <summary>
        /// the default JobFactory used by Quartz - simply calls
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public virtual IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            IJobDetail jobDetail = bundle.JobDetail;
            Type jobType = jobDetail.JobType;
            try
            {
                if (log.IsDebugEnabled)
                {
                    log.Debug($"Producing instance of Job '{jobDetail.Key}', class={jobType.FullName}");
                }
                return ObjectUtils.InstantiateType<IJob>(jobType);
            }
            catch (Exception ex)
            {
                SchedulerException se = new SchedulerException($"Problem instantiating class '{jobDetail.JobType.FullName}' ", ex);
                throw se;
            }
            throw new NotImplementedException();
        }

        public void ReturnJob(IJob job)
        {
            throw new NotImplementedException();
        }
    }
}

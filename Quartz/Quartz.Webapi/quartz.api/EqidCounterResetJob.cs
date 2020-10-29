using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quartz.api
{
    public class EqidCounterResetJob : IJob
    {
        private readonly ILogger _logger;
        public EqidCounterResetJob(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EqidCounterResetJob>();
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //throw new NotImplementedException();
            _logger.LogInformation($"{nameof(EqidCounterResetJob)} Schedule job executed.");
            await Task.CompletedTask;
        }
    }
}

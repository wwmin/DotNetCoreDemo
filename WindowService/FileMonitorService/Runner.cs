using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileMonitorService
{
    public class Runner
    {
        private readonly ILogger<Runner> _logger;
        public Runner(ILogger<Runner> logger)
        {
            _logger = logger;
        }

        public void DoAction(string name)
        {
            _logger.LogInformation($"name: {name}");
        }
    }
}

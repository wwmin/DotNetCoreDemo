using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace freeSqlWeb1.Services
{
    public class EmailService : IMessageService
    {
        private ILogger _log;
        public EmailService(ILogger log)
        {
            _log = log;
        }
        public string Send(string msg)
        {
            _log.LogInformation(msg);
            return msg;
        }
    }
}

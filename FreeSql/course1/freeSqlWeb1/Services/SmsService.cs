using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace freeSqlWeb1.Services
{
    public class SmsService : IMessageService
    {
        public string Send(string msg)
        {
            return msg;
        }
    }
}

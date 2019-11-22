using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyNetQ.WebApi.MQModels
{
    
    [Queue("test.order",ExchangeName ="test.order")]
    public class OrderMessage
    {
        public string text { get; set; }
        public int id { get; set; }
    }
}

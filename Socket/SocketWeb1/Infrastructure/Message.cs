using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocketWeb1.Infrastructure
{
    public class Message
    {
        public string SendClientId { get; set; }
        public string action { get; set; }
        public string msg { get; set; }
        public string nick { get; set; }
    }
}

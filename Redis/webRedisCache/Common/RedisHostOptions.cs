using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webRedisCache.Common
{
    public class RedisHostOptions
    {
        /// <summary>
        /// 连接地址
        /// </summary>
        public string host { get; set; }
        /// <summary>
        /// 连接端口
        /// </summary>
        public int port { get; set; }
    }
}

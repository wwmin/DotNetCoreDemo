using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtDemo.Configuration
{
    /// <summary>
    /// JwtOptions
    /// </summary>
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        /// <summary>
        /// 过期时间(分钟)
        /// </summary>
        public double ExpireMinutes { get; set; }
        /// <summary>
        /// 加密密钥
        /// </summary>
        public string SecurityKey { get; set; }
    }
}

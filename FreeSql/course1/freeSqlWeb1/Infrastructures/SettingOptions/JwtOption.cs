using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace freeSqlWeb1.Infrastructures.SettingModels
{
    /// <summary>
    /// setting models
    /// </summary>
    public class JwtOption
    {
        /// <summary>
        /// ServerSecret
        /// </summary>
        public string ServerSecret { get; set; }
        /// <summary>
        /// Issuer
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// Audience
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// Lifetime
        /// </summary>
        public int Lifetime { get; set; }
        /// <summary>
        /// RenewwalTime
        /// </summary>
        public int RenewwalTime { get; set; }
        /// <summary>
        /// ValidateLifetime
        /// </summary>
        public bool ValidateLifetime { get; set; }
        /// <summary>
        /// HeadField
        /// </summary>
        public string HeadField { get; set; }
        /// <summary>
        /// ReTokenHeadField
        /// </summary>
        public string ReTokenHeadField { get; set; }
        /// <summary>
        /// Prefix
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// IgnoreUrls
        /// </summary>
        public string[] IgnoreUrls { get; set; }
    }
}

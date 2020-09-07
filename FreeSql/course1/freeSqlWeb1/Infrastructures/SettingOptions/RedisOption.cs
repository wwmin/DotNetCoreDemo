using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace freeSqlWeb1.Infrastructures.SettingOptions
{
    /// <summary>
    /// RedisOptions
    /// </summary>
    public class RedisOption
    {
        /// <summary>
        /// InstanceName
        /// </summary>
        public string InstanceName { get; set; }
        /// <summary>
        /// ConnectionString
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// DefaultDatabase
        /// </summary>
        public int DefaultDatabase { get; set; }
        /// <summary>
        /// IsOpenRedis
        /// </summary>
        public bool IsOpenRedis { get; set; }
        /// <summary>
        /// 默认过期时间 (分钟)
        /// </summary>
        public int DefaultExpireTime { get; set; }

        /// <summary>
        /// 数据库表存储缓存时间(分钟)
        /// </summary>
        public int DbTableCacheMinutes { get; set; }
        /// <summary>
        /// 数据库表存储缓存滑动过期时间(分钟)
        /// </summary>
        public int DbTableCacheSlideMinutes { get; set; }
    }
}

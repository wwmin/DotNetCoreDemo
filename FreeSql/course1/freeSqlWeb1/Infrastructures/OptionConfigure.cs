using freeSqlWeb1.Infrastructures.SettingModels;
using freeSqlWeb1.Infrastructures.SettingOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace freeSqlWeb1.Infrastructures
{
    /// <summary>
    /// Option 配置
    /// </summary>
    public static class OptionConfigure
    {
        //public static IServiceProvider ServiceProvider;
        /// <summary>
        /// Configuration
        /// </summary>
        public static IConfiguration Configuration;
        /// <summary>
        /// Init
        /// </summary>
        /// <param name="_configuration"></param>
        /// <param name="services"></param>
        public static void Init(IConfiguration _configuration, IServiceCollection services)
        {
            Configuration = _configuration;
            SetOptionConfigure(services);
        }
        #region 服务注入方式使用options
        /// <summary>
        /// 服务注入的方式使用options
        /// </summary>
        /// <param name="services"></param>
        private static void SetOptionConfigure(IServiceCollection services)
        {
            //services.Configure<JwtOption>(Configuration.GetSection("JWT"));
            //services.Configure<RedisOption>(Configuration.GetSection("Redis"));
            //读取自定义文件,并可以使用注入方式读取
            CustomOptionConfigure(services);
        }
        #endregion

        #region 读取自定义json文件
        private static void CustomOptionConfigure(IServiceCollection services)
        {
            IConfiguration _configuration;
            var builder = new ConfigurationBuilder();
            //方式1
            //_configuration = builder.AddJsonFile("secret.json", false, true).Build();
            //方式2
            _configuration = builder.Add(new JsonConfigurationSource
            {
                Path = "Infrastructures/JsonFiles/secret.json",
                Optional = false,
                ReloadOnChange = true
            }).Build();
            //services.Configure<RedisOption>(_configuration.GetSection("Redis"));
        }
        #endregion

        #region CorsAllowUrl
        private static string _CorsAllowUrl = string.Empty;
        /// <summary>
        /// 链接白名单（可不做身份验证）
        /// </summary>
        public static List<string> CorsAllowUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_CorsAllowUrl))
                {
                    _CorsAllowUrl = Configuration["CorsAllowUrl"];
                }
                List<string> listUrls = new List<string>();
                if (!string.IsNullOrEmpty(_CorsAllowUrl))
                {
                    string[] urls = System.Text.RegularExpressions.Regex.Split(_CorsAllowUrl, ",");
                    if (urls.Length > 0)
                    {
                        foreach (string url in urls)
                        {
                            if (!listUrls.Contains(url))
                            {
                                listUrls.Add(url);
                            }
                        }
                    }

                }
                return listUrls;
            }
        }
        #endregion

        #region FilePath
        private static string _FilePath = string.Empty;
        /// <summary>
        /// 文件路径
        /// </summary>
        public static string FilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_FilePath))
                {
                    _FilePath = Configuration["CommonSettings:FilePath"];
                }
                return _FilePath;
            }
        }
        #endregion

        #region Redis
        private static string _IsOpenCache = string.Empty;
        /// <summary>
        /// 是否使用Redis
        /// </summary>
        public static bool IsOpenCache
        {
            get
            {
                if (string.IsNullOrEmpty(_IsOpenCache))
                {
                    _IsOpenCache = Configuration["Redis:IsOpenRedis"];
                }
                if (_IsOpenCache.ToLower() == "true")
                {
                    return true;
                }
                return false;
            }
        }

        private static string _RedisConnectionString = string.Empty;
        /// <summary>
        /// Redis默认连接串
        /// </summary>
        public static string RedisConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_RedisConnectionString))
                {
                    _RedisConnectionString = Configuration["Redis:ConnectionString"] ?? "127.0.0.1";
                }
                return _RedisConnectionString;
            }
        }

        private static string _RedisInstanceName = string.Empty;
        /// <summary>
        /// Redis 实例名称
        /// </summary>
        public static string RedisInstanceName
        {
            get
            {
                if (string.IsNullOrEmpty(_RedisInstanceName))
                {
                    _RedisInstanceName = Configuration["Redis:InstanceName"] ?? "api.redis";
                }
                return _RedisInstanceName;
            }
        }

        private static int _RedisDefaultExpireTime = 0;
        /// <summary>
        /// RedisDefaultExpireTime
        /// </summary>
        public static int RedisDefaultExpireTime
        {
            get
            {
                if (_RedisDefaultExpireTime == 0)
                {
                    var time = Configuration["Redis:DefaultExpireTime"];
                    _RedisDefaultExpireTime = time != null ? int.Parse(time) : 30;
                }
                return _RedisDefaultExpireTime;
            }
        }
        #endregion

        #region isLocalHost
        private static bool _IsLocalHost = true;
        /// <summary>
        /// 部署判断时使用
        /// </summary>
        public static bool IsLocalHost
        {
            get
            {
                bool.TryParse(Configuration["IsLocalHost"], out bool isLocalHost);
                return isLocalHost;
            }
        }
        #endregion

        #region webclinet config

        private static string _ncp_send_data = string.Empty;

        public static string Ncp_send_data
        {
            get { return Configuration["ClientUris:ncp_data"]; }
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using webRedisCache.Common;

namespace webRedisCache
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //service注入config
        private void OptionConfigure(IServiceCollection services)
        {
            //Redis 信息
            services.Configure<RedisHostOptions>(Configuration.GetSection("redis"));
        }

        // 读取自定义json文件
        private void CustomOptionConfigure(IServiceCollection services)
        {
            IConfiguration _configuration;
            var builder = new ConfigurationBuilder();
            //方式1
            //_configuration = builder.AddJsonFile("secret.json", false, true).Build();
            //方式2
            _configuration = builder.Add(new JsonConfigurationSource
            {
                Path = "JsonFile/secret.json",
                Optional = false,
                ReloadOnChange = true
            }).Build();
            services.Configure<RedisHostOptions>(_configuration.GetSection("redis"));
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            OptionConfigure(services);
            CustomOptionConfigure(services);
            services.AddEasyCaching(options =>
            {
                options.UseRedis(configure =>
                {
                    RedisHostOptions redisOption = JsonConfigHelper.GetAppSettings<RedisHostOptions>("JsonFile/secret.json", "redis");
                    configure.DBConfig.Endpoints.Add(new EasyCaching.Core.Configurations.ServerEndPoint(redisOption.host, redisOption.port));
                    configure.DBConfig.AllowAdmin = true;
                }, "RedisExample");
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

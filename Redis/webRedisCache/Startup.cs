using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

        //service×¢Èëconfig
        private void OptionConfigure(IServiceCollection services)
        {
            //Redis ÐÅÏ¢
            services.Configure<RedisHostOptions>(Configuration.GetSection("redis"));
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            OptionConfigure(services);
            services.AddEasyCaching(options =>
            {
                options.UseRedis(configure =>
                {
                    configure.DBConfig.Endpoints.Add(new EasyCaching.Core.Configurations.ServerEndPoint(Configuration["redis:host"], int.Parse(Configuration["redis:port"])));
                    configure.DBConfig.AllowAdmin = true;
                },"RedisExample");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapWeb1.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CapWeb1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCap(x =>
            {
                //x.UseInMemoryStorage();
                x.UseMySql(opt =>
                {
                    opt.TableNamePrefix = "cap";
                    opt.ConnectionString = "server= 192.168.0.198;port=3306;database=cap_test;user=root;password=mysql;Charset=utf8;";
                });
                x.UseRabbitMQ(opt =>
                {
                    opt.HostName = "localhost";
                    opt.UserName = "admin";
                    opt.Password = "admin";
                    opt.VirtualHost = "/";
                    opt.Port = 5672;
                    //opt.ExchangeName = "";
                    //opt.QueueMessageExpires = 60 * 60 * 24;
                    opt.ConnectionFactoryOptions = opt =>
                    {

                    };
                });
                x.UseDashboard(opt=>
                {
                    opt.Authorization = new[] { new TestAuthorizationFilter() };
                });
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

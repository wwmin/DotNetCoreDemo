using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorAppApi.Infrastructures.Configures;
using FreeSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlazorAppApi
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _log;
        public IConfiguration Configuration { get; }
        public IFreeSql FreeSql { get; }
        public Startup(IConfiguration configuration,IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
            FreeSql = new FreeSqlBuilder().UseConnectionString(DataType.MySql, Configuration.GetConnectionString("DatabaseMysql"))
                .UseAutoSyncStructure(true)
                .Build();
            if (_env.IsDevelopment())
            {
                var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddConsole()
                    .AddEventLog();
                });
                ILogger logger = loggerFactory.CreateLogger<Program>();
                _log = logger;
                
                FreeSql.Aop.CurdBefore += (s, e) =>
                {
                    logger.LogInformation(e.Sql);
                };
                FreeSql.Aop.CurdAfter += (s, e) =>
                {
                    logger.LogInformation(e.Sql);
                };
            }
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILogger>(_log);
            //×¢ÈëHttpContextAccessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IFreeSql>(FreeSql);
            services.AddControllers();
            services.UseSwagger();
            services.AddAutoMapper(builder => builder.AddProfile(new AutoMapperConfigure()));
            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(policyName:"Open");
            app.UseSwaggerUI();


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

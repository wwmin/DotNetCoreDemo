using AutoMapper;
using FreeSql;
using freeSqlWeb1.AutoMappers;
using freeSqlWeb1.Configures;
using freeSqlWeb1.Extensions;
using freeSqlWeb1.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace freeSqlWeb1
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _log;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
            Fsql = new FreeSqlBuilder()
                .UseConnectionString(DataType.MySql, Configuration.GetConnectionString("DatabaseMySql"))
                .UseAutoSyncStructure(true)
                .Build();
            if (_env.IsDevelopment())
            {
                var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder
                        .AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddConsole()
                        .AddEventLog();
                });
                ILogger logger = loggerFactory.CreateLogger<Program>();
                _log = logger;
                Fsql.Aop.CurdAfter += (s, e) =>
                {
                    logger.LogInformation(e.Sql);
                };
                Fsql.Aop.CurdAfter += (s, e) =>
                {
                    logger.LogInformation(s.ToString());
                };
            }
        }


        public IFreeSql Fsql { get; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Some Configure
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue;//重置文件上传的大小
                options.ValueLengthLimit = int.MaxValue;
                //options.MultipartHeadersLengthLimit = int.MaxValue;
            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = false; //=false为默认自动处理ModelState
            });
            #endregion
            #region use swagger
            services.UseSwagger();
            #endregion
            #region FreeSql
            services.AddSingleton<IFreeSql>(Fsql);
            #endregion
            #region Controller config
            services.AddControllers().AddNewtonsoftJson(options => options.UseCamelCasing(true)).AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter()));
            #endregion
            #region Add some service
            //services.AddScoped<IMessageService>
            services.AddSingleton<ILogger>(_log);
            services.AddMessage(builder => builder.UserEmail());
            #endregion
            #region AutoMapper
            services.AddAutoMapper(builder=>builder.AddProfile(new AutoMapperConfig()));
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthorization();
            #region file service
            var cachePeriod = env.IsDevelopment() ? "600" : "315360000";
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "upload")),
                RequestPath = "/upload",
                OnPrepareResponse = ctx =>
                {
                    //Requires the following import
                    //using Microsoft.AspNetCore.Http
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public,max-age={cachePeriod}");
                }
            });
            #endregion
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

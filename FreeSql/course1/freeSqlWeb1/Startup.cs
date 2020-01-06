using FreeSql;
using freeSqlWeb1.Configures;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace freeSqlWeb1
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
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
                Fsql.Aop.CurdBefore = (s, e) =>
                {
                    logger.LogInformation(e.Sql);
                };
                Fsql.Aop.CurdAfter = (s, e) =>
                {
                    //logger.LogInformation(s.ToString());
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
            services.AddSingleton<IFreeSql>(Fsql);
            services.AddControllers().AddNewtonsoftJson(options => options.UseCamelCasing(true)).AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter()));
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

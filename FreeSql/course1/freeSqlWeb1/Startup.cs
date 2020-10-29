using AspNetCoreRateLimit;
using AutoMapper;
using FreeSql;
using freeSqlWeb1.AutoMappers;
using freeSqlWeb1.Configures;
using freeSqlWeb1.Extensions;
using freeSqlWeb1.Infrastructures;
using freeSqlWeb1.Infrastructures.MiddleWares;
using freeSqlWeb1.Infrastructures.SettingModels;
using freeSqlWeb1.Infrastructures.SettingOptions;
using freeSqlWeb1.Infrastructures.Utils;
using freeSqlWeb1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            #region optionConfigure add init redis
            //needed to load configuration from appsettings.json
            services.AddOptions();
            OptionConfigure.Init(Configuration, services);
            #endregion
            #region Redis
            var redisOption = Configuration.GetSection("Redis").Get<RedisOption>();
            //services.AddSingleton(new RedisHelper(redisOption.ConnectionString, redisOption.InstanceName, redisOption.DefaultDatabase));
            RedisUtil.Init(redisOption);
            #endregion
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
            #region IPRateLimit
            // needed to store rate limit counters and ip rules
            services.AddMemoryCache();
            //load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            //load ip rules from appsettings.json
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
            // inject counter and rules stores
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            // https://github.com/aspnet/Hosting/issues/793
            // the IHttpContextAccessor service is not registered by default.
            // the clientId/clientIp resolvers use it.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            #endregion
            #region Jwt
            var jwtOptions = Configuration.GetSection("JWT").Get<JwtOption>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                var serverSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.ServerSecret));
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//是否验证Issuer
                    ValidateAudience = true,//是否验证Audience
                    ValidateLifetime = true,//是否验证失效时间
                    ClockSkew = TimeSpan.FromSeconds(30),
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    IssuerSigningKey = serverSecret,//
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        //if the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/posthub")))
                        {
                            //read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
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
            services.AddAutoMapper(builder => builder.AddProfile(new AutoMapperConfig()));
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var ms = Helpers.GetModels<BaseEntity>();
            //app.UseHttpsRedirection();
            app.UseMiddleware<RequestMiddleware>();
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

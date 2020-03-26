using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JwtDemo.Authorization.Jwt;
using JwtDemo.Authorization.Secret;
using JwtDemo.Configuration;
using JwtDemo.Handlers;
using JwtDemo.Handlers.AgePolicyHandler;
using JwtDemo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace JwtDemo
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
            services.AddHttpContextAccessor();
            services.AddTransient<IJwtAppService, JwtAppService>();
            services.AddTransient<ISecretService, SecretService>();
            services.AddDistributedRedisCache(options =>
            {
                //用于连接Redis的配置
                options.Configuration = "localhost:6379";
                //Redis实例名RedisDistributedCache
                options.InstanceName = "jwt:";
            });
            #region add store
            services.AddTransient<UserStore>();
            #endregion
            #region Swagger
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("jwt_test_api", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "jwt test api",
                    Description = "api for jwt test",
                    Contact = new OpenApiContact() { Name = "wwmin", Email = "wwei.min@163.com" }
                });
                option.SwaggerDoc("testApi", new OpenApiInfo
                {
                    Version = "test",
                    Title = "api test",
                    Description = "api test",
                    Contact = new OpenApiContact() { Name = "wwmin", Email = "wwei.min@163.com" }
                });


                option.IgnoreObsoleteActions();
                option.IgnoreObsoleteProperties();
                //option.DocumentFilter<HiddenApiFilter>();

                //自定义类型映射
                option.MapType<byte>(() => new OpenApiSchema { Type = "byte", Example = new OpenApiByte(0) });
                option.MapType<long>(() => new OpenApiSchema { Type = "long", Example = new OpenApiLong(0L) });
                option.MapType<int>(() => new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(0) });
                option.MapType<DateTime>(() => new OpenApiSchema { Type = "DateTime", Example = new OpenApiDateTime(DateTimeOffset.Now) });

                //xml注释
                foreach (var file in Directory.GetFiles(AppContext.BaseDirectory, "*.xml"))
                {
                    option.IncludeXmlComments(file, true);
                }
                //include document file 
                //option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Startup).Assembly.GetName().Name}.xml"), true);

                ////Authorization的设置
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "请输入验证的jwt。示例：Bearer {jwt}",
                    Name = "Authorization",

                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            #endregion
            #region auth
            services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));//将jwtOptions注入到service中
            var jwtOptions = Configuration.GetSection("Jwt").Get<JwtOptions>();
            services.AddAuthorization(options =>
            {
               
                //1.definition authorization policy
                options.AddPolicy("Permission", policy => policy.Requirements.Add(new PolicyRequirement()));
                options.AddPolicy(Permissions.UserCreate, policy => policy.AddRequirements(new PermissionAuthorizationRequirement(Permissions.UserCreate)));
                options.AddPolicy(Permissions.UserRead, policy => policy.AddRequirements(new PermissionAuthorizationRequirement(Permissions.UserRead)));
                options.AddPolicy(Permissions.UserUpdate, policy => policy.AddRequirements(new PermissionAuthorizationRequirement(Permissions.UserUpdate)));
                options.AddPolicy(Permissions.UserDelete, policy => policy.AddRequirements(new PermissionAuthorizationRequirement(Permissions.UserDelete)));
            }).AddAuthentication(options =>
            {
                //2.authentication
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //3. use jwt bearer
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,//是否验证失效时间
                    ClockSkew = TimeSpan.FromSeconds(1),//时间偏移量
                    ValidAudience = jwtOptions.Audience,
                    ValidIssuer = jwtOptions.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];//将header中token写入cotext.token

                        var path = context.HttpContext.Request.Path;//针对特殊路径
                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/hub")))
                        {
                            //Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        //token expired
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("adminOnly", policy => policy.RequireClaim("adminOnly"));
            //});
            //DI handler proces function
            services.AddSingleton<IAuthorizationHandler, PolicyHandler>();
            //策略模式
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            // Replace the default authorization policy provider with our own
            // custom provider which can return authorization policies for given
            // policy names (instead of using the default policy provider)
            services.AddSingleton<IAuthorizationPolicyProvider, AgePolicyProvider>();
            // As always, handlers must be provided for the requirements of the authorization policies
            services.AddSingleton<IAuthorizationHandler, AgeAuthorizationHandler>();
            #endregion
            #region Add Assembly
            string assemblies = Configuration["Assembly:InfrastructureAssembly"];
            if (!string.IsNullOrEmpty(assemblies))
            {
                foreach (var item in assemblies.Split("|"))
                {
                    Assembly assembly = Assembly.Load(item);
                    foreach (var implement in assembly.GetTypes())
                    {
                        Type[] interfaceType = implement.GetInterfaces();
                        foreach (var service in interfaceType)
                        {
                            services.AddTransient(service, implement);
                        }
                    }
                }
            }
            #endregion
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region use swagger ui
            //Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            //Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("./swagger/jwt_test_api/swagger.json", "jwt_test_api docs");//路径使用./方式防止使用二级域名导致路径错误问题
                option.SwaggerEndpoint("./swagger/testApi/swagger.json", "test");
                //option.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("index.html");
                option.RoutePrefix = string.Empty;
                option.DocumentTitle = "jwt test api";
                option.ConfigObject.DocExpansion = Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None;
            });
            #endregion
            app.UseRouting();

            app.UseAuthentication();//启用认证中间件
            app.UseAuthorization();//启用授权中间件
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

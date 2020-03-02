using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWeb1.Infrastructure.Configures
{
    /// <summary>
    /// 配置swagger
    /// </summary>
    public static class SwaggerConfigure
    {
        /// <summary>
        /// configure service
        /// </summary>
        /// <param name="services"></param>
        public static void UseSwagger(this IServiceCollection services)
        {
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            //});
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("SignalRWeb1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "SignalRWeb1 API",
                    Description = "api for SignalRWeb1",
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
                option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Startup).Assembly.GetName().Name}.xml"), true);

                ////Authorization的设置
                //option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    In = ParameterLocation.Header,
                //    Description = "请输入验证的jwt。示例：Bearer {jwt}",
                //    Name = "Authorization",

                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer",
                //    BearerFormat = "JWT"
                //});
                //option.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id="Bearer"
                //            }
                //        },
                //        new string[]{}
                //    }
                //});
            });
        }

        /// <summary>
        /// 使用swaggerUI
        /// </summary>
        /// <param name="app"></param>
        public static void UseMySwaggerUI(this IApplicationBuilder app)
        {
            //Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            //Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("./swagger/SignalRWeb1/swagger.json", "SignalRWeb1 Docs");//路径使用./方式防止使用二级域名导致路径错误问题
                option.SwaggerEndpoint("./swagger/testApi/swagger.json", "test test");
                //option.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("index.html");
                option.RoutePrefix = string.Empty;
                option.DocumentTitle = "SignalRWeb1 api";
                option.ConfigObject.DocExpansion = Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None;
            });
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Infrastructure
{
    /// <summary>
    /// 配置swagger
    /// </summary>
    public static class SwaggerConfigure
    {
        /// <summary>
        /// 配置swagger
        /// </summary>
        /// <param name="services"></param>
        public static void UseSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "File Upload Download Api",
                    Description = "api for file upload download",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "wwmin",
                        Email = "wwei.min@163.com"
                    }
                });
                option.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo { Version = "test", Title = "api test", Description = "api test", Contact = new Microsoft.OpenApi.Models.OpenApiContact() { Name = "wwmin", Email = "wwei.min@163.com" } });

                option.IgnoreObsoleteActions();//忽略果实的Action
                option.IgnoreObsoleteProperties();//忽略过时的属性

                //自定义类型映射
                //option.MapType<byte>(() => new OpenApiSchema { Type = "byte", Example = new OpenApiByte(0) });
                //option.MapType<long>(() => new OpenApiSchema { Type = "long", Example = new OpenApiLong(0L) });
                //option.MapType<int>(() => new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(0) });
                //option.MapType<DateTime>(() => new OpenApiSchema { Type = "DateTime", Example = new OpenApiDateTime(DateTimeOffset.Now) });

                //xml 注释
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
        public static void UseSwaggerUI(this IApplicationBuilder app)
        {
            //Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            //Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("./swagger/v1/swagger.json", "file upload download api docs");//路径使用./方式防止使用二级域名导致路径错误问题
                option.SwaggerEndpoint("./swagger/v2/swagger.json", "api test");
                //option.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("index.html");
                option.RoutePrefix = string.Empty;
                option.DocumentTitle = "file upload download api";
                option.ConfigObject.DocExpansion = Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None;
            });
        }
    }
}

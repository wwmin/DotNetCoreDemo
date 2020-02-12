using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace OcelotBasic
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
            #region IdentityServerService

            var authenticationProviderKey = "OcelotKey";
            var identityServerOptions = new IdentityServerOptions();
            Configuration.Bind("IdentityServerOptions", identityServerOptions);
            services.AddAuthentication(identityServerOptions.IdentityScheme)
                .AddIdentityServerAuthentication(authenticationProviderKey, options =>
                    {
                        options.RequireHttpsMetadata = false;//�Ƿ�����https
                        options.Authority = $"http://{identityServerOptions.ServerIP}:{identityServerOptions.ServerPort}";//������Ȩ��֤�ĵ�ַ
                        options.ApiName = identityServerOptions.ResourceName;//��Դ����,����֤������ע�����Դ�б������е�apiResourceһ��
                        options.SupportedTokens = SupportedTokens.Both;
                    });
            #endregion
            services.AddOcelot(new ConfigurationBuilder().AddJsonFile("ocelot.json", optional: true, reloadOnChange: true).Build()).AddConsul();
            //services.AddOcelot();//ע��Ocelot����
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            await app.UseOcelot();//ʹ��ocelot�м��
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

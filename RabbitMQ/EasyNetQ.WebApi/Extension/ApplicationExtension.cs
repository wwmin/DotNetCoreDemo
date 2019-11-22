using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EasyNetQ.WebApi.Extension
{
    public static class ApplicationExtension
    {
        public static IApplicationBuilder UseSubscribe(this IApplicationBuilder applicationBuilder,string subscriptionIdPrefix,
            Assembly assembly)
        {
            var services = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider;
            var lifeTime = services.GetService<IApplicationLifetime>();
            var bus = services.GetService<IBus>();
            lifeTime.ApplicationStarted.Register(() =>
            {
                var subscriber = new AutoSubscriber(bus, subscriptionIdPrefix);
                subscriber.Subscribe(assembly);
                subscriber.SubscribeAsync(assembly);
            });

            lifeTime.ApplicationStopped.Register(() => { bus.Dispose(); });

            return applicationBuilder;
        }
    }
}

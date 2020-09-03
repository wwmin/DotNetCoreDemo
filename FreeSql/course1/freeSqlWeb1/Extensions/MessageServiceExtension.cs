using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace freeSqlWeb1.Extensions
{
    public static class MessageServiceExtension
    {
        public static void AddMessage(this IServiceCollection services, Action<MessageServiceBuilder> config)
        {
            var builder = new MessageServiceBuilder(services);
            config(builder);
        }
    }
}

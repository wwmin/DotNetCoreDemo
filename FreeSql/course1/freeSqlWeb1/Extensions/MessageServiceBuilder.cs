using freeSqlWeb1.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace freeSqlWeb1.Extensions
{
    public class MessageServiceBuilder
    {
        public IServiceCollection serviceCollection { get; set; }

        public MessageServiceBuilder(IServiceCollection services)
        {
            this.serviceCollection = services;
        }

        public void UserEmail()
        {
            serviceCollection.AddSingleton<IMessageService, EmailService>();
        }

        public void UseSms()
        {
            serviceCollection.AddSingleton<IMessageService, SmsService>();
        }
    }
}

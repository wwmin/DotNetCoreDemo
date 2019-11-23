using EasyNetQ.AutoSubscribe;
using EasyNetQ.WebApi.Extension;
using EasyNetQ.WebApi.MQModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyNetQ.WebApi.Services
{
    public class OrderConsumer : IConsumeAsync<OrderMessage>
    {
        [AutoSubscriberConsumer(SubscriptionId = "TestOrderService")]
        public Task ConsumeAsync(OrderMessage message)
        {

            return Task.Run(() =>
            {
                string msg = "主线程中执行: " + "text: " + message.text + ",id: " + message.id;
                
                
                Console.WriteLine(msg);
            });
        }
    }
}

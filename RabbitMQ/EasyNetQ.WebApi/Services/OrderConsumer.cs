using EasyNetQ.AutoSubscribe;
using EasyNetQ.WebApi.MQModels;
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
            Console.WriteLine("主线程中执行:" + message);
            return Task.Run(() =>
            {
                Console.WriteLine(message);                
            });
        }
    }
}

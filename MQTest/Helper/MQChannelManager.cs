using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTest.Helper
{
    public class MQChannelManager
    {
        public MQConnection MQConn { get; set; }
        public MQChannelManager(MQConnection conn)
        {
            this.MQConn = conn;
        }

        /// <summary>
        /// 创建消息通道
        /// </summary>
        /// <param name="exchangeType"></param>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="routekey"></param>
        /// <returns></returns>
        public MQChannel CreateReceiveChannel(string exchangeType,string exchange,string queue,string routekey)
        {
            IModel model = this.CreateModel(exchangeType, exchange, queue, routekey);
            model.BasicQos(0, 1, false);
            EventingBasicConsumer consumer = this.CreateConsumer(model, queue);
            MQChannel channel = new MQChannel(exchangeType, exchange, queue, routekey)
            {
                Connection = this.MQConn.Connection,
                Consumer = consumer
            };
            consumer.Received += channel.Receive;
            return channel;
        }

        /// <summary>
        /// 拆功能键一个通道,包含交换机/队列/路由,并建立绑定关系
        /// </summary>
        /// <param name="type">交换机类型</param>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名称</param>
        /// <param name="routeKey">路由名称</param>
        /// <param name="arguments"></param>
        /// <returns>IModel</returns>
        private IModel CreateModel(string type,string exchange,string queue,string routeKey,IDictionary<string,object> arguments = null)
        {
            type = string.IsNullOrEmpty(type) ? "default" : type;
            IModel model = this.MQConn.Connection.CreateModel();
            model.ExchangeDeclare(exchange, type, false, false, null);
            model.BasicQos(0, 1, false);
            model.QueueDeclare(queue, false, false, false, arguments);
            model.QueueBind(queue, exchange, routeKey);
            return model;
        }

        /// <summary>
        /// 接收消息到队列中
        /// </summary>
        /// <param name="model">消息通道</param>
        /// <param name="queue">队列名称</param>
        /// <returns>EventingBasicConsumer</returns>
        private EventingBasicConsumer CreateConsumer(IModel model,string queue)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(model);
            model.BasicConsume(queue, false, consumer);
            return consumer;
        }
    }
}

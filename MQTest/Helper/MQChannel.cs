using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTest.Helper
{
    public class MQChannel
    {
        public string ExchangeTypeName { get; set; }
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public string RouteKeyName { get; set; }
        public IConnection Connection { get; set; }
        public EventingBasicConsumer Consumer { get; set; }

        /// <summary>
        /// 外部订阅消费者通知委托
        /// </summary>
        public Action<MessageBody> OnReceivedCallBack { get; set; }
        public MQChannel(string exchangeType, string exchange, string queue, string routekey)
        {
            this.ExchangeTypeName = exchangeType;
            this.ExchangeName = exchange;
            this.QueueName = queue;
            this.RouteKeyName = routekey;
        }

        public void Publish(string content)
        {
            byte[] body = MQConnection.UTF8.GetBytes(content);
            IBasicProperties prop = new BasicProperties();
            prop.DeliveryMode = 1;
            Consumer.Model.BasicPublish("", "hello", false, prop, body);
        }

        internal void Receive(object sender, BasicDeliverEventArgs e)
        {
            MessageBody body = new MessageBody();
            try
            {
                string content = MQConnection.UTF8.GetString(e.Body);
                body.Content = content;
                body.Consumer = (EventingBasicConsumer)sender;
                body.BasicDeliver = e;
            }
            catch (Exception ex)
            {
                body.ErrorMessage = $"订阅-出错{ex.Message}";
                body.Exception = ex;
                body.Error = true;
                body.Code = 500;
            }
            OnReceivedCallBack?.Invoke(body);
        }

        public void SetBasicAck(EventingBasicConsumer consumer,ulong deliveryTag,bool multiple)
        {
            consumer.Model.BasicAck(deliveryTag, multiple);
        }

        public void Stop()
        {
            if(this.Connection != null && this.Connection.IsOpen)
            {
                this.Connection.Close();
                this.Connection.Dispose();
            }
        }
    }
}

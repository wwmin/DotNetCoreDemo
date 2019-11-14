using MQTest.Helper;
using MQTest.Utils;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTest.Service
{
    public class DemoService : MQServiceBase
    {
        public Action<MessageLevel, string, Exception> OnAction = null;
        public DemoService(MQConfig config) : base(config)
        {
            base.Queue.Add(new QueueInfo()
            {
                ExchangeType = ExchangeType.Direct,
                Queue = "hello",
                RouterKey = "",
                OnReceived = this.OnReceived
            });
        }

        public override string vHost { get { return "/"; } }

        public override string Exchange => "";


        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="message"></param>
        public override void OnReceived(MessageBody message)
        {
            try
            {
                Console.WriteLine(message.Content);
            }
            catch (Exception ex)
            {
                OnAction?.Invoke(MessageLevel.Error, ex.Message, ex);
            }
            message.Consumer.Model.BasicAck(message.BasicDeliver.DeliveryTag, true);
        }
    }
}

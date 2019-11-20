using MQUtil;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Receiver.Service
{
    public class ReceiveService : MQServiceBase
    {
        public Action<MessageLevel, string, Exception> OnAction = null;
        public ReceiveService(MQConfig config) : base(config)
        {
            base.Queue.Add(new QueueInfo()
            {
                ExchangeType = ExchangeType.Direct,
                Queue = "hello",
                RouterKey = "#",
                OnReceived = this.OnReceived
            });
        }
        public override string vHost => "/";

        public override string Exchange => "save.#";

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

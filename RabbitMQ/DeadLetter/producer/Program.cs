using RabbitMQ.Client;
using System;

namespace producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello RabbitMQ!");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                }
            }
        }
    }
}

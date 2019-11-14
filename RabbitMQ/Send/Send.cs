using RabbitMQ.Client;
using System;
using System.Text;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Password = "admin",
                Port = 5672,
                UserName = "admin",
                AutomaticRecoveryEnabled = true
            };
            using (var connection = factory.CreateConnection())//建立到代理服务器的连接
            {
                using (var channel = connection.CreateModel())//获得信道
                {
                    channel.QueueDeclare(queue: "hello",//
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    var message = GetMessage(args);
                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "",
                        routingKey: "hello",
                        basicProperties: null,
                        body: body);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" [x] Send {0}", message);
                    Console.ResetColor();
                }
            }
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
    }
}

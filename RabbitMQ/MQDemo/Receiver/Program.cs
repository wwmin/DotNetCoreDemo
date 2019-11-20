using MQUtil;
using Receiver.Service;
using System;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Receive();
        }

        static void Receive()
        {
            MQConfig config = new MQConfig()
            {
                HostName = "localhost",
                Password = "admin",
                UserName = "admin",
                Port = 5672
            };

            MQServiceManager manager = new MQServiceManager();
            manager.AddService(new ReceiveService(config));
            manager.OnAction = OnActionOutput;
            manager.Start();

            Console.WriteLine("服务已启动");
            Console.ReadKey();

            manager.Stop();
            Console.WriteLine("服务已停止,按任意键退出...");
            Console.ReadKey();
        }

        static void OnActionOutput(MessageLevel level,string message,Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0} | {1} | {2}",level,message,ex?.StackTrace);
            Console.ResetColor();
        }
    }
}

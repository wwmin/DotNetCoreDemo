using MQTest.Helper;
using MQTest.Service;
using MQTest.Utils;
using System;

namespace MQTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Test();
        }

        static void Test()
        {
            MQConfig config = new MQConfig()
            {
                HostName = "localhost",
                Password = "admin",
                Port = 5672,
                UserName = "admin"
            };

            MQServiceManager manager = new MQServiceManager();
            manager.AddService(new DemoService(config));
            manager.OnAction = OnActionOutput;
            manager.Start();

            Console.WriteLine("服务已启动");
            Console.ReadKey();

            manager.Stop();
            Console.WriteLine("服务已停止,按任意键退出...");
            Console.ReadKey();
        }

        static void OnActionOutput(MessageLevel level, string message, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0} | {1} | {2}", level, message, ex?.StackTrace);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}

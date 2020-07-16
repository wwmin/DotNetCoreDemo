using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace publish
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world");

            //消息队列
            Thread.Sleep(1000);
            var mq = MSMQHelper.CreateQueue();
            MSMQHelper.SendQueue(mq, "hello,world,from publish");
            Console.Read();
        }
    }
}

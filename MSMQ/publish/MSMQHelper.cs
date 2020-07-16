using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace publish
{
    public class MSMQHelper
    {
        public static MessageQueue CreateQueue()
        {
            string path = ".\\Private$\\queue";
            MessageQueue queue;
            //如果存在指定路径的消息队列
            if (MessageQueue.Exists(path))
            {
                queue = new MessageQueue(path);
            }
            else
            {
                queue = MessageQueue.Create(path);
            }
            return queue;
        }

        public static void SendQueue(MessageQueue queue, string mqmsg)
        {
            Message msg = new Message();
            //内容
            msg.Body = mqmsg;
            //指定格式化程序
            msg.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            queue.Send(msg);
        }

        public static string ImportQueue(MessageQueue queue)
        {
            //接收到的消息对象
            Message msg = queue.Receive();
            //指定格式化程序
            msg.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            //接收到的内容
            string str = msg.Body.ToString();
            return str;
        }
    }
}

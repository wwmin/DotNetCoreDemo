using EasyNetQ;
using System;

namespace Messages
{
    [Queue("TestMessageQueue", ExchangeName = "MyTestExchange")]
    public class TextMessage
    {
        public string Text { get; set; }
    }
}

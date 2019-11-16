using System;
using System.Collections.Generic;
using System.Text;

namespace aop1
{
    public class EmailMessage : IMessage
    {
        public void Receive(string content)
        {
            Console.WriteLine($"Send Email: {content}");
        }

        public void Send(string content)
        {
            Console.WriteLine($"Receive Email:{content}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace aop1
{
    public interface IMessage
    {
        void Send(string content);
        void Receive(string content);
    }
}

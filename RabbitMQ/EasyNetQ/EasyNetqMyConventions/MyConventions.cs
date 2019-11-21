using EasyNetQ;
using Messages;
using System;

namespace EasyNetqMyConventions
{
    public class MyConventions : Conventions
    {
        public MyConventions(ITypeNameSerializer typeNameSerializer) : base(typeNameSerializer)
        {
            ErrorQueueNamingConvention = messageInfo => "MyErrorQueue";
        }
    }
}

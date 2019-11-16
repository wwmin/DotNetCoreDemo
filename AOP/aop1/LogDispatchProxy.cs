using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace aop1
{
    public class LogDispatchProxy : DispatchProxy
    {
        public object TargetClass { get; set; }
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            Write("方法执行前");
            var result = targetMethod.Invoke(TargetClass, args);
            Write("方法执行后");
            return result;
        }

        private void Write(string content)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(content);
            Console.ResetColor();
        }
    }
}

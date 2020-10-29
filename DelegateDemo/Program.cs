using System;

namespace DelegateDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            DeleageDemoMyDelegate delegateDemo = new DeleageDemoMyDelegate();
            {
                SayHiDelegate method = new SayHiDelegate(delegateDemo.SayHiChinese);
                method += delegateDemo.SayHiEnglish;
                delegateDemo.SayHiShow("wwmin", method);
            }

            Action action1 = new Action(() => Console.WriteLine("www"));
            action1.Invoke();

            Func<string> func1 = new Func<string>(() => "wwmin");
            func1 += () => "liyue";
            Console.WriteLine(func1.Invoke());

            Console.ReadKey();
        }
    }
}

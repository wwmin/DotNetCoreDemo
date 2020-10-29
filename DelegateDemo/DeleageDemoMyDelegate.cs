using System;
using System.Collections.Generic;
using System.Text;

namespace DelegateDemo
{
    public delegate void SayHiDelegate(string name);
    public class DeleageDemoMyDelegate
    {
        public void Show()
        {

        }

        public void SayHiShow(string name, SayHiDelegate method)
        {
            Console.WriteLine("需要先举手");
            method.Invoke(name);
        }

        public void SayHiChinese(string name)
        {
            Console.WriteLine($"{name}: 晚上好");
        }

        public void SayHiMalasia(string name)
        {
            Console.WriteLine($"{name}: Malasia");
        }

        public void SayHiEnglish(string name)
        {
            Console.WriteLine($"{name}: Hello!");
        }
    }
}

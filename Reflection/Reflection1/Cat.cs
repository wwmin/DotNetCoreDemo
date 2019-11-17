using System;
using System.Collections.Generic;
using System.Text;

namespace Reflection1
{
    public class Cat : Animal
    {
        public Cat()
        {
            base.Name = "tom";
            base.Age = 2;
        }
        public override void Shout()
        {
            Console.WriteLine($"喵喵,我是{base.Name},今年{base.Age}");
        }
    }
}

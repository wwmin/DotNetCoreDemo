using System;
using System.Collections.Generic;
using System.Text;

namespace Reflection1
{
    public class Dog : Animal
    {
        public Dog()
        {
            base.Name = "bluce";
            base.Age = 3;
        }
        public override void Shout()
        {
            Console.WriteLine($"旺旺,我是{base.Name},今年{base.Age}岁");
        }
    }
}

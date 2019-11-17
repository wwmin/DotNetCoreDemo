using System;
using System.Collections.Generic;
using System.Text;

namespace Reflection1
{
    public abstract class Animal
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public abstract void Shout();
    }
}

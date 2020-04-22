using System;
using System.Collections.Generic;
using System.Text;

namespace Delegate1
{
    public class Person
    {
        private string Address;
        public Person(string address)
        {
            Address = address;
        }
        public event EventHandler<FallsIllEventArgs> FallsIll;

        public void OnFallsIll()
        {
            FallsIll?.Invoke(this, new FallsIllEventArgs(Address));
        }
    }


    public class FallsIllEventArgs : EventArgs
    {
        public readonly string Address;
        public FallsIllEventArgs(string address)
        {
            this.Address = address;
        }

    }
}

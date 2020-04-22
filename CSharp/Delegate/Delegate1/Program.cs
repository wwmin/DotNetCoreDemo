using System;

namespace Delegate1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //Transformer t = delegate_square;
            //t += delegate_cube;
            //Console.WriteLine(t(3));

            #region EventHandler
            var person = new Person("天津");
            person.FallsIll += PersonFallsIll;
            person.OnFallsIll();
            Console.ReadKey();
            #endregion
        }

        private static void PersonFallsIll(object sender, FallsIllEventArgs eventArgs)
        {
            Console.WriteLine($"A docker has been called to {eventArgs.Address}");
        }
        #region delegate
        delegate int Transformer(int x);

        static int delegate_square(int x)
        {
            return x * x;
        }

        static int delegate_cube(int x)
        {
            return x * x * x;
        }
        #endregion

    }

    //public delegate void SomeChangedhandler(decimal x);

    //public class Broadcaster
    //{
    //    public event SomeChangedhandler handler;
    //}




    //public event EventHandler<FallsIllEventArgs> FallsIll;
    //public void OnFallsIll()
    //{
    //    FallsIll?.Invoke(this, new FallsIllEventArgs("china tianjin"));
    //}
}

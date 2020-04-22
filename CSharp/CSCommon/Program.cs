using CSCommon.Dynamic;
using System;

namespace CSCommon
{
    class Program
    {
        static dynamic Mean(dynamic x, dynamic y) => (x + y) / 2;
        static void Main(string[] args)
        {
            #region dynamic语法
            dynamic d = new Duck();
            d.Quack();
            d.Waddle();

            int x = 3, y = 4;
            Console.WriteLine(Mean(x, y));

            dynamic dd = 5;
            dd.Hello();
            #endregion

            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tuple1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Tuple!");

            //var bob = ("Bob", 23);
            //Console.WriteLine(bob.Item1);
            //Console.WriteLine(bob.Item2);

            //var joe = bob;//joe is a *copy* of job
            //joe.Item1 = "Joe";
            //Console.WriteLine(bob);
            //Console.WriteLine(joe);

            //(string Name, int Age) bob = ("Bob", 23);
            //var p = GetPerson();
            //Console.WriteLine(p.Item1);
            //Console.WriteLine(p.Item2);

            //Tuple 可泛型
            //Task<(string, int)> a;
            //Dictionary<(string, int), Uri> b;
            //IEnumerable<(int id, string name)> c;


            //为Tuple元素命名
            //var tuple = (Name: "Bob", Age: 23);
            //Console.WriteLine(tuple.Name);
            //Console.WriteLine(tuple.Age);
            //var p = GetPerson2();
            //Console.WriteLine(p.Name);
            //Console.WriteLine(p.Age);

            //类型和顺序相同 则两个tuple兼容
            //(string Name, int Age, Char Sex) bob1 = ("Bob", 23, 'M');
            //(string Age, int Sex, Char Name) bob2 = bob1;

            //ValueTuple创建Tuple,此时不可以指定元素名称
            //ValueTuple<string, int> bob1 = ValueTuple.Create("Bob", 23);
            //(string, int) bob2 = ValueTuple.Create("Bob", 23);

            //Deconstructing Tuples
            //Tuple 隐式的支持Deconstruction模式(反构建模式)
            //var bob = (Name: "Bob", Age: 23);
            //string name = bob.Name;
            //int age = bob.Age;
            //var bob = (Name: "Bob", Age: 23);
            //(string name, int age) = bob;
            //Console.WriteLine(name);
            //Console.WriteLine(age);
            //相似但不同
            //var bob = (Name: "Bob", Age: 23);
            //(string name, int age) bob2 = bob;//声明一个tuple
            //(string name, int age) = bob;//Deconstructing a tuple

            //相等性比较
            //ValueTuple<> 也重写了Equals方法,让比较更有意义
            var t1 = ("one", 1);
            var t2 = ("one", 1);
            Console.WriteLine(t1.Equals(t2)); //true

            Console.ReadKey();
        }
        //Tuple 作为返回类型
        static (string, int) GetPerson() => ("Bob", 23);

        static (string Name, int Age) GetPerson2() => ("Bob", 23);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Equal
{
    public class EqualTest
    {
        public static void Test()
        {
            Console.WriteLine("=============class==============");
            //Console.WriteLine("Hello wwmin!");
            var my1 = new MyClass("wwmin");
            var my2 = new MyClass("wwmin");

            Console.WriteLine(my1 == my2);
            Console.WriteLine(my1.Equals(my2));
            Console.WriteLine(Equals(my1, null));
            if (my1 == my2)
            {
                Console.WriteLine("my1==my2");
            }
            Console.WriteLine("==============deconstruct object============");
            var user = new User("11", "32")
            ;
            (var name, var email) = user;
            Console.WriteLine($"name:{name}\nemail:{email}");
            Console.WriteLine("===============string=============");
            var str1 = "wwmin";
            var str2 = string.Copy(str1);
            Console.WriteLine(str1.Equals(str2));
            Console.WriteLine(str1.Equals((object)str2));
            Console.WriteLine(object.ReferenceEquals(str1, str2));
            object str3 = string.Copy((string)str1);
            Console.WriteLine((object)str1 == str3);

            Console.WriteLine("ByEqualOperator<string>(str1, str2) = " + ByEqualOperator<string>(str1, str2));
            Console.WriteLine("===============struct equals====================");
            var struct1 = new MyStruct { Name = "wwmin" };
            var struct2 = new MyStruct { Name = "wwmin" };
            var struct3 = new MyStruct { Name = "Nick" };

            Console.WriteLine(struct1.Equals(struct2));
            Console.WriteLine(struct1.Equals(struct3));

            Console.WriteLine("=================int==================");
            int num2 = 2;
            int num2too = 2;
            int num3 = 3;
            Console.WriteLine(num2.Equals(num2too));
            Console.WriteLine("================tuple==============");
            var tp1 = Tuple.Create<int, int>(2, 3);
            var tp2 = Tuple.Create<int, int>(2, 3);
            Console.WriteLine(" ReferenceEquals(tp1, tp2)=" + ReferenceEquals(tp1, tp2));
            Console.WriteLine("tp1.Equlas(tp2)=" + tp1.Equals(tp2));
            Console.WriteLine(value: tp1 == tp2);
            var studentValueTuple1 = GetStudent();
            var studnetValueTuple2 = GetStudent();
            Console.WriteLine(studentValueTuple1.Equals(studnetValueTuple2));
            Console.WriteLine("=================test thread and valueTuple===========");
            RunnTest();
        }
        public static bool ByEqualOperator<T>(T a, T b) where T : class
        {
            return a == b;
        }

        public static (string name, int age, uint height) GetStudent()
        {
            return ("wwmin", 30, 170);
        }

        public static void writeStudentInfo(Object student)
        {
            var studentInfo = (ValueTuple<string, int, uint>)student;
            Console.WriteLine($"Student Information: Name [{studentInfo.Item1}], Age [{studentInfo.Item2}], Height [{studentInfo.Item3}]");
            var (name, age, height) = GetStudent();//解构赋值
            Console.WriteLine($"Student Information: Name [{name}], Age [{age}], Height [{height}]");
        }

        public static void RunnTest()
        {
            var t = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(writeStudentInfo));
            t.Start(new ValueTuple<string, int, uint>("wwmin", 20, 170));
            while (t.IsAlive)
            {
                System.Threading.Thread.Sleep(2000);
            }
            Console.WriteLine("thread is exit.");
        }

    }

    public class MyClass
    {
        public MyClass(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public void Deconstruct(out string name)
        {
            name = Name;
        }
    }

    public class User
    {
        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        //解构函数
        public void Deconstruct(out string name, out string email)
        {
            name = Name;
            email = Email;
        }
    }

    public struct MyStruct
    {
        public string Name { get; set; }
    }

}

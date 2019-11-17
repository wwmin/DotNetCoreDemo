using System;
using System.Reflection;

namespace Reflection1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine("请录入动物类型:");
            string type = Console.ReadLine().Trim();

            Animal a = null;
            switch (type)
            {
                case "cat":
                    a = new Cat();
                    a.Shout();
                    break;
                case "dog":
                    a = new Dog();
                    a.Shout();
                    break;
                default:
                    break;
            }
            string v = "abc";
            Type t = v.GetType();
            Type t2 = Type.GetType("System.string", false, true);
            var t3 = typeof(string);
            Console.WriteLine($"v.GetType():{t},Type.GetType:" +
                $"{t2},typeof(string):{t3}");

            Console.WriteLine("class reflect");
            Type tc = typeof(Cat);
            Console.WriteLine(tc.Name);

            Console.WriteLine("assembly");
            Assembly assembly1 = Assembly.Load("Reflection1");
            Assembly assembly2 = Assembly.LoadFrom(@"C:\work\demo\TestDemo\Reflection\Reflection1\bin\Debug\netcoreapp3.0\Reflection1.dll");

            Console.WriteLine($"assembly1:{assembly1.FullName}");
            Console.WriteLine($"assembly2:{assembly2.FullName}");
            Type[] ats = assembly1.GetTypes();
            foreach (Type definedType in ats)
            {
                Console.WriteLine(definedType.Name);
            }

            Console.WriteLine("====使用反射实现动态调用====");
            Console.WriteLine("请录入动物类型:"); 
            type = Console.ReadLine().Trim();
            //创建程序集对象,静态加载程序集 前提:需要先添加对加载程序集的引用
            Assembly ass = Assembly.Load("Reflection1");
            // 获取程序集中的类型(在这里指的就是Reflection1里的类, 即:Cat,Dog,....)
            Type[] types = ass.GetTypes();
            foreach (Type definedType in types)
            {
                // t.Name 表示类名(即 Cat,Dog,Pig,Bird)
                if (type == definedType.Name.ToLower())
                {
                    //找到Shout方法
                    MethodInfo m = definedType.GetMethod("Shout");
                    //创建对象
                    object o = Activator.CreateInstance(definedType);//对应于: new Cat()或new ...
                    //找属性
                    PropertyInfo[] para = definedType.GetProperties();
                    //遍历属性
                    foreach (PropertyInfo p in para)
                    {
                        //输出属性的名字 即:Name和Age
                        if (p.Name == "Name")
                        {
                            //给属性赋值
                            p.SetValue(o, "wwmin", null);
                        }
                        if (p.Name == "Age")
                        {
                            // 获取o对象的属性为p的属性值并加10
                            int age = Convert.ToInt32(p.GetValue(o)) + 10;
                            p.SetValue(o, age, null);
                        }
                    }
                    //调用方法
                    m.Invoke(o, null);//对应于cat.Shout();
                }
            }
            Console.ReadKey();
        }
    }
}

using System;
using System.Reflection;

namespace aop1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // 使用DispatchProxy 类的静态方法Create生成代理类,其中Create是个泛型方法
            // 该泛型方法有两个值,第一个值必须是接口,第二个必须是DispatchProxy的子类
            IMessage messageDispatchProxy = DispatchProxy.Create<IMessage, LogDispatchProxy>();
            // 创建一个实现了IMessage 接口的类的实例,并赋值给代理类的TargetClass属性
            ((LogDispatchProxy)messageDispatchProxy).TargetClass = new EmailMessage();
            messageDispatchProxy.Send("早上好");
            Console.WriteLine("=======================================");
            messageDispatchProxy.Receive("中午好");
            Console.ReadKey();

            /*
             通过DispatchProxy.Create创建的代理类messageDispatchProxy就是一个LogDispatchProxy类,并且利用我们提供的实例
             实现了IMessage接口,所以messageDispatchProxy可以强转为LogDispatchProxy或IMessage
             至此,我们没有通过任何第三方类库,自己实现了一个AOP
             */
        }
    }
}

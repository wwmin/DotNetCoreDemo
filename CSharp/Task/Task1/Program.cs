using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World! Task!");
            Console.WriteLine("参考:https://www.cnblogs.com/scmail81/p/9508448.html");

            //Test1();
            //Test2();
            //Test3();
            Test4();
        }

        #region Test1
        /// <summary>
        /// Task简单任务示例
        /// </summary>
        static void Test1()
        {
            Task t = new Task(() =>
            {
                Console.WriteLine("任务开始工作...");
                //模拟工作过程
                Thread.Sleep(2000);
            });
            t.Start();
            t.ContinueWith(task =>
            {
                Console.WriteLine("任务完成,完成时候的状态为:");
                Console.WriteLine("IsCanceled={0}\t\nIsCompleted={1}\t\nIsFaulted={2}", task.IsCanceled, task.IsCompleted, task.IsFaulted);
            });
            Console.ReadKey();
        }
        #endregion

        #region Test2
        /// <summary>
        /// Task的几种用法
        /// </summary>
        static void Test2()
        {
            #region 使用方式1
            Console.WriteLine("\n使用方式1");
            var t1 = new Task(() => TaskMethod("Task 1"));
            var t2 = new Task(() => TaskMethod("Task 2"));
            t2.Start();
            t1.Start();
            Task.WaitAll(t1, t2);
            #endregion

            #region 使用方式2
            Console.WriteLine("\n使用方式2");
            Task.Run(() => TaskMethod("Task 3"));
            Task.Run(() => TaskMethod("Task 4")).Wait();
            Task.Run(() => TaskMethod("Task 5")); //注意上述三个的执行位置
            #endregion

            #region 使用方式3
            Console.WriteLine("\n使用方式3");
            Task.Factory.StartNew(() => TaskMethod("Task 6"));
            Task.Factory.StartNew(() => TaskMethod("Task 7"), TaskCreationOptions.LongRunning);  //标记为长事件运行任务,则任务不会使用线程池,而在单独的线程中运行
            #endregion

            #region 使用常规方式
            Console.WriteLine("\n使用常规方式");
            Console.WriteLine("主线程正处理业务");
            Thread.Sleep(2000);
            Task task = new Task(() =>
           {
               Console.WriteLine("使用System.Threading.Tasks.Task执行异步操作.");
               for (int i = 0; i < 10; i++)
               {
                   Console.WriteLine(i);
               }
           });
            task.Start();//启动任务,并安排到当前任务队列线程中执行任务(System.Threading.Tasks.TaskScheduler)
            Console.WriteLine("主线程执行其他任务");
            task.Wait();
            #endregion

            Console.WriteLine("主线程执行结束");
            Console.ReadKey();
        }

        static void TaskMethod(string name)
        {
            Console.WriteLine("Task {0} is running on a thread id {1}. Is thread pool thread: {2}",
               name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
        }
        #endregion

        #region Test3
        /// <summary>
        /// async/await的实现方式
        /// </summary>
        static void Test3()
        {
            Console.WriteLine("主线程执行业务处理");
            AsyncFunction();
            Console.WriteLine("主线程执行其他任务");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(string.Format("Main:i={0}", i));
            }
            Console.ReadKey();
        }
        async static void AsyncFunction()
        {
            await Task.Delay(1000);//将延迟执行时间改为0,看一下执行效果: Main线程反而后执行
            Console.WriteLine("使用System.Threading.Tasks.Task执行异步操作");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(string.Format("AsyncFunction:i={0}", i));
            }
        }
        #endregion

        #region Test4
        /// <summary>
        /// 带返回值的方式
        /// </summary>
        static void Test4()
        {
            TaskMethod2("Main Thread Task");
            Task<int> task = CreateTask("Task 1");
            task.Start();
            int result = task.Result;
            Console.WriteLine("Task 1 Result is:{0}", result);

            task = CreateTask("Task 2");
            task.RunSynchronously();//该任务会运行在主线程中
            result = task.Result;
            Console.WriteLine("Task 2 Result is:{0}", result);

            task = CreateTask("Task 3");
            Console.WriteLine("Task 3 现在正状态:" + task.Status);
            task.Start();

            while (!task.IsCompleted)
            {
                Console.WriteLine("Task 3 现在正状态:" + task.Status);
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }

            Console.WriteLine("Task 3 现在正状态:" + task.Status);
            result = task.Result;
            Console.WriteLine("Task 3 Result is:" + result);

            #region 常规使用方式
            Task<int> getSumTask = new Task<int>(() => GetSum());//创建任务
            getSumTask.Start();
            Console.WriteLine("主线程执行其他处理");
            getSumTask.Wait();//等待任务的完成执行过程
            Console.WriteLine("任务执行结果：{0}", getSumTask.Result.ToString());
            #endregion

            Console.ReadKey();
        }

        static Task<int> CreateTask(string name)
        {
            return new Task<int>(() => TaskMethod2(name));
        }

        static int TaskMethod2(string name)
        {
            Console.WriteLine("Task {0} is running on a thread id {1}. Is thread pool thread: {2}",
               name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(100);
            return name.Length;
        }

        static int GetSum()
        {
            int sum = 0;
            Console.WriteLine("使用Task执行异步操作.");
            for (int i = 0; i < 100; i++)
            {
                sum += i;
            }
            return sum;
        }
        #endregion
    }
}

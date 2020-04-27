using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World! Parallel");
            //Test1();
            //Test2();
            //Test3();
            //Test4();
            //Test5();
            //Test6();
            //Test7();
            //Test8();
            //Test9();
            //Test10();
            Test11();
        }

        #region Test1
        /// <summary>
        /// Parale.Invoke 用于任务的并行
        /// 函数的功能和Task有些相似，就是并发执行一系列任务，然后等待所有完成
        /// 和Task比起来，省略了Task.WaitAll这一步，自然也缺少了Task的相关管理功能
        /// </summary>
        static void Test1()
        {
            var actions = new Action[]
            {
                ()=>ActionTest("test 1"),
                ()=>ActionTest("test 2"),
                ()=>ActionTest("test 3"),
                ()=>ActionTest("test 4")
            };
            Console.WriteLine("Parallel.Invoke 1 Test");
            Parallel.Invoke(actions);

            Console.ReadKey();
        }

        static void ActionTest(object value)
        {
            Console.WriteLine(">>> thread:{0},value:{1}", Thread.CurrentThread.ManagedThreadId, value);
        }
        #endregion

        #region Test2
        /// <summary>
        /// For方法,主要用于处理针对数组元素的并行操作(数据的并行)
        /// </summary>
        static void Test2()
        {
            //int[] nums = new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            int[] nums = Enumerable.Range(0, 15).ToArray();
            Parallel.For(0, nums.Length, i =>
            {
                Console.WriteLine("针对数组索引{0}对应的那个元素{1}的一些工作...ThreadId={2}", i < 10 ? " " + i : i.ToString(), nums[i] < 10 ? " " + nums[i] : nums[i].ToString(), Thread.CurrentThread.ManagedThreadId);
            });
            Console.ReadKey();
        }
        #endregion

        #region Test3
        /// <summary>
        /// Foreach方法,主要用于处理泛型集合元素的并行操作(数据的并行)
        /// </summary>
        static void Test3()
        {
            List<int> nums = Enumerable.Range(1, 10).ToList();
            Parallel.ForEach(nums, item =>
            {
                Console.WriteLine("针对集合元素{0}的一些工作代码...ThreadId={1}", item, Thread.CurrentThread.ManagedThreadId);
            });
            Console.ReadKey();
        }
        #endregion

        #region Test4
        /// <summary>
        /// AsParallel并行
        /// </summary>
        static void Test4()
        {
            List<int> nums = Enumerable.Range(1, 10).ToList();
            var evenNumbers = nums.AsParallel().Select(item => Calculate(item));
            //注意这里是个延迟加载,也就是不用集合的时候这个Calculate里面的算法是不会执行的,可以注释下面一行代码看效果
            //Console.WriteLine(evenNumbers.Count());
            Console.ReadKey();
        }

        static int Calculate(int number)
        {
            Console.WriteLine("针对集合元素{0}的一些工作代码...ThreadId={1}", number, Thread.CurrentThread.ManagedThreadId);
            return number * 2;
        }
        #endregion

        #region Test5
        /// <summary>
        /// AsOrdered() 对结果进行排序
        /// </summary>
        static void Test5()
        {
            List<int> nums = Enumerable.Range(1, 10).ToList();
            var evenNumbers = nums.AsParallel().AsOrdered().Select(item =>
            {
                return Calculate(item);
            });
            //注意这里是个延迟加载,也就是不用集合的时候 这个Calculate里面的算法 是不会去运行 可以屏蔽下面的代码看效果;
            foreach (var item in evenNumbers)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
        #endregion

        #region Test6
        /// <summary>
        /// ForEach的独到之处就是可以将数据进行分区,每个小区内实现串行计算,分区采用Partitioner.Create实现
        /// List是非线程安全的,ConcurrentBag是线程安全的
        /// </summary>
        static void Test6()
        {
            List<int> total = new List<int>();
            for (int j = 0; j < 4; j++)
            {
                ConcurrentBag<int> bag = new ConcurrentBag<int>();
                var watch = Stopwatch.StartNew();
                watch.Start();
                Parallel.ForEach(Partitioner.Create(0, 3000000), i =>
                {
                    for (int m = i.Item1; m < i.Item2; m++)
                    {
                        bag.Add(m);
                    };
                    total.Add(bag.Count);
                });
                Console.WriteLine("并行计算: 集合有{0},总共耗时:{1}", bag.Count, watch.ElapsedMilliseconds);
                GC.Collect();
            }
            Console.WriteLine("List<int> 集合数为: " + total.Count + "  ,注意这里的值每次都可能不一样");
            Console.ReadKey();
        }
        #endregion

        #region Test7
        /// <summary>
        /// ParallelOptions 类
        /// 可以实例化出此类,然后可配置线程参数
        /// 有时候我们的线程可能会跑遍所有的内核，为了提高其他应用程序的稳定性，就要限制参与的内核
        /// </summary>
        static void Test7()
        {

            Stopwatch watch = new Stopwatch();
            watch.Start();

            var dic = LoadData();

            var query = (from n in dic.Values.AsParallel()
                         where n.Age > 20 && n.Age < 25
                         select n).ToList();
            watch.Stop();
            Console.WriteLine("查询:{0}", query.Count);
            Console.WriteLine("并行计算耗费时间:{0}", watch.ElapsedMilliseconds);
            Console.ReadKey();
        }

        static ConcurrentDictionary<int, Student> LoadData()
        {
            ConcurrentDictionary<int, Student> dic = new ConcurrentDictionary<int, Student>();
            ParallelOptions options = new ParallelOptions();
            //指定使用的硬件线程数为4
            options.MaxDegreeOfParallelism = 4;
            //预加载1500w条记录
            Parallel.For(0, 1500000, options, i =>
            {
                var single = new Student()
                {
                    ID = i,
                    Name = "hxc" + i,
                    Age = i % 151,
                    CreateTime = DateTime.Now.AddSeconds(i)
                };
                dic.TryAdd(i, single);
            });
            return dic;
        }

        class Student
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public DateTime CreateTime { get; set; }
        }
        #endregion

        #region Test8
        /// <summary>
        /// 退出并行执行的循环
        /// 在串行代码中我们break一下就搞定了，但是并行就不是这么简单了，不过没关系，在并行循环的委托参数中提供了一个ParallelLoopState，
        /// 该实例提供了Break和Stop方法来帮我们实现。
        /// Break: 当然这个是通知并行计算尽快的退出循环，比如并行计算正在迭代100，那么break后程序还会迭代所有小于100的。
        /// Stop：这个就不一样了，比如正在迭代100突然遇到stop，那它啥也不管了，直接退出。
        /// </summary>
        static void Test8()
        {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();

            Parallel.For(0, 2000000, (i, state) =>
            {
                if (bag.Count == 10000)
                {
                    //state.Break();//可能bag数不是10000 ,多试几次就能发现
                    state.Stop();//可能bag数不是10000 ,多试几次就能发现
                    return;
                }
                bag.Add(i);
            });

            Console.WriteLine("当前集合有{0}各元素", bag.Count);
            Console.ReadKey();
        }
        #endregion

        #region Test9
        /// <summary>
        /// 取消并行执行 cancel
        /// </summary>
        static void Test9()
        {
            var cts = new CancellationTokenSource();
            var ct = cts.Token;
            Task.Factory.StartNew(() => fun(ct));
            Console.ReadKey();
            cts.Cancel();
            Console.WriteLine("任务取消了!"); //在此之后还可能执行部分任务
            Console.ReadKey();
        }

        static void fun(CancellationToken token)
        {
            Parallel.For(0, 10000, new ParallelOptions { CancellationToken = token },
                i =>
                {
                    Thread.Sleep(50);//放慢执行速度
                    Console.WriteLine("针对数组索引{0}的一些工作代码...ThreadId={1}", i, Thread.CurrentThread.ManagedThreadId);
                });
        }
        #endregion

        #region Test10
        /// <summary>
        /// 获取异常
        /// 注意Parallel里面 不建议抛出异常 因为在极端的情况下比如进去的第一批线程先都抛异常了 
        /// 此时AggregateExcepation就只能捕获到这一批的错误,然后程序就结束了
        /// </summary>
        static void Test10()
        {
            try
            {
                Parallel.Invoke(Run1, Run2);
            }
            catch (AggregateException ex)
            {
                foreach (var single in ex.InnerExceptions)
                {
                    Console.WriteLine(single.Message);
                }
            }
            Console.WriteLine("执行结束");
            Console.ReadKey();
        }

        static void Run1()
        {
            Thread.Sleep(3000);
            throw new Exception("我是任务1抛出的异常");
        }

        static void Run2()
        {
            Thread.Sleep(5000);
            throw new Exception("我是任务2抛出的异常");
        }
        #endregion

        #region Test11
        public static List<int> NumberList = null;
        private static readonly object locker = new object();
        /// <summary>
        /// 避免捕获不全错误,可以如下处理
        /// 不在AggregateExcepation中来处理 而是在Parallel里面的try catch来记录错误,或处理错误
        /// </summary>
        static void Test11()
        {
            List<string> errList = new List<string>();
            Parallel.For(0, 10, i =>
            {
                try
                {
                    Test11_throw_err(i);
                }
                catch (Exception ex)
                {
                    lock (locker)
                    {
                        errList.Add(ex.Message);
                    }
                    //throw ex;//注意:这里不再将错误抛出
                }
            });

            int Index = 1;
            foreach (string err in errList)
            {
                Console.WriteLine("{0}、的错误:{1}", Index++, err);
            }

            Console.ReadKey();
        }

        static void Test11_throw_err(int Number)
        {
            throw new Exception("1111");
        }
        #endregion
    }
}

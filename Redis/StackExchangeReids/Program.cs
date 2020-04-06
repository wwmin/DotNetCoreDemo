using StackExchange.Redis;
using StackExchange.Redis.Profiling;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StackExchangeReids
{
    class Program
    {
        private static ConnectionMultiplexer _redis { get; set; }

        static void Main(string[] args)
        {
            _redis = ConnectionMultiplexer.Connect("localhost,allowAdmin=true");
            IDatabase db = _redis.GetDatabase();

            TestString();
            TestProfiling();
            //TestPubSub();
            #region 测试Fire-Forget模式
            int i = 0;
            //i = 2;
            if (i > 0)
            {
                while (i > 0)
                {
                    TestFireForget();
                    i--;
                }
                string pageKeyName = "pageIndexCount";

                var count = db.StringGet(pageKeyName);
                Console.WriteLine(count);
            }

            #endregion
            //TestKeyValueChannel();
            //TestTransaction();
            //TestEvents();
            //TestStream();
            TestServer();
            Console.ReadKey();
        }
        #region 测试String
        /// <summary>
        /// 测试String方式
        /// </summary>
        public static async void TestString()
        {
            IDatabase db = _redis.GetDatabase(1);
            await db.StringSetAsync("name", "wwmin", TimeSpan.FromSeconds(20));
            await db.StringSetAsync("age", 10, TimeSpan.FromSeconds(20));
            string value = await db.StringGetAsync("name");
            Console.WriteLine(value);

            byte[] k = new byte[] { 1, 2 }, v = new byte[] { 4, 5 };
            db.StringSet(k, v, TimeSpan.FromSeconds(20));
            byte[] vs = db.StringGet(k);
            Console.WriteLine(vs[0].ToString() + "," + vs[1].ToString());

            //异步

            var aPending = db.StringGetAsync("name");
            var bPending = db.StringGetAsync("age");
            //方式一 线程阻塞
            //var name = db.Wait(aPending);
            //var age = db.Wait(bPending);
            //Console.WriteLine(name + "|" + age);
            //方式二 线程阻塞
            //Task.WaitAll(aPending, bPending);
            //Console.WriteLine(aPending.Result + "," + bPending.Result);
            //方式三 线程不阻塞
            await Task.WhenAll(new[] { aPending, bPending }).ContinueWith(s =>
            {
                Console.WriteLine(string.Join("-", s.Result.ToList()));
            });
        }
        #endregion
        #region 测试发布/订阅
        /// <summary>
        /// 测试发布/订阅
        /// </summary>
        public static async void TestPubSub()
        {
            ISubscriber sub = _redis.GetSubscriber();
            sub.Subscribe("message", (channel, message) =>
            {
                Console.WriteLine((string)message);
            });
            sub.Subscribe("message").OnMessage(async channelMessage =>
            {
                await Task.Delay(1000);
                Console.WriteLine((string)channelMessage.Message);
            });

            await sub.PublishAsync("message", "hello");
        }
        #endregion
        #region Fire-Forget模式

        public static async void TestFireForget()
        {
            string pageKeyName = "pageIndexCount";
            IDatabase db = _redis.GetDatabase();
            await db.StringIncrementAsync(pageKeyName, 1, flags: CommandFlags.FireAndForget);

            //设置pageKeyName为滑动过期值的flag模式
            db.KeyExpire(pageKeyName, TimeSpan.FromSeconds(60), flags: CommandFlags.FireAndForget);
            var value = db.StringGet(pageKeyName);
            Console.WriteLine(value);
        }
        #endregion
        #region Key value
        public static void TestKeyValueChannel()
        {
            IDatabase db = _redis.GetDatabase(1);
            string key = "someKey";
            db.StringIncrement(key);
            string key2 = "someKey2";
            db.StringIncrement(key2);
            string someKey = db.KeyRandom();
            Console.WriteLine(someKey);
            db.KeyDelete(someKey);
        }
        #endregion
        #region Transactions in Redis
        public static void TestTransaction()
        {
            IDatabase db = _redis.GetDatabase(1);
            var newId = Guid.NewGuid();
            var tran = db.CreateTransaction();
            string custKey = "custKey";
            tran.AddCondition(Condition.HashNotExists(custKey, "UniqueId"));
            tran.HashSetAsync(custKey, "UniqueID", newId.ToString(), When.NotExists);
            db.KeyExpire(custKey, TimeSpan.FromSeconds(10), flags: CommandFlags.FireAndForget);
            bool committed = tran.Execute();
            if (committed)
            {
                Console.WriteLine("transaction success!");
            }
            else
            {
                Console.WriteLine("transaction error!");
            }
        }
        #endregion
        #region Events
        public async static void TestEvents()
        {
            IDatabase db = _redis.GetDatabase();
            _redis.ConfigurationChanged += new EventHandler<EndPointEventArgs>(AddEvent);
            TestEventHandle t = new TestEventHandle();
            await Task.Delay(2000);
            t.InitAddEvent(AddEvent);
            t.CallAddEvent();
        }

        public static void AddEvent(object sender, EventArgs e)
        {
            AddEventData ad = (AddEventData)e;
            int c = ad.a + ad.b;
            Console.WriteLine($"触发事件AddEvent, {ad.a}+{ad.b}={c}");
        }
        public class TestEventHandle
        {
            private event EventHandler<AddEventData> OnAddEvent;

            public void InitAddEvent(EventHandler<AddEventData> addEvent)
            {
                //this.OnAddEvent = addEvent;
                this.OnAddEvent += new EventHandler<AddEventData>(addEvent);
            }

            public void CallAddEvent()
            {
                if (OnAddEvent != null)
                {
                    AddEventData ad = new AddEventData();
                    ad.a = 1;
                    ad.b = 2;
                    OnAddEvent(this, ad);
                }
            }
        }

        public class AddEventData : EventArgs
        {
            public int a;
            public int b;
        }

        #endregion
        #region Stream
        public static void TestStream()
        {
            IDatabase db = _redis.GetDatabase();
            var messageId = db.StreamAdd("event_stream", "foo_name", "bar_value");
            Console.WriteLine(messageId);
        }
        #endregion
        #region Keys,Scan,Flushdb
        public static void TestServer()
        {
            var server = _redis.GetServer("localhost:6379");
            foreach (var key in server.Keys(pattern: "*"))
            {
                Console.WriteLine(key);
            }
            server.FlushAllDatabases();// _redis = ConnectionMultiplexer.Connect("localhost,allowAdmin=true");//config need allowAdmin = true
        }
        #endregion
        #region Profiling
        public static void TestProfiling()
        {
            var profiler = new AsyncLocalProfiler();
            _redis.RegisterProfiler(profiler.GetSession);
            //var commands = profiler.GetSession().FinishProfiling();
            //Console.WriteLine(string.Join(",", commands.Select(p => p.ElapsedTime)));

            var toyProfiler = new ToyProfiler();
            //var sharedSession = new ProfilingSession();
            _redis.RegisterProfiler(() => toyProfiler.PerThreadSession);
            var threads = new List<Thread>();
            var perThreadTimings = new ConcurrentDictionary<Thread, List<IProfiledCommand>>();
            for (int i = 0; i < 16; i++)
            {
                var db = _redis.GetDatabase(i);
                var thread = new Thread(
                    delegate ()
                    {
                        //set each thread to share a session
                        //toyProfiler.PerThreadSession = sharedSession;
                        var threadTasks = new List<Task>();
                        toyProfiler.PerThreadSession = new ProfilingSession();
                        for (int j = 0; j < 1000; j++)
                        {
                            var task = db.StringSetAsync("" + j, "" + j);
                            threadTasks.Add(task);
                        }
                        Task.WaitAll(threadTasks.ToArray());
                        perThreadTimings[Thread.CurrentThread] = toyProfiler.PerThreadSession.FinishProfiling().ToList();
                    });
                threads.Add(thread);
            }
            threads.ForEach(thread => thread.Start());
            threads.ForEach(thread => thread.Join());
            //var timings = sharedSession.FinishProfiling();
            Console.WriteLine(perThreadTimings.Count);
        }
        class AsyncLocalProfiler
        {
            private readonly AsyncLocal<ProfilingSession> perThreadSession = new AsyncLocal<ProfilingSession>();
            public ProfilingSession GetSession()
            {
                var val = perThreadSession.Value;
                if (val == null)
                {
                    perThreadSession.Value = val = new ProfilingSession();
                }
                return val;
            }
        }

        class ToyProfiler
        {
            private readonly ThreadLocal<ProfilingSession> perThreadSession = new ThreadLocal<ProfilingSession>();
            public ProfilingSession PerThreadSession
            {
                get => perThreadSession.Value;
                set => perThreadSession.Value = value;
            }
        }


        #endregion
    }
}

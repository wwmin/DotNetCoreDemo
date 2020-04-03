using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StackExchangeReids
{
    class Program
    {
        private static ConnectionMultiplexer _redis { get; set; }

        static void Main(string[] args)
        {
            _redis = ConnectionMultiplexer.Connect("localhost");
            //TestString();
            //TestPubSub();
            #region 测试Fire-Forget模式
            int i = 0;
            i = 2;
            if (i > 0)
            {
                while (i > 0)
                {
                    TestFireForget();
                    i--;
                }
                string pageKeyName = "pageIndexCount";
                IDatabase db = _redis.GetDatabase();
                var count = db.StringGet(pageKeyName);
                Console.WriteLine(count);
            }

            #endregion


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
    }
}

using System;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Abstractions;

namespace XUnitTestCS
{
    public class UnitTest1
    {
        private ITestOutputHelper _out;
        public UnitTest1(ITestOutputHelper outputHelper)
        {
            _out = outputHelper;
        }
        #region 给类型实现解构
        [Fact]
        public void Test1()
        {
            var x = new MyDeconstruct();
            var (o, u) = x;
            Assert.Equal(3, o + u);
        }

        class MyDeconstruct
        {
            private int A => 1;
            private int B => 2;
            public void Deconstruct(out int a, out int b)
            {
                a = A;
                b = B;
            }
        }
        #endregion
        #region 不是只有Task和ValueTask才能await
        [Fact]
        public async void TestMyTask()
        {
            var obj = new MyTask<int>();
            //await obj.GetAwaiter();
            var a = await obj;
            Console.WriteLine(a.ToString());
        }
        /// <summary>
        /// 封装自己的异步类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class MyTask<T>
        {
            public MyAwaiter<T> GetAwaiter()
            {
                return new MyAwaiter<T>();
            }
        }

        public class MyAwaiter<T> : INotifyCompletion
        {
            public bool IsCompleted { get; private set; }
            public T GetResult()
            {
                //throw new NotImplementedException();
                return default;
            }
            public void OnCompleted(Action continuation)
            {
                //throw new NotImplementedException();
                continuation.Invoke();
            }
        }
        #endregion
        #region 不是只有IEnuerable和IEnumerator才能被foreach
        [Fact]
        public void TestMyEnumerator()
        {
            var x = new MyEnumerable<int>();
            foreach (var item in x)
            {

            }
        }

        class MyEnumerator<T>
        {
            public T Current { get; private set; }
            public bool MoveNext()
            {
                throw new NotImplementedException();
            }
        }

        class MyEnumerable<T>
        {
            public MyEnumerator<T> GetEnumerator()
            {
                return new MyEnumerator<T>();
            }
        }
        #endregion
        #region 不是只有 IAsyncEnumerable 和 IAsyncEnumerator 才能被 await foreach
        [Fact]
        public async void TestMyAsync()
        {
            var x = new MyAsyncEnumerable<int>();
            await foreach (var i in x)
            {

            }
        }
        class MyAsyncEnumerator<T>
        {
            public T Current { get; private set; }
            public MyTask<bool> MoveNextAsync()
            {
                throw new NotImplementedException();
            }
        }

        class MyAsyncEnumerable<T>
        {
            public MyAsyncEnumerator<T> GetAsyncEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        #endregion
        #region ref struct要怎么实现IDisposable
        [Fact]
        public void TestMyDisposable()
        {
            MyDisposable y;
            using (y = new MyDisposable(10))
            {
                //y.i = 10;
                Assert.Equal(10, y.i);
            }
            y.Dispose();
            //using var y = new MyDisposable();
            Assert.Equal(0, y.i);
        }

        ref struct MyDisposable
        {
            public MyDisposable(int i)
            {
                this.i = i;
            }
            public int i { get; set; }
            public void Dispose() => this.i = 0;
        }
        #endregion
        #region 不是只有Range才能使用切片

        [Fact]
        public void TestRange()
        {

            string url = "https://www.baidu.com/index.html";
            string baidu = url[8..url.LastIndexOf("/")];
            _out.WriteLine(baidu);
            Assert.Equal("www.baidu.com", baidu);

            var myRange = new MyRange(url);
            var baiduUrl = myRange[8..url.LastIndexOf("/")];
            _out.WriteLine(baiduUrl.ToString());
            Assert.Equal("www.baidu.com", baiduUrl);

        }
        class MyRange
        {
            private string name { get; set; }
            public MyRange(string s)
            {
                this.name = s;
            }
            public int Count { get; private set; }
            public string Slice(int x, int y) => this.name.Substring(x, y);
        }
        #endregion
        #region 不是只有实现了IEnumerable才能用LINQ

        [Fact]
        public void TestLinq()
        {
            var x = new Just<int>(3);
            var y = new Just<int>(7);
            var z = new Nothing<int>();

            Maybe<int> u = from x0 in x from y0 in y select x0 + y0;
            Maybe<int> v = from x0 in x from z0 in z select x0 + z0;
            Maybe<int> just = from c in x where true select c;
            Maybe<int> nothing = from c in x where false select c;
            Assert.Equal("Just 10", u.ToString());
            Assert.Equal("Nothing", v.ToString());
            Assert.Equal("Just 3", just.ToString());
            Assert.Equal("Nothing", nothing.ToString());
        }

        class Just<T> : Maybe<T>
        {
            private readonly T value;
            public Just(T value) { this.value = value; }
            public override Maybe<U> Select<U>(Func<T, Maybe<U>> f) => f(value);

            public override string ToString() => $"Just {value}";
        }

        class Nothing<T> : Maybe<T>
        {
            public override Maybe<U> Select<U>(Func<T, Maybe<U>> f) => new Nothing<U>();
            public override string ToString()
            {
                return "Nothing";
            }
        }

        abstract class Maybe<T>
        {
            public abstract Maybe<U> Select<U>(Func<T, Maybe<U>> f);

            public Maybe<V> SelectMany<U, V>(Func<T, Maybe<U>> k, Func<T, U, V> s) => Select(x => k(x).Select(y => new Just<V>(s(x, y))));

            public Maybe<T> Where(Func<Maybe<T>, bool> f) => f(this) ? this : new Nothing<T>();
        }
        #endregion
    }
}

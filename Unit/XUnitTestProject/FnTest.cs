using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using XUnitTestProject.Infrastructure;

namespace XUnitTestProject
{
    public class FnTest
    {
        private ITestOutputHelper _out;
        public FnTest(ITestOutputHelper outputHelper)
        {
            _out = outputHelper;
        }

        /// <summary>
        /// 笛卡尔集 然后相加
        /// </summary>
        [Fact]
        public void Test1()
        {
            int[] a1 = { 1, 2, 3, 4, 5 };
            int[] a2 = { 5, 4, 3, 2, 1 };
            var res = a1.SelectMany(v => a2, (v1, v2) => $"{v1}+{v2}={v1 + v2}").ToList();
            Assert.Equal(a1.Count() * a2.Count(), res.Count);
        }

        /// <summary>
        /// delegate与event 本质都是多播委托
        /// </summary>
        [Fact]
        public void Test2()
        {
            string text = "Hello World";
            Action v = () => _out.WriteLine(text);
            v += () => _out.WriteLine(text.Length.ToString());
            v();
            Assert.Equal("Hello World", text);
        }

        /// <summary>
        /// 测试yield
        /// </summary>
        /// <param name="n"></param>
        [Theory]
        [InlineData(4)]
        public void Test3(int n)
        {
            var data = GenerateFibonacci(n).ToList();
            Assert.Equal(n, data.Count);
        }

        /// <summary>
        /// 使用yield方式返回斐波那契数列
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public IEnumerable<int> GenerateFibonacci(int n)
        {

            if (n >= 1) yield return 1;
            int a = 1, b = 0;
            for (int i = 2; i <= n; ++i)
            {

                int t = b;
                b = a;
                a += t;
                yield return a;
            }

        }

        /// <summary>
        /// 测试复合函数
        /// </summary>
        [Fact]
        public void Test4()
        {
            Func<int, int> add = (int i) => i + 1;
            Func<int, int> sub = (int i) => i - 1;
            var f = add.Compose(sub);
            var a = f(1);
            Assert.Equal(1, a);
        }


        #region Myabe
        /// <summary>
        /// 实现 Maybe<T> monad，并利用 LINQ实现对 Nothing（空值）和 Just（有值）的求和
        /// </summary>
        [Fact]
        public void Test5()
        {
            Maybe<int> a = Maybe.Just(5);
            Maybe<int> b = Maybe.Nothing<int>();
            Maybe<int> c = Maybe.Just(10);
            _out.WriteLine(a.ToString());
            _out.WriteLine(b.ToString());
            _out.WriteLine(c.ToString());

            Maybe<List<int>> ints = new Maybe<List<int>>();

            Assert.Equal("Nothing", b.ToString());
        }

        /// <summary>
        /// 实现 Maybe<T> monad，并利用 LINQ实现对 Nothing（空值）和 Just（有值）的求和
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public struct Maybe<T>
        {
            public readonly bool HasValue;
            public readonly T Value;
            public Maybe(bool hasValue, T value)
            {
                HasValue = hasValue;
                Value = value;
            }

            public Maybe<B> SelectMany<TCollection, B>(Func<T, Maybe<TCollection>> collectionSelector, Func<T, TCollection, B> f)
            {
                if (!HasValue) return Maybe.Nothing<B>();

                Maybe<TCollection> collection = collectionSelector(Value);
                if (!collection.HasValue) return Maybe.Nothing<B>();
                return Maybe.Just(f(Value, collection.Value));
            }

            public override string ToString() => HasValue ? $"Just {Value}" : "Nothing";

        }

        public class Maybe
        {
            public static Maybe<T> Just<T>(T value)
            {
                return new Maybe<T>(true, value);
            }
            public static Maybe<T> Nothing<T>()
            {
                return new Maybe<T>();
            }
        }
        #endregion

    }
}

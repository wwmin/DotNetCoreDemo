using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

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
    }
}

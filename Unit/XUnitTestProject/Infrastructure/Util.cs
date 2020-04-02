using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestProject.Infrastructure
{
    public static class Util
    {
        /// <summary>
        /// 将多个函数复合
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns></returns>
        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T1, T2> f1, Func<T2, T3> f2)
        {
            return x => f2(f1(x));
        }
    }
}

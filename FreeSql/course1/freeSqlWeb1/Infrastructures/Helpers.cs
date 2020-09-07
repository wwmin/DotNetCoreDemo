using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace freeSqlWeb1.Infrastructures
{
    public static class Helpers
    {

        /// <summary>
        /// 获取继承T类的所有子类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<Type> GetModels<T>() where T : class
        {
            try
            {
                var assembly = Assembly.GetAssembly(typeof(T));
                var types = assembly.GetTypes();

                var currentType = typeof(T);
                string currentTypeName = currentType.Name;
                var res = new List<Type>();
                foreach (var item in types)
                {
                    var bt = item.BaseType;
                    while (bt != null)
                    {
                        if (bt.Name == currentTypeName)
                        {
                            #region 获取实体类实例
                            //Type ot = Type.GetType(currentType.FullName, true);
                            //object obj = Activator.CreateInstance(ot);
                            //if (obj != null)
                            //{
                            //    T info = obj as T;
                            //    res.Add(info);
                            //}
                            #endregion
                            res.Add(item);
                            break;
                        }
                        else
                        {
                            bt = bt.BaseType;//继续获取更深层父级
                        }
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 获取实现了IT接口的所有实体类
        /// </summary>
        /// <typeparam name="IT"></typeparam>
        /// <returns></returns>
        public static List<Type> GetInterfaceModels<IT>() where IT : class
        {
            try
            {
                var assembly = Assembly.GetAssembly(typeof(IT));
                var types = assembly.GetTypes();

                var res = new List<Type>();
                foreach (Type item in types)
                {
                    if (item.IsInterface)
                        continue;//判断是否是接口
                    Type[] ins = item.GetInterfaces();
                    foreach (Type t in ins)
                    {
                        if (t != typeof(IT))//是否实现了IT接口
                            continue;
                        //var method = item.GetMethod("GetMapTypes");//获取类型的方法
                        //var impl = (IT)assembly.CreateInstance(item.FullName);//实例化
                        res.Add(t);
                        break;
                    }
                }
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region 类型转换


        /// <summary>
        /// Enumerable<string> 转 Enumerable<int>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<int> ToInt(this IEnumerable<string> source)
        {
            foreach (string item in source)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    yield return 0;
                }
                else
                {
                    int defaultValue;
                    int.TryParse(item, out defaultValue);
                    yield return defaultValue;
                }
            }
        }

        /// <summary>
        /// 字符串转int
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ToInt(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return 0;
            }
            int defaultValue;
            int.TryParse(source, out defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// Enumerable<string> 转 Enumerable<int>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<double> ToDouble(this IEnumerable<string> source)
        {
            foreach (string item in source)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    yield return 0d;
                }
                else
                {
                    double defaultValue;
                    double.TryParse(item, out defaultValue);
                    yield return defaultValue;
                }
            }
        }
        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double ToDouble(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return 0;
            }
            double defaultValue;
            double.TryParse(source, out defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return 0;
            }
            decimal defaultValue;
            decimal.TryParse(source, out defaultValue);
            return defaultValue;
        }

        public static string ToJson<T>(this T source) where T : class, new()
        {
            if (source == null)
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(source);
        }

        /// <summary>
        /// 对象序列化为string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            if (obj == null) return string.Empty;
            return obj switch
            {
                byte r => r.ToString(),
                string r => r,
                int r => r.ToString(),
                double r => r.ToString(),
                decimal r => r.ToString(),
                float r => r.ToString(),
                _ => JsonConvert.SerializeObject(obj)
            };
        }
        #endregion
    }
}

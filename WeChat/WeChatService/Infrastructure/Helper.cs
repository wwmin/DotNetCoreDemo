using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace WeChatService.Infrastructure
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// 创建client
        /// </summary>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        public static HttpClient CreateHttpClient(string baseUri)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }
        /// <summary>
        /// 解析JSON字符串成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToEntity<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }
        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToListEntitys<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }
        /// <summary>
        /// 反序列化JSON到给定的匿名对象
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }
        /// <summary>
        /// 将对象序列化为JSON格式,包括是否成功result，错误描述error，具体数据data
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="pError">错误描述</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o, bool isSuccess = true, string pError = "")
        {
            Dictionary<string, object> mResult = new Dictionary<string, object>();
            mResult.Add("result", isSuccess);
            mResult.Add("error", pError);
            mResult.Add("data", o);
            string json = JsonConvert.SerializeObject(mResult);
            return json;
        }
        /// <summary>
        /// 将对象序列化为JSON格式,包括是否成功result，错误描述error，具体数据data
        /// </summary>
        /// <param name="t">对象</param>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="pError">错误描述</param>
        /// <returns>json字符串</returns>
        public static string SerializeListEntitys<T>(this List<T> t, bool isSuccess = true, string pError = "")
        {
            Dictionary<string, object> mResult = new Dictionary<string, object>();
            mResult.Add("result", isSuccess);
            mResult.Add("error", pError);
            mResult.Add("data", t);
            string json = JsonConvert.SerializeObject(mResult);
            return json;
        }

        /// <summary>
        /// 将对象序列化为JSON格式,包括是否成功result，错误描述error，具体数据data
        /// </summary>
        /// <param name="t">对象</param>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="pError">错误描述</param>
        /// <returns>json字符串</returns>
        public static string SerializeEntity<T>(this T t, bool isSuccess = true, string pError = "")
        {
            Dictionary<string, object> mResult = new Dictionary<string, object>();
            mResult.Add("result", isSuccess);
            mResult.Add("error", pError);
            mResult.Add("data", t);
            string json = JsonConvert.SerializeObject(mResult);
            return json;
        }

        /// <summary>
        /// 将对象序列化为JSON格式,包括是否成功result，错误描述error，具体数据data
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="pError">错误描述</param>
        /// <returns>HttpResponseMessage</returns>
        public static HttpResponseMessage HttpResponseSerializeEntity<T>(this T t, bool isSuccess = true, string pError = "")
        {
            Dictionary<string, object> mResult = new Dictionary<string, object>();
            mResult.Add("result", isSuccess);
            mResult.Add("error", pError);
            mResult.Add("data", t);
            string json = JsonConvert.SerializeObject(mResult);
            return new HttpResponseMessage { Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json") };
        }

        /// <summary>
        /// 将对象序列化为JSON格式,包括是否成功result，错误描述error，具体数据data
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="pError">错误描述</param>
        /// <returns>json字符串</returns>
        public static HttpResponseMessage HttpResponseSerializeListEntitys<T>(this List<T> t, bool isSuccess = true, string pError = "")
        {
            Dictionary<string, object> mResult = new Dictionary<string, object>();
            mResult.Add("result", isSuccess);
            mResult.Add("error", pError);
            mResult.Add("data", t);
            string json = JsonConvert.SerializeObject(mResult);
            return new HttpResponseMessage { Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json") };
        }

        /// <summary>
        /// 将对象序列化为JSON格式,包括是否成功result，错误描述error，具体数据data
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="pError">错误描述</param>
        /// <returns>json字符串</returns>
        public static HttpResponseMessage HttpResponseSerializeObject(this object o, bool isSuccess = true, string pError = "")
        {
            Dictionary<string, object> mResult = new Dictionary<string, object>();

            mResult.Add("result", isSuccess);
            mResult.Add("error", pError);
            mResult.Add("data", o);
            string json = JsonConvert.SerializeObject(mResult);
            return new HttpResponseMessage { Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json") };
        }
        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <param name="source">经过Unicode编码的字符串</param>
        /// <returns>正常字符串</returns>
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                   source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
    }
}

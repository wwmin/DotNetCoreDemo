using freeSqlWeb1.Infrastructures.SettingOptions;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace freeSqlWeb1.Infrastructures.Utils
{
    public static class RedisUtil
    {
        private static ConnectionMultiplexer _redis { get; set; }
        private static IDatabase db = null;
        private static string instanceName = string.Empty;
        private static RedisOption _redisOption;

        private static ConcurrentDictionary<string, ConnectionMultiplexer> _connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        /// <summary>
        /// 初始化Db
        /// </summary>
        /// <param name="redisOption"></param>
        public static void Init(RedisOption redisOption/*ConnectionMultiplexer _redisConnectionMultiplexer, string _instanceName, */)
        {
            instanceName = redisOption.InstanceName;
            _redisOption = redisOption;
            _redis = GetConnect();
            db = GetDatabase();
        }
        #region Redis 元操作


        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        public static ConnectionMultiplexer GetConnect()
        {
            return _connections.GetOrAdd(_redisOption.InstanceName, p => ConnectionMultiplexer.Connect(_redisOption.ConnectionString));
        }
        /// <summary>
        /// 获取数据库实例
        /// </summary>
        /// <returns></returns>
        public static IDatabase GetDatabase()
        {
            return GetConnect().GetDatabase(_redisOption.DefaultDatabase);
        }

        /// <summary>
        /// 获取数据库收发消息连接
        /// </summary>
        /// <returns></returns>
        public static ISubscriber GetSubscriber()
        {
            return GetConnect().GetSubscriber();
        }
        #endregion
        #region 提取实体修改方案
        /// <summary>
        /// 保存获取修改一条数据库数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async static Task<bool> EntityAddEditAsync<T>(T entity) where T : BaseEntity
        {
            if (entity == null) return false;
            return await HashSetAsync(UKey<T>(), entity.Id, entity);
        }


        /// <summary>
        /// 保存获取修改多条数据库数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">List<T></param>
        /// <returns></returns>
        public async static Task EntityAddEditRangeAsync<T>(List<T> entities) where T : BaseEntity
        {
            if (entities != null)
            {
                await HashSetJsonListAsync(UKey<T>(), entities, item => item.Id);
            };
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public async static Task<bool> EntityDelAsync<T>(T entity) where T : BaseEntity
        {
            return await HashDeleteAsync(UKey<T>(), entity.Id);
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public async static Task<bool[]> EntityDelRangeAsync<T>(List<T> entities) where T : BaseEntity
        {
            //HashDeleteAsync
            List<Task<bool>> tasks = new List<Task<bool>>();
            foreach (var item in entities)
            {
                tasks.Add(HashDeleteAsync(UKey<T>(), item.Id));
            }
            return await Task.WhenAll(tasks.ToArray());
        }

        /// <summary>
        /// 清空整张表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async static Task<bool> EntityRemoveTableAsync<T>() where T : BaseEntity
        {
            return await db.KeyDeleteAsync(UKey<T>());
        }
        #endregion

        #region 通用
        /// <summary>
        /// 获取类名 如果带id则返回形式为 sys_user:1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string UKey<T>() where T : class
        {
            return instanceName + ":" + typeof(T).Name;
        }

        /// <summary>
        /// 设置key过期时间
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="t"></param>
        public static void KeyExpire(string redisKey, TimeSpan? t = null)
        {
            if (t == null)
                t = TimeSpan.FromMinutes(_redisOption.DbTableCacheMinutes);
            db.KeyExpire(redisKey, t);
        }
        /// <summary>
        /// 设置key过期时间
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="t"></param>
        public async static void KeyExpireAsync(string redisKey, TimeSpan? t = null)
        {
            if (t == null)
                t = TimeSpan.FromMinutes(_redisOption.DbTableCacheMinutes);
            await db.KeyExpireAsync(redisKey, t);
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="key"></param>
        public static bool Remove(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return db.KeyDelete(key);
            }
            return false;
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="key"></param>
        public async static Task<bool> RemoveAsync(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return await db.KeyDeleteAsync(key);
            }
            return false;
        }
        #endregion

        #region Hash
        /// <summary>
        /// 判断该字段是否存在 hash 中
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static bool HashExists(string redisKey, RedisValue hashField)
        {
            return db.HashExists(redisKey, hashField);
        }

        /// <summary>
        /// 判断该字段是否存在 hash 中
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async static Task<bool> HashExistsAsync(string redisKey, RedisValue hashField)
        {
            return await db.HashExistsAsync(redisKey, hashField);
        }

        /// <summary>
        /// 从 hash 中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static bool HashDelete(string redisKey, RedisValue hashField)
        {
            return db.HashDelete(redisKey, hashField);
        }

        /// <summary>
        /// 从 hash 中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async static Task<bool> HashDeleteAsync(string redisKey, RedisValue hashField)
        {
            return await db.HashDeleteAsync(redisKey, hashField);
        }

        /// <summary>
        /// 从 hash 中移除多个指定字段
        /// </summary>
        /// <param name="rediskey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static long HashDelete(string rediskey, IEnumerable<RedisValue> hashField)
        {
            return db.HashDelete(rediskey, hashField.ToArray());
        }
        /// <summary>
        /// 从 hash 中移除多个指定字段
        /// </summary>
        /// <param name="rediskey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async static Task<long> HashDeleteAsync(string rediskey, IEnumerable<RedisValue> hashField)
        {
            return await db.HashDeleteAsync(rediskey, hashField.ToArray());
        }

        /// <summary>
        /// 递增  默认按1递增  可用于计数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static long HashIncrement(string redisKey, RedisValue hashField, TimeSpan? t = null)
        {
            var result = db.HashIncrement(redisKey, hashField);
            KeyExpire(redisKey, t);
            return result;
        }



        /// <summary>
        /// 递增  默认按1递增  可用于计数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async static Task<long> HashIncrementAsync(string redisKey, string hashField, TimeSpan? t = null)
        {
            var result = await db.HashIncrementAsync(redisKey, hashField);
            KeyExpireAsync(redisKey, t);
            return result;
        }
        /// <summary>
        /// 递减  默认按1递减   可用于抢购类的案例
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static long HashDecrement(string redisKey, string hashField, TimeSpan? t = null)
        {
            var result = db.HashDecrement(redisKey, hashField);
            KeyExpire(redisKey, t);
            return result;
        }
        /// <summary>
        /// 递减  默认按1递减   可用于抢购类的案例
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async static Task<long> HashDecrementAsync(string redisKey, string hashField, TimeSpan? t = null)
        {
            var result = await db.HashDecrementAsync(redisKey, hashField);
            KeyExpireAsync(redisKey, t);
            return result;
        }
        /// <summary>
        /// 在 hash 中保存或修改一个值   字符类型
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool HashSet(string redisKey, string hashField, string value, TimeSpan? t = null)
        {
            KeyExpire(redisKey, t);
            return db.HashSet(redisKey, hashField, value);
        }
        /// <summary>
        /// 在 hash 中保存或修改一个值   字符类型
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async static Task<bool> HashSetAsync(string redisKey, string hashField, string value, TimeSpan? t = null)
        {
            KeyExpire(redisKey, t);
            return await db.HashSetAsync(redisKey, hashField, value);
        }
        /// <summary>
        /// 保存一个字符串类型集合
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="stringList"></param>
        /// <param name="getFieldKey"></param>
        /// <param name="t"></param>
        public static void HashSetList(string redisKey, IEnumerable<string> stringList, Func<string, string> getFieldKey, TimeSpan? t = null)
        {
            List<HashEntry> listHashEntry = new List<HashEntry>();
            foreach (var item in stringList)
            {
                listHashEntry.Add(new HashEntry(getFieldKey(item), item));
            }
            db.HashSet(redisKey, listHashEntry.ToArray());
            KeyExpire(redisKey, t);
        }

        /// <summary>
        /// 保存一个字符串类型集合
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="stringList"></param>
        /// <param name="getFieldKey"></param>
        /// <param name="t"></param>
        public async static void HashSetListAsync(string redisKey, IEnumerable<string> stringList, Func<string, string> getFieldKey, TimeSpan? t = null)
        {
            List<HashEntry> listHashEntry = new List<HashEntry>();
            foreach (var item in stringList)
            {
                listHashEntry.Add(new HashEntry(getFieldKey(item), item));
            }
            await db.HashSetAsync(redisKey, listHashEntry.ToArray());
            KeyExpire(redisKey, t);
        }

        /// <summary>
        /// 保存多个对象集合  非序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="list"></param>
        /// <param name="getModelId"></param>
        public static void HashSetObjectList<T>(string tableName, IEnumerable<T> list, Func<T, string> getModelId) where T : class
        {
            foreach (var item in list)
            {
                List<HashEntry> listHashEntry = new List<HashEntry>();
                Type t = item.GetType();//获得该类的Type
                foreach (PropertyInfo pi in t.GetProperties())
                {
                    string name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行子要想要的操作
                    var value = pi.GetValue(item, null);//用pi.GetValue获取值
                    listHashEntry.Add(new HashEntry(name, value.ToString()));
                }
                db.HashSet(tableName + getModelId(item), listHashEntry.ToArray());
            }
        }
        /// <summary>
        /// 保存多个对象集合  非序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="list"></param>
        /// <param name="getModelId"></param>
        public async static void HashSetObjectListAsync<T>(string tableName, IEnumerable<T> list, Func<T, string> getModelId) where T : class
        {
            foreach (var item in list)
            {
                List<HashEntry> listHashEntry = new List<HashEntry>();
                Type t = item.GetType();//获得该类的Type
                foreach (PropertyInfo pi in t.GetProperties())
                {
                    string name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行子要想要的操作
                    var value = pi.GetValue(item, null);//用pi.GetValue获取值
                    listHashEntry.Add(new HashEntry(name, value.ToString()));
                }
                await db.HashSetAsync(tableName + getModelId(item), listHashEntry.ToArray());
            }
        }
        /// <summary>
        /// 保存或修改一个hash对象（序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool HashSet<T>(string redisKey, RedisValue hashField, T value, TimeSpan? t = null)
        {
            var json = JsonSerializer.Serialize(value);
            KeyExpire(redisKey, t);
            return db.HashSet(redisKey, hashField, json);
        }

        /// <summary>
        /// 保存或修改一个hash对象（序列化）`
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async static Task<bool> HashSetAsync<T>(string redisKey, RedisValue hashField, T value, TimeSpan? t = null)
        {
            var json = JsonSerializer.Serialize(value);
            KeyExpire(redisKey, t);
            return await db.HashSetAsync(redisKey, hashField, json);
        }


        /// <summary>
        /// 保存Hash对象集合 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="list"></param>
        /// <param name="getModelId"></param>
        /// <param name="t"></param>
        public static void HashSetJsonList<T>(string redisKey, IEnumerable<T> list, Func<T, RedisValue> getModelId, TimeSpan? t = null) where T : class
        {
            List<HashEntry> listHashEntry = new List<HashEntry>();
            foreach (var item in list)
            {
                string json = JsonSerializer.Serialize(item);
                listHashEntry.Add(new HashEntry(getModelId(item), json));
            }
            db.HashSet(redisKey, listHashEntry.ToArray());
            KeyExpire(redisKey, t);
        }

        /// <summary>
        /// 保存Hash对象集合 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="list"></param>
        /// <param name="getModelId"></param>
        /// <param name="t"></param>
        public async static Task HashSetJsonListAsync<T>(string redisKey, IEnumerable<T> list, Func<T, RedisValue> getModelId, TimeSpan? t = null) where T : class
        {
            List<HashEntry> listHashEntry = new List<HashEntry>();
            foreach (var item in list)
            {
                string json = JsonSerializer.Serialize(item);
                listHashEntry.Add(new HashEntry(getModelId(item), json));
            }
            await db.HashSetAsync(redisKey, listHashEntry.ToArray());
            KeyExpireAsync(redisKey, t);
        }

        /// <summary>
        /// 从 hash 中获取对象（反序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static T HashGet<T>(string redisKey, RedisValue hashField) where T : class
        {
            return JsonSerializer.Deserialize<T>(db.HashGet(redisKey, hashField));
        }


        /// <summary>
        /// 从 hash 中获取对象（反序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async static Task<T> HashGetAsync<T>(string redisKey, RedisValue hashField) where T : class
        {
            return JsonSerializer.Deserialize<T>(await db.HashGetAsync(redisKey, hashField));
        }

        /// <summary>
        /// 根据hashkey获取所有的值  序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static List<T> HashGetAll<T>(string redisKey) where T : class
        {
            List<T> result = new List<T>();
            HashEntry[] arr = db.HashGetAll(redisKey);
            foreach (var item in arr)
            {
                if (!item.Value.IsNullOrEmpty)
                {
                    var t = JsonSerializer.Deserialize<T>(item.Value);
                    if (t != null)
                    {
                        result.Add(t);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 根据hashkey获取所有的值  序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async static Task<List<T>> HashGetAllAsync<T>(string redisKey) where T : class
        {
            List<T> result = new List<T>();
            HashEntry[] arr = await db.HashGetAllAsync(redisKey);
            foreach (var item in arr)
            {
                if (!item.Value.IsNullOrEmpty)
                {
                    var t = JsonSerializer.Deserialize<T>(item.Value);
                    if (t != null)
                    {
                        result.Add(t);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 根据hashkey获取所有的值  非序列化
        /// </summary>
        /// <param name="redisKeys"></param>
        /// <returns></returns>
        public static IEnumerable<Dictionary<RedisValue, RedisValue>> HashGetAll(IEnumerable<string> redisKeys)
        {
            List<Dictionary<RedisValue, RedisValue>> dicList = new List<Dictionary<RedisValue, RedisValue>>();
            foreach (var item in redisKeys)
            {
                HashEntry[] arr = db.HashGetAll(item);
                foreach (var he in arr)
                {
                    Dictionary<RedisValue, RedisValue> dic = new Dictionary<RedisValue, RedisValue>();
                    if (!he.Value.IsNull)
                    {
                        dic.Add(he.Name, he.Value);
                    }
                    dicList.Add(dic);
                }
            }
            return dicList;
        }

        /// <summary>
        /// 根据hashkey获取所有的值  非序列化
        /// </summary>
        /// <param name="redisKeys"></param>
        /// <returns></returns>
        public async static Task<IEnumerable<Dictionary<RedisValue, RedisValue>>> HashGetAllAsync(IEnumerable<string> redisKeys)
        {
            List<Dictionary<RedisValue, RedisValue>> dicList = new List<Dictionary<RedisValue, RedisValue>>();
            foreach (var item in redisKeys)
            {
                HashEntry[] arr = await db.HashGetAllAsync(item);
                foreach (var he in arr)
                {
                    Dictionary<RedisValue, RedisValue> dic = new Dictionary<RedisValue, RedisValue>();
                    if (!he.Value.IsNull)
                    {
                        dic.Add(he.Name, he.Value);
                    }
                    dicList.Add(dic);
                }
            }
            return dicList;
        }

        /// <summary>
        /// 根据hashkey获取单个字段hashField的值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static RedisValue HashGet(string redisKey, RedisValue hashField)
        {
            return db.HashGet(redisKey, hashField);
        }

        /// <summary>
        /// 根据hashkey获取多个字段hashField的值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static RedisValue[] HashGet(string redisKey, RedisValue[] hashField)
        {
            return db.HashGet(redisKey, hashField);
        }
        /// <summary>
        /// 根据hashkey获取多个字段hashField的值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async static Task<RedisValue[]> HashGetAsync(string redisKey, RedisValue[] hashField)
        {
            return await db.HashGetAsync(redisKey, hashField);
        }
        /// <summary>
        /// 根据hashkey返回所有的HashFiled
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static IEnumerable<RedisValue> HashKeys(string redisKey)
        {
            return db.HashKeys(redisKey);
        }

        /// <summary>
        /// 根据hashkey返回所有的HashFiled
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async static Task<IEnumerable<RedisValue>> HashKeysAsync(string redisKey)
        {
            return await db.HashKeysAsync(redisKey);
        }
        /// <summary>
        /// 根据hashkey返回所有HashValue值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static RedisValue[] HashValues(string redisKey)
        {
            return db.HashValues(redisKey);
        }
        /// <summary>
        /// 根据hashkey返回所有HashValue值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async static Task<RedisValue[]> HashValuesAsync(string redisKey)
        {
            return await db.HashValuesAsync(redisKey);
        }
        #endregion

        #region String
        public static bool StringSet(string key, object value, TimeSpan? t = null)
        {
            //string strValue = value.ToJson();
            return db.StringSet(key, value.ToJson());
        }
        public async static Task<bool> StringSetAsync(string key, object value, TimeSpan? t = null)
        {
            //string strValue = value.ToJson();
            var b = await db.StringSetAsync(key, value.ToJson());
            KeyExpireAsync(key, t);
            return b;
        }

        public static string StringGet(string key)
        {
            return db.StringGet(key);
        }

        public async static Task<string> StringGetAsync(string key)
        {
            return await db.StringGetAsync(key);
        }

        public static T StringGet<T>(string key)
        {
            string value = StringGet(key);
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<T> StringGetAsync<T>(string key)
        {
            string value = await StringGetAsync(key);
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 消息推送及订阅
        /// <summary>
        /// 消息推送
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static void MessagePub(RedisChannel channel, RedisValue message)
        {
            ISubscriber sub = _redis.GetSubscriber();
            sub.PublishAsync(channel, message);
        }
        /// <summary>
        /// 消息订阅
        /// </summary>
        /// <param name="channel"></param>
        public async static void MessageSub(RedisChannel channel)
        {
            ISubscriber sub = _redis.GetSubscriber();
            await sub.SubscribeAsync(channel, (channel, message) =>
            {
                Console.WriteLine((string)message);
            });
        }
        /// <summary>
        /// 消息订阅 线程安全且按照队列方式接收处理消息
        /// </summary>
        /// <param name="channel"></param>
        public static void MessageSubWithSequential(RedisChannel channel)
        {
            ISubscriber sub = _redis.GetSubscriber();
            sub.Subscribe(channel).OnMessage(async channelMessage =>
            {
                await Task.Delay(1);
                Console.WriteLine((string)channelMessage.Message);
            });
        }
        #endregion
    }
}

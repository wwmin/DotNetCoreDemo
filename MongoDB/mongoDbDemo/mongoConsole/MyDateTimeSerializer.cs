using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace mongoConsole
{
    /// <summary>
    /// 修改mongodb中对于DateTime字段的序列化类
    /// </summary>
    public class MyDateTimeSerializer : DateTimeSerializer
    {
        public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var obj = base.Deserialize(context, args);
            return new DateTime(obj.Ticks, DateTimeKind.Unspecified);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
        {
            var utcValue = new DateTime(value.Ticks, DateTimeKind.Utc);
            base.Serialize(context, args, utcValue);
        }
    }
}

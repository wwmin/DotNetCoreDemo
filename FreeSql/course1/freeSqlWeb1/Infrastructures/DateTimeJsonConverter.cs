using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace freeSqlWeb1.Infrastructures
{
    /// <summary>
    /// 日期接口转化器,对DateTime序列化
    /// </summary>
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if (string.IsNullOrWhiteSpace(reader.GetString()))
                {
                    return DateTime.MinValue;
                }
                if (DateTime.TryParse(reader.GetString(), out DateTime date))
                {
                    return date;
                }
            }
            return reader.GetDateTime();
        }


        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            if (value.Kind == DateTimeKind.Utc)
            {
                value = value.AddHours(8);//东八区时间
                writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }
    }
}

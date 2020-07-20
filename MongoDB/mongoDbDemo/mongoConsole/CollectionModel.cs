using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace mongoConsole
{
    public class CollectionModel
    {
        [BsonId]
        public ObjectId topic_id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public int favor { get; set; }
        public Author author { get; set; }
        public TagEnumeration tag { get; set; }
        [BsonRepresentation(BsonType.String)]
        public TagEnumeration tagString { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime post_time { get; set; } 

        public DateTime post_time_serialize { get; set; }
    }

    public class Contact
    {
        public string mobile { get; set; }
    }

    public class Author
    {
        public string name { get; set; }
        public List<Contact> contacts { get; set; }
    }

    //枚举字段
    public enum TagEnumeration
    {
        CSharp = 1,
        Python = 2
    }

}

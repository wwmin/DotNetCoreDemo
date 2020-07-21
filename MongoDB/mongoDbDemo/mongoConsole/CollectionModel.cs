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
        public string content { get; set; }
        public int favor { get; set; }
        public Author author { get; set; }
        public TagEnumeration tag { get; set; }
        [BsonRepresentation(BsonType.String)]
        public TagEnumeration tagString { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]//MyDateTimeSerializer全局话后就不用对DateTime加特殊特性了
        public DateTime post_time { get; set; }
        //[BsonSerializer(typeof(MyDateTimeSerializer))]//或者将此出配置全部化,
        public DateTime post_time_serialize { get; set; }
        //保存字典，MongoDB定义了三种保存属性：Document、ArrayOfDocuments、ArrayOfArrays，默认是Document。
        /*
         * [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
         * public Dictionary<string, int> extra_info { get; set; }
         * 这三种属性下，保存在数据集中的数据结构有区别。
         * DictionaryRepresentation.Document：
         * { 
                "extra_info" : {
                    "type" : NumberInt(1), 
                    "mode" : NumberInt(2)
                }
            }
         * DictionaryRepresentation.ArrayOfDocuments：
         * 
         * { 
            "extra_info" : [
                    {
                        "k" : "type", 
                        "v" : NumberInt(1)
                    }, 
                    {
                        "k" : "mode", 
                        "v" : NumberInt(2)
                    }
                ]
            }
         * DictionaryRepresentation.ArrayOfArrays：
         * { 
            "extra_info" : [
                [
                    "type", 
                    NumberInt(1)
                ], 
                [
                    "mode", 
                    NumberInt(2)
                ]
            ]
          }
         * 这三种方式，从数据保存上并没有什么区别，但从查询来讲，如果这个字段需要进行查询，那三种方式区别很大。
         *  如果采用BsonDocument方式查询，DictionaryRepresentation.Document无疑是写着最方便的。
         * 如果用Builder方式查询，DictionaryRepresentation.ArrayOfDocuments是最容易写的。
         * DictionaryRepresentation.ArrayOfArrays就算了。数组套数组，查询条件写死人。
         * 我自己在使用时，多数情况用DictionaryRepresentation.ArrayOfDocuments。
         */
        public Dictionary<string, int> extra_info { get; set; }
        [BsonElement("element_name")]//这个属性用来改数据集中的字段名称用的,映射到数据库中字段即为'element_name'
        public string bson_element_name { get; set; }
        /// <summary>
        /// 测试 BsonDefaultValue("默认值")
        /// </summary>
        [BsonDefaultValue("This is a default title")]//并没有成功，使用方式不对？
        public string title { get; set; }
        [BsonRepresentation(BsonType.String)]//映射类中的数据类型和数据集中的数据类型做转换
        public int number_string { get; set; }
        [BsonIgnore]//忽略某些字段，映射类中不希望被保存到数据集中
        public string ignore_string { get; set; }
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

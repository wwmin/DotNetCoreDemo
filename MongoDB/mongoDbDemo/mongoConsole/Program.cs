using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace mongoConsole
{
    class Program
    {
        private const string MongoDBConnection = "mongodb://test:111111@localhost:27017/test";

        private static IMongoClient _client = new MongoClient(MongoDBConnection);
        private static IMongoDatabase _database = _client.GetDatabase("test");
        private static IMongoCollection<CollectionModel> _collection = _database.GetCollection<CollectionModel>("TestCollection");
        static async Task Main(string[] args)
        {
            BsonSerializer.RegisterSerializer(typeof(DateTime), new MyDateTimeSerializer());

            await Demo();
            Console.WriteLine("Hello World!");
        }

        private static async Task Demo()
        {
            CollectionModel new_item = new CollectionModel()
            {
                //title = "demo",//使用默认值
                content = "demo content",
                favor = 100,
                author = new Author
                {
                    name = "wwmin",
                    contacts = new System.Collections.Generic.List<Contact>()
                },
                tag = TagEnumeration.CSharp,
                tagString = TagEnumeration.CSharp,
                post_time = DateTime.Now,
                post_time_serialize = DateTime.Now,
                extra_info = new System.Collections.Generic.Dictionary<string, int> { { "name", 1 }, { "age", 2 } },
                bson_element_name = "test bson element name",
                number_string = 1,
                ignore_string = "ignore string"
            };
            Contact contact_item1 = new Contact()
            {
                mobile = "13000000000",
            };
            Contact contact_item2 = new Contact()
            {
                mobile = "13111111111"
            };
            new_item.author.contacts.Add(contact_item1);
            new_item.author.contacts.Add(contact_item2);
            await _collection.InsertOneAsync(new_item);
        }
    }
}

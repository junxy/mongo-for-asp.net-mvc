using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace MvcNews
{
    public class MongoDbContext
    {
        private static readonly MongoServer mongo = null;

        static MongoDbContext()
        {
            mongo = MongoServer.Create(new MongoServerSettings
            {
                SafeMode = SafeMode.False,
                Server = new MongoServerAddress("127.0.0.1", 27017)
            });
        }

        public static MongoDatabase GetDatabase()
        {
            return mongo.GetDatabase("ddd");
        }

        public static MongoCollection<T> GetCollection<T>(string collectionName)
        {
            return GetDatabase().GetCollection<T>(collectionName);
        }
        
    }
}
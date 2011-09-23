using System;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using MongoDB.Driver;

namespace MvcNews.Entities
{
    public class NewsModel
    {
        public ObjectId Id { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public DateTime PubDate { get; set; }

        public MongoDBRef Category { get; set; }


    }
}
using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace MvcNews.Entities
{
    public class NewsCategoryModel
    {

        public ObjectId Id { get; set; }

        public string CategoryName { get; set; }

        public ICollection<NewsModel> News { get; set; }

    }
}
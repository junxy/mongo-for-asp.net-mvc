using System;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MvcNews.Entities;
using MongoDB.Driver.Builders;
using System.Collections.Generic;

namespace MvcNews.Controllers
{
    public class NewsController : Controller
    {
        private MongoCollection<NewsCategoryModel> categories = MongoDbContext.GetCollection<NewsCategoryModel>("newscategories");
        private MongoCollection<NewsModel> news = MongoDbContext.GetCollection<NewsModel>("news");

        //
        // GET: /Home/

        public ActionResult Index(int p = 1)
        {
            if (!news.IndexExists(IndexKeys.Descending("PubDate")))
            {
                news.CreateIndex(IndexKeys.Descending("PubDate"));
            }

            var pageSize = 20;
            var skipCount = (p - 1) * pageSize;
            var models = news.Find(Query.Matches("Title", new BsonRegularExpression("e_")));

            models.SetSkip(skipCount);
            models.SetLimit(pageSize);
            models.SetSortOrder(SortBy.Descending("PubDate"));

            var count = models.Count();

            ViewBag.Count = count;
            ViewBag.PageCount = count / pageSize + ((count % pageSize > 0) ? 1 : 0);
            ViewBag.CurrentIndex = p;
            
            return View(models);
        }

        public ActionResult Create()
        {
            var categoriesList = categories.FindAll();

            ViewBag.categories = new SelectList(categoriesList, "Id", "CategoryName");

            var news = new NewsModel()
            {
                PubDate = DateTime.Now
            };
            return View(news);
        }

        [HttpPost]
        public ActionResult Create(NewsModel model, string categoryId)
        {
            try
            {
                model.Category = new MongoDBRef("newscategories", BsonValue.Create(categoryId));

                news.Insert(model);
                return RedirectToAction("Index");
            }
            catch
            {
                throw;
                //return View(model);
            }
        }

        public ActionResult Edit(string id)
        {
            var model = news.FindOneById(new ObjectId(id));
            var categoriesList = categories.FindAll();

            ViewBag.categories = new SelectList(categoriesList, "Id", "CategoryName", model.Category != null ? model.Category.Id.AsString : null);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(string id, string categoryId)
        {
            try
            {
                var newss = news.FindOneById(new ObjectId(id));
                TryUpdateModel(newss);

                newss.Category = new MongoDBRef("newscategories", BsonValue.Create(categoryId));

                news.Save(newss);
                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }

        public ActionResult Details(string id)
        {
            var model = news.FindOneById(new ObjectId(id));
            return View(model);
        }

        public ActionResult Delete(string id)
        {
            var model = news.FindOneById(new ObjectId(id));
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(string id, NewsCategoryModel o)
        {
            var model = news.FindAndRemove(Query.EQ("_id", ObjectId.Parse(id)), SortBy.Null);
            //return View(model);
            return RedirectToAction("Index");
        }

        public ActionResult InsertBatch()
        {

            var count = 1000000;
            var newsList = new List<NewsModel>(count);
            for (int i = 0; i < count; i++)
            {
                var n = new NewsModel()
                {
                    Id = ObjectId.GenerateNewId(),
                    Content = string.Format("g_content_{0}", i + 1),
                    PubDate = DateTime.Now,
                    Title = string.Format("g_title_{0}", i + 1),

                    //Category = new MongoDBRef("newscategories", categories.FindOne().Id)
                };
                newsList.Add(n);
            }

            news.InsertBatch(newsList);
            
            return Content("ok");

        }
    }
}
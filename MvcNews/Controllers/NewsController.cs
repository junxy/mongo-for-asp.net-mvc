using System;
using System.Web.Mvc;
using MongoDB.Driver;
using MvcNews.Entities;
using MongoDB.Bson;

namespace MvcNews.Controllers
{
    public class NewsController : Controller
    {
        private static readonly MongoServer mongo = null;

        static NewsController()
        {
            mongo = MongoServer.Create(new MongoServerSettings
            {
                SafeMode = SafeMode.False,
                Server = new MongoServerAddress("127.0.0.1", 27017)
            });
        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            var news = mongo.GetDatabase("ddd").GetCollection<NewsModel>("news").FindAll();
            return View(news);
        }

        public ActionResult Create()
        {
            var categories = mongo.GetDatabase("ddd").GetCollection<NewsCategoryModel>("newscategories").FindAll();

            ViewBag.categories = new SelectList(categories, "Id", "CategoryName");

            var news = new NewsModel()
            {
                PubDate = DateTime.Now
            };
            return View(news);
        }

        [HttpPost]
        public ActionResult Create(NewsModel model, string categoryId)
        {
            var news = mongo.GetDatabase("ddd").GetCollection<NewsModel>("news");
            
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
            var model = mongo.GetDatabase("ddd").GetCollection<NewsModel>("news").FindOneById(new ObjectId(id));
            var categories = mongo.GetDatabase("ddd").GetCollection<NewsCategoryModel>("newscategories").FindAll();

            ViewBag.categories = new SelectList(categories, "Id", "CategoryName", model.Category.Id.AsString);

            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(string id,string categoryId)
        {
            var news = mongo.GetDatabase("ddd").GetCollection<NewsModel>("news");
            
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

    }
}
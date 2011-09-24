using System;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MvcNews.Entities;
using MongoDB.Driver.Builders;

namespace MvcNews.Controllers
{
    public class NewsController : Controller
    {
        private MongoCollection<NewsCategoryModel> categories = MongoDbContext.GetCollection<NewsCategoryModel>("newscategories");
        private MongoCollection<NewsModel> news = MongoDbContext.GetCollection<NewsModel>("news");

        //
        // GET: /Home/

        public ActionResult Index()
        {
            var models = news.FindAll();
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

            ViewBag.categories = new SelectList(categoriesList, "Id", "CategoryName", model.Category.Id.AsString);

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
    }
}
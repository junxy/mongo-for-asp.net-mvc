using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcNews.Entities;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MvcNews.Controllers
{
    public class NewsCategoryController : Controller
    {
        private static readonly MongoServer mongo = null;

        static NewsCategoryController()
        {
            mongo = MongoServer.Create(new MongoServerSettings
            {
                SafeMode = SafeMode.False,
                Server = new MongoServerAddress("127.0.0.1", 27017)
            });
        }

        //
        // GET: /NewsCategory/

        public ActionResult Index()
        {
            var categories = mongo.GetDatabase("ddd").GetCollection<NewsCategoryModel>("newscategories").FindAll();
            return View(categories);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NewsCategoryModel model)
        {
            var categories = mongo.GetDatabase("ddd").GetCollection<NewsCategoryModel>("newscategories");
            try
            {
                categories.Insert(model);
                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }

        public ActionResult Edit(string id)
        {
            var model = mongo.GetDatabase("ddd").GetCollection<NewsCategoryModel>("newscategories").FindOneById(new ObjectId(id));
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(string id, NewsCategoryModel model)
        {
            var categories = mongo.GetDatabase("ddd").GetCollection<NewsCategoryModel>("newscategories");
            try
            {
                var category = categories.FindOneById(new ObjectId(id));
                TryUpdateModel(category);
                categories.Save(category);
                return RedirectToAction("Index");
            }
            catch
            {

                throw;
            }
        }

    }
}

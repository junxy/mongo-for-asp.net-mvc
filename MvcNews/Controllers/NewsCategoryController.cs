using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MvcNews.Entities;

namespace MvcNews.Controllers
{
    public class NewsCategoryController : Controller
    {
        private MongoCollection<NewsCategoryModel> categories = MongoDbContext.GetCollection<NewsCategoryModel>("newscategories");

        //
        // GET: /NewsCategory/

        public ActionResult Index()
        {
            var models = categories.FindAll();
            return View(models);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NewsCategoryModel model)
        {
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
            var model = categories.FindOneById(new ObjectId(id));
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(string id, NewsCategoryModel model)
        {
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

        public ActionResult Details(string id)
        {
            var model = categories.FindOneById(new ObjectId(id));
            return View(model);
        }

        public ActionResult Delete(string id)
        {
            var model = categories.FindOneById(new ObjectId(id));
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(string id, NewsCategoryModel o)
        {
            var model = categories.FindAndRemove(Query.EQ("_id", ObjectId.Parse(id)), SortBy.Null);
            //return View(model);
            return RedirectToAction("Index");
        }
    }
}
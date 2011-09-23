using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MongoDB.Bson.Serialization;
using MvcNews.Entities;

namespace MvcNews
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "News", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        void InitializeClassMaps()
        {
            BsonClassMap.RegisterClassMap<NewsModel>(
                n =>
                {
                    n.AutoMap();
                    var idMember = n.GetMemberMap(a => a.Id);
                    //idMember.SetRepresentation(MongoDB.Bson.BsonType.ObjectId);
                    n.SetIdMember(idMember);
                    
                }
            );

            BsonClassMap.RegisterClassMap<NewsCategoryModel>(
                n =>
                {
                    n.AutoMap();
                    var idMember = n.GetMemberMap(a => a.Id);
                    //idMember.SetRepresentation(MongoDB.Bson.BsonType.ObjectId);
                    n.SetIdMember(idMember);
                }
            );

        }

    }
}
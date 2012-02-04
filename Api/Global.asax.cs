using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Api
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "GetById",
                "get/{id}",
                new { controller = "StreamApi", action = "Get" }
            );

            routes.MapRoute(
                "GetByDate",
                "{type}",
                new { controller = "StreamApi", action = "Stream", type = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
namespace DotNetGroup.Api
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    using DotNetGroup.Services;

    using WebApiContrib.Formatting.Jsonp;

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "StreamApi",
                routeTemplate: "v1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            GlobalConfiguration.Configuration.Formatters.Insert(0, new JsonpMediaTypeFormatter());
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            new PeriodicStreamPersister(
                    ConfigurationManager.AppSettings["db.connection"],
                    ConfigurationManager.AppSettings["db.database"],
                    TimeSpan.Parse(ConfigurationManager.AppSettings["sys.refresh"] ?? "00:01:00")).Start();
        }
    }
}
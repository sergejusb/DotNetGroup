namespace DotNetGroup.Web
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using DotNetGroup.Services;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            StreamConfig.Poll();
        }

        public static class StreamConfig
        {
            public static void Poll()
            {
                var connectionString = ConfigurationManager.AppSettings["db.connection"];
                var database = ConfigurationManager.AppSettings["db.database"];
                var streamPersister = new StreamPersister(connectionString, database);
                var period = TimeSpan.Parse(ConfigurationManager.AppSettings["sys.refresh"] ?? "00:01:00");

                new StreamUpdater(streamPersister, period).Poll();
            }
        }
    }
}
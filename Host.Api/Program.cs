namespace Host.Api
{
    using System;
    using System.Configuration;
    using System.Web.Http;
    using System.Web.Http.Routing;
    using System.Web.Http.SelfHost;

    using Host.Api.Formatters;

    public class Program
    {
        public static void Main(string[] args)
        {
            var baseAddress = ConfigurationManager.AppSettings["host.address"];
            var config = new HttpSelfHostConfiguration(baseAddress);
            config.Formatters.Insert(0, new JsonpMediaTypeFormatter());

            var streamCountRouteDefaults = new HttpRouteValueDictionary
            {
                { "controller", "stream" },
                { "action", "count" }
            };
            config.Routes.Add("StreamCount", new HttpRoute("stream/count", streamCountRouteDefaults));

            var defaultRouteDefaults = new HttpRouteValueDictionary
            {
                { "controller", "stream" },
                { "id", RouteParameter.Optional }
            };
            config.Routes.Add("Default", new HttpRoute("{controller}/{id}", defaultRouteDefaults));

            var server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();
            
            Console.WriteLine("API is running in {0}...", baseAddress);
            Console.ReadKey();
        }
    }
}

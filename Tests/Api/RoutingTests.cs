using System.Web.Mvc;
using System.Web.Routing;
using Api;
using NUnit.Framework;
using Tests.Fakes;

namespace Tests.Api
{
    [TestFixture]
    public class RoutingTests
    {
        [Test]
        public void Given_Path_Is_Rss_Then_RssControler_Json_Action_With_No_Parameters_Is_Called()
        {
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            var context = new FakeHttpContext("~/rss");

            var routeData = routes.GetRouteData(context);

            Assert.AreEqual("rss", routeData.Values["controller"]);
            Assert.AreEqual("json", routeData.Values["action"]);
            Assert.AreEqual(UrlParameter.Optional, routeData.Values["id"]);
        }

        [Test]
        public void Given_Path_Is_Rss_Slash_Url_Then_RssControler_Json_Action_With_Id_Parameters_Is_Called()
        {
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            var context = new FakeHttpContext("~/rss/url");

            var routeData = routes.GetRouteData(context);

            Assert.AreEqual("rss", routeData.Values["controller"]);
            Assert.AreEqual("json", routeData.Values["action"]);
            Assert.AreEqual("url", routeData.Values["id"]);
        }

        [Test]
        public void Given_Path_Is_Rss_Dot_Json_Slash_Url_Then_RssControler_Json_Action_With_Id_Parameters_Is_Called()
        {
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            var context = new FakeHttpContext("~/rss.json/url");

            var routeData = routes.GetRouteData(context);

            Assert.AreEqual("rss", routeData.Values["controller"]);
            Assert.AreEqual("json", routeData.Values["action"]);
            Assert.AreEqual("url", routeData.Values["id"]);
        }

        [Test]
        public void Given_Path_Is_Twitter_TwitterControler_Json_Action_With_No_Parameters_Is_Called()
        {
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            var context = new FakeHttpContext("~/twitter");

            var routeData = routes.GetRouteData(context);

            Assert.AreEqual("twitter", routeData.Values["controller"]);
            Assert.AreEqual("json", routeData.Values["action"]);
            Assert.AreEqual(UrlParameter.Optional, routeData.Values["id"]);
        }

        [Test]
        public void Given_Path_Is_Twitter_Slash_Url_Then_TwitterControler_Json_Action_With_Id_Parameters_Is_Called()
        {
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            var context = new FakeHttpContext("~/twitter/url");

            var routeData = routes.GetRouteData(context);

            Assert.AreEqual("twitter", routeData.Values["controller"]);
            Assert.AreEqual("json", routeData.Values["action"]);
            Assert.AreEqual("url", routeData.Values["id"]);
        }

        [Test]
        public void Given_Path_Is_Twitter_Dot_Json_Slash_Url_Then_TwitterControler_Json_Action_With_Id_Parameters_Is_Called()
        {
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            var context = new FakeHttpContext("~/twitter.json/url");

            var routeData = routes.GetRouteData(context);

            Assert.AreEqual("twitter", routeData.Values["controller"]);
            Assert.AreEqual("json", routeData.Values["action"]);
            Assert.AreEqual("url", routeData.Values["id"]);
        }
    }
}

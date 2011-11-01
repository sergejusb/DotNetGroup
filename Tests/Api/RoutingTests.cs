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
            var routeData = GetRouteDataForUrl("~/rss");

            Assert.AreEqual("rss", routeData.Values["controller"]);
            Assert.AreEqual("json", routeData.Values["action"]);
            Assert.AreEqual(UrlParameter.Optional, routeData.Values["id"]);
        }

        [Test]
        public void Given_Path_Is_Rss_Slash_Url_Then_RssControler_Json_Action_With_Id_Parameters_Is_Called()
        {
            var routeData = GetRouteDataForUrl("~/rss/url");

            Assert.AreEqual("rss", routeData.Values["controller"]);
            Assert.AreEqual("json", routeData.Values["action"]);
            Assert.AreEqual("url", routeData.Values["id"]);
        }

        [Test]
        public void Given_Path_Is_Rss_Dot_Json_Slash_Url_Then_RssControler_Json_Action_With_Id_Parameters_Is_Called()
        {
            var routeData = GetRouteDataForUrl("~/rss.json/url");

            Assert.AreEqual("rss", routeData.Values["controller"]);
            Assert.AreEqual("json", routeData.Values["action"]);
            Assert.AreEqual("url", routeData.Values["id"]);
        }

        [Test]
        public void Given_Path_Is_Twitter_TwitterControler_Json_Action_With_No_Parameters_Is_Called()
        {
            var routeData = GetRouteDataForUrl("~/twitter");

            Assert.AreEqual("twitter", routeData.Values["controller"]);
            Assert.AreEqual("json", routeData.Values["action"]);
            Assert.AreEqual(UrlParameter.Optional, routeData.Values["id"]);
        }

        [Test]
        public void Given_Path_Is_Twitter_Slash_Url_Then_TwitterControler_Json_Action_With_Id_Parameters_Is_Called()
        {
            var routeData = GetRouteDataForUrl("~/twitter/url");

            Assert.AreEqual("twitter", routeData.Values["controller"]);
            Assert.AreEqual("json", routeData.Values["action"]);
            Assert.AreEqual("url", routeData.Values["id"]);
        }

        [Test]
        public void Given_Path_Is_Twitter_Dot_Json_Slash_Url_Then_TwitterControler_Json_Action_With_Id_Parameters_Is_Called()
        {
            var routeData = GetRouteDataForUrl("~/twitter.json/url");

            Assert.AreEqual("twitter", routeData.Values["controller"]);
            Assert.AreEqual("json", routeData.Values["action"]);
            Assert.AreEqual("url", routeData.Values["id"]);
        }

        private static RouteData GetRouteDataForUrl(string relativeUrl)
        {
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            var context = new FakeHttpContext(relativeUrl);
            return routes.GetRouteData(context);
        }
    }
}

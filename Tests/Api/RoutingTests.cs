namespace Tests.Api
{
    using System.Web.Routing;

    using global::Api;

    using NUnit.Framework;

    using Tests.Fakes;

    [TestFixture]
    public class RoutingTests
    {
        [Test]
        public void Given_Get_Slash_Id_Path_Then_StreamApi_Get_Action_Is_Called()
        {
            var routeData = GetRouteDataForUrl("~/get/1");

            Assert.AreEqual("StreamApi", routeData.Values["controller"]);
            Assert.AreEqual("Get", routeData.Values["action"]);
            Assert.AreEqual("1", routeData.Values["id"]);
        }

        [Test]
        public void Given_Root_Path_Then_StreamApi_Stream_Action_Is_Called()
        {
            var routeData = GetRouteDataForUrl("~/");

            Assert.AreEqual("StreamApi", routeData.Values["controller"]);
            Assert.AreEqual("Stream", routeData.Values["action"]);
        }

        [Test]
        public void Given_Root_Slash_Rss_Path_Then_StreamApi_Stream_Action_Is_Called()
        {
            var routeData = GetRouteDataForUrl("~/rss");

            Assert.AreEqual("StreamApi", routeData.Values["controller"]);
            Assert.AreEqual("Stream", routeData.Values["action"]);
            Assert.AreEqual("rss", routeData.Values["type"]);
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

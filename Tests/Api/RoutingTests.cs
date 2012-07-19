namespace DotNetGroup.Tests.Api
{
    using System.Web.Http;
    using System.Web.Routing;

    using DotNetGroup.Api;
    using DotNetGroup.Tests.Fakes;

    using NUnit.Framework;

    [TestFixture]
    public class RoutingTests
    {
        [Test]
        public void Given_Stream_Slash_Id_Path_Then_Stream_Get_Action_Is_Called()
        {
            var routeData = GetRouteDataForUrl("~/v1/stream/1");

            Assert.AreEqual("stream", routeData.Values["controller"]);
            Assert.AreEqual("1", routeData.Values["id"]);
        }

        [Test]
        public void Given_Stream_Path_Then_Stream_Get_Action_Is_Called()
        {
            var routeData = GetRouteDataForUrl("~/v1/stream");

            Assert.AreEqual("stream", routeData.Values["controller"]);
            Assert.AreEqual(RouteParameter.Optional, routeData.Values["id"]);
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

namespace Tests.Api
{
    using System.Web.Mvc;

    using global::Api.Controllers;

    using Mvc.Jsonp;

    using NUnit.Framework;

    [TestFixture]
    public class ApiControllerTests
    {
        [Test]
        public void Given_Callback_Is_Null_Json_Result_Is_Returned()
        {
            var controller = new FakeController();

            var result = controller.JsonOrJsonp(new object(), callback: null);

            Assert.IsInstanceOf<JsonResult>(result);
        }

        [Test]
        public void Given_Callback_Is_Provided_Jsonp_Result_Is_Returned()
        {
            var controller = new FakeController();

            var result = controller.JsonOrJsonp(new object(), "callback");

            Assert.IsInstanceOf<JsonpResult>(result);
        }

        private class FakeController : ApiController
        {
        }
    }
}

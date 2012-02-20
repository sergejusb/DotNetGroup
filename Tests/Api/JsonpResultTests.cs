namespace Tests.Api
{
    using System;
    using System.Collections.Specialized;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    using global::Api.Extensions;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class JsonpResultTests
    {
        private readonly object fakeData = new object();

        [Test]
        public void Given_Null_ControllerContext_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new JsonpResult().ExecuteResult(context: null));
        }

        [Test]
        public void Content_Type_Can_Be_Set()
        {
            var result = new JsonpResult
            {
                Data = this.fakeData,
                ContentType = "fakeType"
            };

            var fakeHttpResponse = ExecuteAndGetResponse(result);

            fakeHttpResponse.VerifySet(r => r.ContentType = result.ContentType);
            fakeHttpResponse.Verify(r => r.Write(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void Content_Type_Can_Be_Auto_Set()
        {
            var result = new JsonpResult
            {
                Data = this.fakeData
            };

            var fakeHttpResponse = ExecuteAndGetResponse(result);

            fakeHttpResponse.VerifySet(response => response.ContentType = "application/javascript");
            fakeHttpResponse.Verify(r => r.Write(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void Content_Encoding_Can_Be_Set()
        {
            var result = new JsonpResult
            {
                Data = this.fakeData,
                ContentEncoding = Encoding.UTF8
            };

            var fakeHttpResponse = ExecuteAndGetResponse(result);

            fakeHttpResponse.VerifySet(response => response.ContentEncoding = result.ContentEncoding);
            fakeHttpResponse.Verify(r => r.Write(It.IsAny<string>()), Times.Once());
        }

        private static Mock<HttpResponseBase> ExecuteAndGetResponse(JsonpResult result)
        {
            var fakeControllerContext = new Mock<ControllerContext>();
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeHttpRequest = new Mock<HttpRequestBase>();
            var fakeHttpResponse = new Mock<HttpResponseBase>();
            fakeHttpRequest.SetupGet(request => request.QueryString).Returns(new NameValueCollection { { "Callback ", "fakeCallback" } });

            fakeHttpContext.Setup(c => c.Request).Returns(fakeHttpRequest.Object);
            fakeHttpContext.Setup(c => c.Response).Returns(fakeHttpResponse.Object);
            fakeControllerContext.Setup(c => c.HttpContext).Returns(fakeHttpContext.Object);

            result.ExecuteResult(fakeControllerContext.Object);

            return fakeHttpResponse;
        }
    }
}

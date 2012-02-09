namespace Tests.Services.Web
{
    using System;

    using NUnit.Framework;

    using global::Services.Web;

    [TestFixture]
    public class UrlBuilderTests
    {
        private const string BaseUrl = "http://api.dotnetgroup.dev";

        [Test]
        public void Given_Null_Arguments_Constructor_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new UrlBuilder(null));
        }

        [Test]
        public void Given_Invalid_Base_Address_Constructor_Throws()
        {
            Assert.Throws<ArgumentException>(() => new UrlBuilder("invalid_url"));
        }

        [Test]
        public void Given_No_Arguments_Base_Url_With_Trailing_Slash_Is_Returned()
        {
            var expectedUrl = "http://api.dotnetgroup.dev/";

            var urlBuilder = new UrlBuilder(BaseUrl);

            var url = urlBuilder.Build();

            Assert.AreEqual(expectedUrl, url);
        }

        [Test]
        public void Given_Type_Argument_Correct_Url_Is_Generated()
        {
            var expectedUrl = "http://api.dotnetgroup.dev/?type=rss";

            var urlBuilder = new UrlBuilder(BaseUrl);
            urlBuilder.AddParameter("type", "rss");

            var url = urlBuilder.Build();

            Assert.AreEqual(expectedUrl, url);
        }

        [Test]
        public void Given_Date_From_And_Limit_Correct_Url_Is_Generated()
        {
            var expectedUrl = "http://api.dotnetgroup.dev/?from=2012-01-01%2010:00:00&limit=10";

            var urlBuilder = new UrlBuilder(BaseUrl);
            urlBuilder.AddParameter("from", "2012-01-01 10:00:00");
            urlBuilder.AddParameter("limit", 10);

            var url = urlBuilder.Build();

            Assert.AreEqual(expectedUrl, url);
        }

        [Test]
        public void Given_Get_And_Id_Parts_Correct_Url_Is_Generated()
        {
            var expectedUrl = "http://api.dotnetgroup.dev/get/1";

            var urlBuilder = new UrlBuilder(BaseUrl);
            urlBuilder.AddPart("get");
            urlBuilder.AddPart(1);

            var url = urlBuilder.Build();

            Assert.AreEqual(expectedUrl, url);
        }

        [Test]
        public void Given_Parts_To_WithPart_Correct_Url_Is_Generated()
        {            
            var url = new UrlBuilder(BaseUrl)
                .WithPart("get")
                .WithPart(1)
                .Build();            

            Assert.AreEqual("http://api.dotnetgroup.dev/get/1", url);
        }

        [Test]
        public void Given_Empty_Parameter_To_WithParamenter_Correct_Url_Is_Generated()
        {
            var url = new UrlBuilder(BaseUrl)
                .WithParameter("limit", null)
                .Build();

            Assert.AreEqual("http://api.dotnetgroup.dev/", url);
        }

        [Test]
        public void Given_Not_Empty_Parameter_To_WithParamenter_Correct_Url_Is_Generated()
        {
            var url = new UrlBuilder(BaseUrl)
                .WithParameter("limit", 10)
                .Build();

            Assert.AreEqual("http://api.dotnetgroup.dev/?limit=10", url);
        }
    }
}

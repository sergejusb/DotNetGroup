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
        public void Given_No_Parameter_Base_Url_With_Trailing_Slash_Is_Returned()
        {
            var url = new UrlBuilder(BaseUrl).Build();

            Assert.AreEqual("http://api.dotnetgroup.dev/", url);
        }

        [Test]
        public void Given_Empty_Parameter_Correct_Url_Is_Generated()
        {
            var url = new UrlBuilder(BaseUrl)
                            .WithParameter("limit", null)
                            .Build();

            Assert.AreEqual("http://api.dotnetgroup.dev/", url);
        }

        [Test]
        public void Given_Type_Parameter_Url_Is_Generated()
        {
            var url = new UrlBuilder(BaseUrl).WithParameter("type", "rss").Build();
            
            Assert.AreEqual("http://api.dotnetgroup.dev/?type=rss", url);
        }

        [Test]
        public void Given_Date_From_And_Limit_Parameter_Correct_Url_Is_Generated()
        {
            var url = new UrlBuilder(BaseUrl)
                            .WithParameter("from", "2012-01-01 10:00:00")
                            .WithParameter("limit", 10)
                            .Build();

            Assert.AreEqual("http://api.dotnetgroup.dev/?from=2012-01-01%2010:00:00&limit=10", url);
        }

        [Test]
        public void Given_Several_Parts_Correct_Url_Is_Generated()
        {            
            var url = new UrlBuilder(BaseUrl)
                            .WithPart("get")
                            .WithPart(1)
                            .Build();            

            Assert.AreEqual("http://api.dotnetgroup.dev/get/1", url);
        }
    }
}

namespace Tests.Services.Rss
{
    using System.Configuration;
    using System.Linq;

    using NUnit.Framework;

    using global::Services.Rss;

    [TestFixture]
    public class UrlConfigProviderTests
    {
        [Test]
        public void Given_AppConfig_Has_Rss_Url_GetValues_Successfully_Returens_It()
        {
            var numberOfRssUrls = 1;
            Assert.IsNotNullOrEmpty(ConfigurationManager.AppSettings["rss.sergejus"]);

            var urlProvider = new UrlConfigProvider();

            var urls = urlProvider.GetValues();

            Assert.IsNotNull(urls);
            Assert.AreEqual(numberOfRssUrls, urls.Count());
        }
    }
}

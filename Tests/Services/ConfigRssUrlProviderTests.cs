using System.Configuration;
using System.Linq;
using NUnit.Framework;
using Services.Rss;

namespace Tests.Services
{
    [TestFixture]
    public class ConfigRssUrlProviderTests
    {
        [Test]
        public void Given_AppConfig_Has_Rss_Url_GetUrls_Successfully_Returens_It()
        {
            var numberOfRssUrls = 1;
            Assert.IsNotNullOrEmpty(ConfigurationManager.AppSettings["rss.sergejus"]);

            var urlProvider = new ConfigRssUrlProvider();

            var urls = urlProvider.GetUrls();

            Assert.IsNotNull(urls);
            Assert.AreEqual(numberOfRssUrls, urls.Count());
        }
    }
}

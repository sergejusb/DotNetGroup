using System.Configuration;
using System.Linq;
using NUnit.Framework;
using Services.Rss;

namespace Tests.Services.Rss
{
    [TestFixture]
    public class RssServiceTests
    {
        [Test]
        public void GetFeeds_Can_Successfully_Retrieve_Values_From_Rss()
        {
            var url = ConfigurationManager.AppSettings["rss.sergejus"];
            var rssService = new RssService();

            var feeds = rssService.GetFeeds(url);

            Assert.Greater(feeds.Count(), 0);
        }

        [Test]
        public void GetFeeds_Returns_Empty_List_When_Internal_Exception_Happens()
        {
            var empty = 0;
            var url = "http://sergejus.blogas.lt/tag/fakeltnet/atom";
            var rssService = new RssService();

            var feeds = rssService.GetFeeds(url);

            Assert.AreEqual(empty, feeds.Count());
        }

        [Test]
        public void Given_Last_Feed_GetFeeds_Can_Successfully_Retrieve_Latest_Values_From_Rss()
        {
            var url = ConfigurationManager.AppSettings["rss.sergejus"];
            var rssService = new RssService();

            var feeds = rssService.GetFeeds(url).ToList();
            if (feeds.Count() > 1)
            {
                var lastFeed = feeds.Last();
                var latestFeeds = rssService.GetFeeds(url, lastFeed);

                Assert.AreEqual(feeds.Count(), latestFeeds.Count() + 1);
            }
        }
    }
}

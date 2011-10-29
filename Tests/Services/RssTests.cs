using System.Configuration;
using System.Linq;
using NUnit.Framework;
using Services.Rss;

namespace Tests.Services
{
    [TestFixture]
    public class RssTests
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
        public void GetFeeds_Returns_Empty_List_When_No_Results_Found()
        {
            var empty = 0;
            var url = "http://sergejus.blogas.lt/tag/fakeltnet/atom";
            var rssService = new RssService();

            var feeds = rssService.GetFeeds(url);

            Assert.AreEqual(empty, feeds.Count());
        }
    }
}

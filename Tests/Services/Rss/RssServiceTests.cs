using System;
using System.Configuration;
using System.Linq;
using NUnit.Framework;
using Services.Rss;

namespace Tests.Services.Rss
{
    [TestFixture]
    public class RssServiceTests
    {
        private DateTime _date;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            _date = DateTime.UtcNow.AddMonths(-2).Date;
        }

        [Test]
        public void GetFeeds_Can_Successfully_Retrieve_Values_From_Rss()
        {
            var url = ConfigurationManager.AppSettings["rss.sergejus"];
            var rssService = new RssService();

            var feeds = rssService.GetFeeds(url, _date);

            Assert.Greater(feeds.Count(), 0);
        }

        [Test]
        public void GetFeeds_Returns_Empty_List_When_Internal_Exception_Happens()
        {
            var empty = 0;
            var url = "http://sergejus.blogas.lt/tag/fakeltnet/atom";
            var rssService = new RssService();

            var feeds = rssService.GetFeeds(url, _date);

            Assert.AreEqual(empty, feeds.Count());
        }

        [Test]
        public void Given_Last_Feed_Date_GetFeeds_Can_Successfully_Retrieve_Latest_Values_From_Rss()
        {
            var url = ConfigurationManager.AppSettings["rss.sergejus"];
            var rssService = new RssService();

            var feeds = rssService.GetFeeds(url, _date).ToList();
            if (feeds.Count() > 1)
            {
                var fromDate = feeds.Last().Published;
                var latestFeeds = rssService.GetFeeds(url, fromDate);

                Assert.That(feeds.Count(), Is.GreaterThan(latestFeeds.Count()));
            }
        }
    }
}

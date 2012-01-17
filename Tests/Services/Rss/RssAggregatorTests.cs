using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Services.Generic;
using Services.Model;
using Services.Rss;

namespace Tests.Services.Rss
{
    [TestFixture]
    public class RssAggregatorTests
    {
        [Test]
        public void RssAggregator_Can_Be_Successfully_Created_With_Default_Constructor()
        {
            new RssAggregator();
        }

        [Test]
        public void Given_30_New_Feeds_GetLatest_Returns_Feeds_In_Correct_Order()
        {
            var urlFeeds = new Dictionary<string, IEnumerable<Item>>
            {
                {"http://atom1", BuildFeeds(5)},
                {"http://atom2", BuildFeeds(10)},
                {"http://atom3", BuildFeeds(15)}
            };
            var minDate = urlFeeds.SelectMany(kv => kv.Value).Select(f => f.Published).Min();
            var maxDate = urlFeeds.SelectMany(kv => kv.Value).Select(f => f.Published).Max();
            var rssAggregator = BuildRssAggregator(urlFeeds);

            var feeds = rssAggregator.GetLatest(DateTime.MinValue).ToList();

            Assert.AreEqual(minDate, feeds.First().Published);
            Assert.AreEqual(maxDate, feeds.Last().Published);
        }

        [Test]
        public void Given_30_New_Feeds_GetLatest_Returns_All_30_Feeds()
        {
            var numberOfFeeds = 30;
            var urlFeeds = new Dictionary<string, IEnumerable<Item>>
            {
                {"http://atom1", BuildFeeds(5)},
                {"http://atom2", BuildFeeds(10)},
                {"http://atom3", BuildFeeds(15)}
            };
            var rssAggregator = BuildRssAggregator(urlFeeds);

            var feeds = rssAggregator.GetLatest(DateTime.MinValue).ToList();

            Assert.AreEqual(numberOfFeeds, feeds.Count);
        }

        private static IItemAggregator BuildRssAggregator(IDictionary<string, IEnumerable<Item>> urlFeeds)
        {
            var rssServiceFake = new Mock<IRssService>();
            foreach (var urlFeed in urlFeeds)
            {
                var feed = urlFeed;
                rssServiceFake.Setup(s => s.GetFeeds(feed.Key, It.IsAny<DateTime>())).Returns(feed.Value);
            }
            var rssUrlProviderFake = new Mock<IConfigProvider>();
            rssUrlProviderFake.Setup(p => p.GetValues()).Returns(urlFeeds.Keys);
            return new RssAggregator(rssServiceFake.Object, rssUrlProviderFake.Object);
        }

        private static IEnumerable<Item> BuildFeeds(int numberOfFeeds)
        {
            return new Fixture().Build<Item>()
                                .Do(f => f.Published = DateTime.Now.AddDays(new Random().Next(numberOfFeeds)).AddHours(new Random().Next(numberOfFeeds)))
                                .CreateMany(numberOfFeeds)
                                .ToList();
        }
    }
}

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
        public void Given_30_Existing_Feeds_GetLatestFeeds_Returns_Feeds_In_Correct_Order()
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

            var feeds = rssAggregator.GetLatest(5 + 10 + 15).ToList();

            Assert.AreEqual(minDate, feeds.Last().Published);
            Assert.AreEqual(maxDate, feeds.First().Published);
        }

        [Test]
        public void Given_30_Existing_Feeds_GetLatestFeeds_With_Maximum_Number_Of_100_Feeds_Returns_30_Feeds()
        {
            var maximumNumberOfFeeds = 100;
            var urlFeeds = new Dictionary<string, IEnumerable<Item>>
            {
                {"http://atom1", BuildFeeds(5)},
                {"http://atom2", BuildFeeds(10)},
                {"http://atom3", BuildFeeds(15)}
            };
            var rssAggregator = BuildRssAggregator(urlFeeds);

            var feeds = rssAggregator.GetLatest(maximumNumberOfFeeds).ToList();

            Assert.AreEqual(5 + 10 + 15, feeds.Count);
        }

        [Test]
        public void Given_30_Existing_Feeds_GetLatestFeeds_With_Maximum_Number_Of_10_Feeds_Returns_10_Feeds()
        {
            var maximumNumberOfFeeds = 10;
            var urlFeeds = new Dictionary<string, IEnumerable<Item>>
            {
                {"http://atom1", BuildFeeds(5)},
                {"http://atom2", BuildFeeds(10)},
                {"http://atom3", BuildFeeds(15)}
            };
            var rssAggregator = BuildRssAggregator(urlFeeds);

            var feeds = rssAggregator.GetLatest(maximumNumberOfFeeds).ToList();

            Assert.AreEqual(maximumNumberOfFeeds, feeds.Count);
        }

        [Test]
        public void Given_30_Existing_Feeds_And_Since_Latest_Feed_Url_10_New_Feeds_Exist_GetLatestFeeds_Returns_10_Feeds()
        {
            var numberOfLatestFeeds = 10;
            var feeds = BuildFeeds(30);
            var latestFeed = feeds.OrderByDescending(f => f.Published).Skip(numberOfLatestFeeds).Take(1).Single();
            var urlFeeds = new Dictionary<string, IEnumerable<Item>>
            {
                {"http://atom1", feeds}
            };
            var rssAggregator = BuildRssAggregator(urlFeeds);

            var latestFeeds = rssAggregator.GetLatest(latestFeed.Url, feeds.Count());

            Assert.AreEqual(numberOfLatestFeeds, latestFeeds.Count());
        }

        [Test]
        public void Given_30_Existing_Feeds_And_Since_Latest_Feed_Url_25_New_Feeds_Exist_GetLatestFeeds_With_Maximum_Number_Of_25_Feeds_Returns_25_Feeds()
        {
            var maximumNumberOfFeeds = 25;
            var feeds = BuildFeeds(30);
            var latestFeed = feeds.OrderByDescending(f => f.Published).Skip(maximumNumberOfFeeds).Take(1).Single();
            var urlFeeds = new Dictionary<string, IEnumerable<Item>>
            {
                {"http://atom1", feeds}
            };
            var rssAggregator = BuildRssAggregator(urlFeeds);

            var latestFeeds = rssAggregator.GetLatest(latestFeed.Url, feeds.Count());

            Assert.AreEqual(maximumNumberOfFeeds, latestFeeds.Count());
        }

        private static IItemAggregator BuildRssAggregator(IDictionary<string, IEnumerable<Item>> urlFeeds)
        {
            var rssServiceFake = new Mock<IRssService>();
            foreach (var urlFeed in urlFeeds)
            {
                var feed = urlFeed;
                rssServiceFake.Setup(s => s.GetFeeds(feed.Key)).Returns(feed.Value);
            }
            var rssUrlProviderFake = new Mock<IConfigProvider>();
            rssUrlProviderFake.Setup(p => p.GetValues()).Returns(urlFeeds.Keys);
            return new RssAggregator(rssServiceFake.Object, rssUrlProviderFake.Object);
        }

        private static IList<Item> BuildFeeds(int numberOfFeeds)
        {
            return new Fixture().Build<Item>()
                                .Do(f => f.Published = DateTime.Now.AddDays(new Random().Next(numberOfFeeds)).AddHours(new Random().Next(numberOfFeeds)))
                                .CreateMany(numberOfFeeds)
                                .ToList();
        }
    }
}

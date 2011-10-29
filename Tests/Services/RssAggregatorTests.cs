using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Services.Rss;

namespace Tests.Services
{
    [TestFixture]
    public class RssAggregatorTests
    {
        [Test]
        public void RssAggregator_Can_Be_Successfully_Created_With_Default_Constructor()
        {
            var urls = new[] { "http://atom1", "http://atom2", "http://atom3" };
            new RssAggregator(urls);
        }

        [Test]
        public void Given_30_Existing_Feeds_GetLatestFeeds_Returns_Feeds_In_Correct_Order()
        {
            var urlFeeds = new Dictionary<string, IEnumerable<Feed>>
            {
                {"http://atom1", BuildFeeds(5)},
                {"http://atom2", BuildFeeds(10)},
                {"http://atom3", BuildFeeds(15)}
            };
            var minDate = urlFeeds.SelectMany(kv => kv.Value).Select(f => f.Published).Min();
            var maxDate = urlFeeds.SelectMany(kv => kv.Value).Select(f => f.Published).Max();
            var rssAggregator = BuildRssAggregator(urlFeeds);

            var feeds = rssAggregator.GetLatestFeeds(5 + 10 + 15).ToList();

            Assert.AreEqual(minDate, feeds.Last().Published);
            Assert.AreEqual(maxDate, feeds.First().Published);
        }

        [Test]
        public void Given_30_Existing_Feeds_GetLatestFeeds_With_Maximum_Number_Of_100_Feeds_Returns_30_Feeds()
        {
            var maximumNumberOfFeeds = 100;
            var urlFeeds = new Dictionary<string, IEnumerable<Feed>>
            {
                {"http://atom1", BuildFeeds(5)},
                {"http://atom2", BuildFeeds(10)},
                {"http://atom3", BuildFeeds(15)}
            };
            var rssAggregator = BuildRssAggregator(urlFeeds);

            var feeds = rssAggregator.GetLatestFeeds(maximumNumberOfFeeds).ToList();

            Assert.AreEqual(5 + 10 + 15, feeds.Count);
        }

        [Test]
        public void Given_30_Existing_Feeds_GetLatestFeeds_With_Maximum_Number_Of_10_Feeds_Returns_10_Feeds()
        {
            var maximumNumberOfFeeds = 10;
            var urlFeeds = new Dictionary<string, IEnumerable<Feed>>
            {
                {"http://atom1", BuildFeeds(5)},
                {"http://atom2", BuildFeeds(10)},
                {"http://atom3", BuildFeeds(15)}
            };
            var rssAggregator = BuildRssAggregator(urlFeeds);

            var feeds = rssAggregator.GetLatestFeeds(maximumNumberOfFeeds).ToList();

            Assert.AreEqual(maximumNumberOfFeeds, feeds.Count);
        }

        private static IRssAggregator BuildRssAggregator(IDictionary<string, IEnumerable<Feed>> urlFeeds)
        {
            var rssServiceFake = new Mock<IRssService>();
            foreach (var urlFeed in urlFeeds)
            {
                var feed = urlFeed;
                rssServiceFake.Setup(s => s.GetFeeds(feed.Key)).Returns(feed.Value);
            }
            var rssService = rssServiceFake.Object;
            return new RssAggregator(rssService, urlFeeds.Keys);
        }

        private static IEnumerable<Feed> BuildFeeds(int numberOfFeeds)
        {
            return new Fixture().Build<Feed>()
                                .Do(f => f.Published = DateTime.Now.AddDays(new Random().Next(numberOfFeeds)).AddHours(new Random().Next(numberOfFeeds)))
                                .CreateMany(numberOfFeeds).ToList();
        }
    }
}

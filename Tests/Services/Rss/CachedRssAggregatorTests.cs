using System;
using System.Collections.Generic;
using System.Threading;
using Moq;
using NUnit.Framework;
using Services.Generic;
using Services.Model;
using Services.Rss;

namespace Tests.Services.Rss
{
    [TestFixture]
    public class CachedRssAggregatorTests
    {
        [Test]
        public void CachedRssAggregator_Can_Be_Successfully_Created_Using_1_Second_Cache_Period()
        {
            var oneSecond = TimeSpan.FromSeconds(1);
            var rssAggregator = new RssAggregator();
            new CachedItemAggregator(rssAggregator, oneSecond);
        }

        [Test]
        public void Given_5_Seconds_Cache_Period_And_Fact_GetLatestFeeds_Is_Called_3_Times_GetUrls_Is_Called_Only_Once()
        {
            var numberOfCalls = 3;
            var numberOfItems = 20;
            var fiveSeconds = TimeSpan.FromSeconds(5);
            var rssServiceFake = GetRssServiceFake();
            var urlProviderFake = GetUrlProviderFake();
            var rssAggregator = new RssAggregator(rssServiceFake.Object, urlProviderFake.Object);
            var aggregator = new CachedItemAggregator(rssAggregator, fiveSeconds);

            for (var i = 0; i < numberOfCalls; i++)
            {
                aggregator.GetLatest(numberOfItems);                
            }

            urlProviderFake.Verify(p => p.GetValues(), Times.Once());
        }

        [Test]
        public void Given_1_Second_Cache_Period_And_Fact_GetLatestFeeds_Is_Called_2_Times_GetUrls_Is_Called_Twice()
        {
            var numberOfCalls = 2;
            var numberOfItems = 20;
            var oneSecond = TimeSpan.FromSeconds(1);
            var rssServiceFake = GetRssServiceFake();
            var urlProviderFake = GetUrlProviderFake();
            var rssAggregator = new RssAggregator(rssServiceFake.Object, urlProviderFake.Object);
            var aggregator = new CachedItemAggregator(rssAggregator, oneSecond);

            for (var i = 0; i < numberOfCalls; i++)
            {
                aggregator.GetLatest(numberOfItems);
                Thread.Sleep(oneSecond);
            }

            urlProviderFake.Verify(p => p.GetValues(), Times.Exactly(numberOfCalls));
        }

        private static Mock<IRssService> GetRssServiceFake()
        {
            var rssServiceFake = new Mock<IRssService>();
            rssServiceFake.Setup(s => s.GetFeeds(It.IsAny<string>())).Returns(new List<Item>());
            return rssServiceFake;
        }

        private static Mock<IConfigProvider> GetUrlProviderFake()
        {
            var urlProviderFake = new Mock<IConfigProvider>();
            urlProviderFake.Setup(p => p.GetValues()).Returns(new List<string>());
            return urlProviderFake;
        }
    }
}
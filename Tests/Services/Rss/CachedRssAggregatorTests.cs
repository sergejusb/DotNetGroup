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
        public void CachedRssAggregator_Can_Be_Successfully_Created()
        {
            var rssAggregator = new RssAggregator();
            new CachedItemAggregator(rssAggregator);
        }

        [Test]
        public void Given_Fact_GetLatestFeeds_Is_Called_3_Times_GetUrls_Is_Called_Only_Once()
        {
            var numberOfCalls = 3;
            var rssServiceFake = GetRssServiceFake();
            var urlProviderFake = GetUrlProviderFake();
            var rssAggregator = new RssAggregator(rssServiceFake.Object, urlProviderFake.Object);
            var aggregator = new CachedItemAggregator(rssAggregator);

            for (var i = 0; i < numberOfCalls; i++)
            {
                aggregator.GetLatest();                
            }

            urlProviderFake.Verify(p => p.GetValues(), Times.Once());
        }

        private static Mock<IRssService> GetRssServiceFake()
        {
            var rssServiceFake = new Mock<IRssService>();
            rssServiceFake.Setup(s => s.GetFeeds(It.IsAny<string>(), It.IsAny<Item>())).Returns(new List<Item>());
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
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Services.Generic;
using Services.Model;
using Services.Twitter;

namespace Tests.Services.Twitter
{
    [TestFixture]
    public class CachedTwitterAggregatorTests
    {
        [Test]
        public void CachedTwitterAggregator_Can_Be_Successfully_Created()
        {
            var twitterAggregator = new TwitterAggregator();
            new CachedItemAggregator(twitterAggregator);
        }

        [Test]
        public void Given_Fact_GetLatestTweets_Is_Called_3_Times_GetTweets_Is_Called_Only_Once()
        {
            var numberOfCalls = 3;
            var twitterServiceFake = GetTwitterServiceFake();
            var queryProviderFake = GetQueryProviderFake();
            var twitterAggregator = new TwitterAggregator(twitterServiceFake.Object, queryProviderFake.Object);
            var aggregator = new CachedItemAggregator(twitterAggregator);

            for (var i = 0; i < numberOfCalls; i++)
            {
                aggregator.GetLatest();
            }

            twitterServiceFake.Verify(s => s.GetTweets(It.IsAny<string>()), Times.Once());
        }

        private static Mock<ITwitterService> GetTwitterServiceFake()
        {
            var twitterServiceFake = new Mock<ITwitterService>();
            twitterServiceFake.Setup(s => s.GetTweets(It.IsAny<string>())).Returns(new List<Item>());
            return twitterServiceFake;
        }

        private static Mock<IConfigProvider> GetQueryProviderFake()
        {
            var urlProviderFake = new Mock<IConfigProvider>();
            urlProviderFake.Setup(p => p.GetValues()).Returns(new List<string> { "%23ltnet+-ltnet.tv" });
            return urlProviderFake;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
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
        public void CachedTwitterAggregator_Can_Be_Successfully_Created_Using_1_Second_Cache_Period()
        {
            var oneSecond = TimeSpan.FromSeconds(1);
            var twitterAggregator = new TwitterAggregator();
            new CachedItemAggregator(twitterAggregator, oneSecond);
        }

        [Test]
        public void Given_5_Seconds_Cache_Period_And_Fact_GetLatestTweets_Is_Called_3_Times_GetTweets_Is_Called_Only_Once()
        {
            var numberOfCalls = 3;
            var maxNumberOfTweets = 30;
            var fiveSeconds = TimeSpan.FromSeconds(5);
            var twitterServiceFake = GetTwitterServiceFake();
            var queryProviderFake = GetQueryProviderFake();
            var twitterAggregator = new TwitterAggregator(twitterServiceFake.Object, queryProviderFake.Object);
            var aggregator = new CachedItemAggregator(twitterAggregator, fiveSeconds);

            for (var i = 0; i < numberOfCalls; i++)
            {
                aggregator.GetLatest(maxNumberOfTweets);
            }

            twitterServiceFake.Verify(s => s.GetTweets(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void Given_1_Seconds_Cache_Period_And_Fact_GetLatestTweets_Is_Called_2_Times_GetTweets_Is_Called_Twice()
        {
            var numberOfCalls = 2;
            var maxNumberOfTweets = 30;
            var oneSecond = TimeSpan.FromSeconds(1);
            var twitterServiceFake = GetTwitterServiceFake();
            var queryProviderFake = GetQueryProviderFake();
            var twitterAggregator = new TwitterAggregator(twitterServiceFake.Object, queryProviderFake.Object);
            var aggregator = new CachedItemAggregator(twitterAggregator, oneSecond);

            for (var i = 0; i < numberOfCalls; i++)
            {
                aggregator.GetLatest(maxNumberOfTweets);
                Thread.Sleep(oneSecond);
            }

            twitterServiceFake.Verify(s => s.GetTweets(It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(numberOfCalls));
        }

        private static Mock<ITwitterService> GetTwitterServiceFake()
        {
            var twitterServiceFake = new Mock<ITwitterService>();
            twitterServiceFake.Setup(s => s.GetTweets(It.IsAny<string>(), It.IsAny<int>())).Returns(new List<Item>());
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

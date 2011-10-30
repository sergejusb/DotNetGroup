using System;
using System.Collections.Generic;
using System.Threading;
using Moq;
using NUnit.Framework;
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
            new CachedTwitterAggregator(oneSecond);
        }

        [Test]
        public void Given_5_Seconds_Cache_Period_And_Fact_GetLatestTweets_Is_Called_3_Times_GetTweets_Is_Called_Only_Once()
        {
            var numberOfCalls = 3;
            var fiveSeconds = TimeSpan.FromSeconds(5);
            var twitterServiceFake = GetTwitterServiceFake();
            var aggregator = new CachedTwitterAggregator(twitterServiceFake.Object, fiveSeconds);

            for (var i = 0; i < numberOfCalls; i++)
            {
                aggregator.GetLatestTweets();
            }

            twitterServiceFake.Verify(s => s.GetTweets(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void Given_1_Seconds_Cache_Period_And_Fact_GetLatestTweets_Is_Called_2_Times_GetTweets_Is_Called_Twice()
        {
            var numberOfCalls = 2;
            var oneSecond = TimeSpan.FromSeconds(1);
            var twitterServiceFake = GetTwitterServiceFake();
            var aggregator = new CachedTwitterAggregator(twitterServiceFake.Object, oneSecond);

            for (var i = 0; i < numberOfCalls; i++)
            {
                aggregator.GetLatestTweets();
                Thread.Sleep(oneSecond);
            }

            twitterServiceFake.Verify(s => s.GetTweets(It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(numberOfCalls));
        }

        private static Mock<ITwitterService> GetTwitterServiceFake()
        {
            var twitterServiceFake = new Mock<ITwitterService>();
            twitterServiceFake.Setup(s => s.GetTweets(It.IsAny<string>(), It.IsAny<int>())).Returns(new List<Tweet>());
            return twitterServiceFake;
        }
    }
}

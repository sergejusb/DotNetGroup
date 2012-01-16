using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Services.Generic;
using Services.Model;
using Services.Twitter;

namespace Tests.Services.Twitter
{
    [TestFixture]
    public class TwitterAggregatorTests
    {
        [Test]
        public void TwitterAggregator_Can_Be_Successfully_Created_With_Default_Constructor()
        {
            new TwitterAggregator();
        }

        [Test]
        public void Given_30_Existing_Tweets_And_Since_Latest_Tweet_Url_10_New_Tweets_Exist_GetLatestTweets_Returns_10_Tweets()
        {
            var numberOfLatestTweets = 10;
            var tweets = BuildTweets(30);
            var latestTweet = tweets.OrderByDescending(t => t.Published).Skip(numberOfLatestTweets).Take(1).Single();
            var twitterServiceFake = GetTwitterServiceFake(tweets);
            var queryProviderFake = GetQueryProviderFake();
            var twitterAggregator = new TwitterAggregator(twitterServiceFake.Object, queryProviderFake.Object);

            var latestTweets = twitterAggregator.GetLatest(latestTweet.Url);

            twitterServiceFake.Verify(s => s.GetTweets(It.IsAny<string>(), It.IsAny<Item>()));
            Assert.AreEqual(numberOfLatestTweets, latestTweets.Count());
        }

        [Test]
        public void Given_30_Existing_Tweets_GetLatestTweets_Returns_All_Tweets_When_Null_Is_Passed()
        {
            var tweets = BuildTweets(30);
            var twitterServiceFake = GetTwitterServiceFake(tweets);
            var queryProviderFake = GetQueryProviderFake();
            var twitterAggregator = new TwitterAggregator(twitterServiceFake.Object, queryProviderFake.Object);

            var latestTweets = twitterAggregator.GetLatest();

            Assert.AreEqual(tweets.Count, latestTweets.Count());
        }

        private static IList<Item> BuildTweets(int numberOfTweets)
        {
            return new Fixture().Build<Item>()
                                .Do(f => f.Published = DateTime.Now.AddDays(new Random().Next(numberOfTweets)).AddHours(new Random().Next(numberOfTweets)))
                                .CreateMany(numberOfTweets)
                                .OrderByDescending(t => t.Published)
                                .ToList();
        }

        private static Mock<ITwitterService> GetTwitterServiceFake(IEnumerable<Item> tweets)
        {
            var twitterServiceFake = new Mock<ITwitterService>();
            twitterServiceFake.Setup(s => s.GetTweets(It.IsAny<string>(), It.IsAny<Item>())).Returns(tweets);
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

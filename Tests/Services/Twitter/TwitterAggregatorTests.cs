using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
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
        public void GetLatestTweets_Filters_Out_Ltnettv_Text()
        {
            var numberOfGoodTweets = 10;
            var maxNumberOfTweets = 20;
            var tweets = BuildTweets(numberOfGoodTweets).ToList();
            tweets.Add(new Fixture().Build<Tweet>().With(t => t.Content, "test ltnet.tv test").CreateAnonymous());
            var twitterServiceFake = GetTwitterServiceFake(tweets);
            var twitterAggregator = new TwitterAggregator(twitterServiceFake.Object);

            var filteredTweets = twitterAggregator.GetLatestTweets(maxNumberOfTweets);

            twitterServiceFake.Verify(s => s.GetTweets(It.IsAny<string>(), maxNumberOfTweets), Times.Once());
            Assert.AreEqual(numberOfGoodTweets, filteredTweets.Count());
        }

        [Test]
        public void Given_30_Existing_Tweets_And_Since_Latest_Tweet_Url_10_New_Tweets_Exist_GetLatestTweets_With_Maximum_Number_Of_20_Tweets_Returns_10_Tweets()
        {
            var numberOfLatestTweets = 10;
            var maxNumberOfTweets = 20;
            var tweets = BuildTweets(30).ToList();
            var latestTweet = tweets.OrderByDescending(t => t.Published).Skip(numberOfLatestTweets).Take(1).Single();
            var twitterServiceFake = GetTwitterServiceFake(tweets);
            var twitterAggregator = new TwitterAggregator(twitterServiceFake.Object);

            var latestTweets = twitterAggregator.GetLatestTweets(latestTweet.Url, maxNumberOfTweets);

            twitterServiceFake.Verify(s => s.GetTweets(It.IsAny<string>(), maxNumberOfTweets));
            Assert.AreEqual(numberOfLatestTweets, latestTweets.Count());
        }
        
        private static IEnumerable<Tweet> BuildTweets(int numberOfTweets)
        {
            return new Fixture().Build<Tweet>()
                                .Do(f => f.Published = DateTime.Now.AddDays(new Random().Next(numberOfTweets)).AddHours(new Random().Next(numberOfTweets)))
                                .CreateMany(numberOfTweets)
                                .OrderByDescending(t => t.Published)
                                .ToList();
        }

        private static Mock<ITwitterService> GetTwitterServiceFake(IEnumerable<Tweet> tweets)
        {
            var twitterServiceFake = new Mock<ITwitterService>();
            twitterServiceFake.Setup(s => s.GetTweets(It.IsAny<string>(), It.IsAny<int>())).Returns(tweets);
            return twitterServiceFake;
        }
    }
}

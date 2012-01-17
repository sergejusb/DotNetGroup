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
        public void Given_30_New_Tweets_GetLatest_Returns_Tweets_In_Correct_Order()
        {
            var queryTweets = new Dictionary<string, IEnumerable<Item>>
            {
                {"#hashtag", BuildTweets(5)},
                {"@user", BuildTweets(10)},
                {"text", BuildTweets(15)}
            };
            var minDate = queryTweets.SelectMany(kv => kv.Value).Select(f => f.Published).Min();
            var maxDate = queryTweets.SelectMany(kv => kv.Value).Select(f => f.Published).Max();
            var twitterAggregator = BuildTwitterAggregator(queryTweets);

            var tweets = twitterAggregator.GetLatest(DateTime.MinValue).ToList();

            Assert.AreEqual(minDate, tweets.First().Published);
            Assert.AreEqual(maxDate, tweets.Last().Published);
        }

        [Test]
        public void Given_30_New_Tweets_GetLatest_Returns_All_30_Tweets()
        {
            var numberOfTweets = 30;
            var queryTweets = new Dictionary<string, IEnumerable<Item>>
            {
                {"#hashtag", BuildTweets(5)},
                {"@user", BuildTweets(10)},
                {"text", BuildTweets(15)}
            };
            var twitterAggregator = BuildTwitterAggregator(queryTweets);

            var tweets = twitterAggregator.GetLatest(DateTime.MinValue).ToList();

            Assert.AreEqual(numberOfTweets, tweets.Count);
        }

        private static IItemAggregator BuildTwitterAggregator(IDictionary<string, IEnumerable<Item>> queryTweets)
        {
            var twitterServiceFake = new Mock<ITwitterService>();
            foreach (var queryTweet in queryTweets)
            {
                var tweet = queryTweet;
                twitterServiceFake.Setup(s => s.GetTweets(tweet.Key, It.IsAny<DateTime>())).Returns(tweet.Value);
            }
            var urlProviderFake = new Mock<IConfigProvider>();
            urlProviderFake.Setup(p => p.GetValues()).Returns(queryTweets.Keys);
            return new TwitterAggregator(twitterServiceFake.Object, urlProviderFake.Object);
        }

        private static IList<Item> BuildTweets(int numberOfTweets)
        {
            return new Fixture().Build<Item>()
                                .Do(f => f.Published = DateTime.Now.AddDays(new Random().Next(numberOfTweets)).AddHours(new Random().Next(numberOfTweets)))
                                .CreateMany(numberOfTweets)
                                .OrderByDescending(t => t.Published)
                                .ToList();
        }
    }
}

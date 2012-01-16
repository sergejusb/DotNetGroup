using System.Linq;
using NUnit.Framework;
using Services.Twitter;

namespace Tests.Services.Twitter
{
    [TestFixture]
    public class TwitterServiceTests
    {
        [Test]
        public void GetTweets_Can_Successfully_Retrieve_Values_From_Twitter()
        {
            var query = "#ltnet";
            var twitterService = new TwitterService();

            var tweets = twitterService.GetTweets(query);

            Assert.Greater(tweets.Count(), 0);
        }

        [Test]
        public void GetTweers_Returns_Empty_List_When_No_Results_Found()
        {
            var empty = 0;
            var query = "#hashtagfortesting";
            var twitterService = new TwitterService();

            var tweets = twitterService.GetTweets(query);

            Assert.AreEqual(empty, tweets.Count());
        }

        [Test]
        public void GetTweers_Returns_Empty_List_When_Internal_Exception_Happens()
        {
            var empty = 0;
            string query = null;
            var twitterService = new TwitterService();

            var tweets = twitterService.GetTweets(query);

            Assert.AreEqual(empty, tweets.Count());
        }

        [Test]
        public void Given_Last_Tweet_GetTweets_Can_Successfully_Retrieve_Latest_Values_From_Twitter()
        {
            var query = "#ltnet";
            var twitterService = new TwitterService();

            var tweets = twitterService.GetTweets(query).ToList();
            if (tweets.Count() > 1)
            {
                var lastTweet = tweets.Last();
                var latestTweets = twitterService.GetTweets(query, lastTweet);

                Assert.AreEqual(tweets.Count(), latestTweets.Count() + 1);
            }
        }
    }
}

using System;
using System.Linq;
using NUnit.Framework;
using Services.Twitter;

namespace Tests.Services.Twitter
{
    [TestFixture]
    public class TwitterServiceTests
    {
        private DateTime _date;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            _date = DateTime.UtcNow.AddMonths(-2).Date;
        }

        [Test]
        public void GetTweets_Can_Successfully_Retrieve_Values_From_Twitter()
        {
            var query = "#ltnet";
            var twitterService = new TwitterService();

            var tweets = twitterService.GetTweets(query, _date);

            Assert.That(tweets.Count(), Is.GreaterThan(0));
        }

        [Test]
        public void GetTweers_Returns_Empty_List_When_No_Results_Found()
        {
            var empty = 0;
            var query = "#hashtagfortesting";
            var twitterService = new TwitterService();

            var tweets = twitterService.GetTweets(query, _date);

            Assert.AreEqual(empty, tweets.Count());
        }

        [Test]
        public void GetTweers_Returns_Empty_List_When_Internal_Exception_Happens()
        {
            var empty = 0;
            string query = null;
            var twitterService = new TwitterService();

            var tweets = twitterService.GetTweets(query, _date);

            Assert.AreEqual(empty, tweets.Count());
        }

        [Test]
        public void Given_Last_Tweet_Date_GetTweets_Can_Successfully_Retrieve_Latest_Values_From_Twitter()
        {
            var query = "#ltnet";
            var twitterService = new TwitterService();

            var tweets = twitterService.GetTweets(query, _date).ToList();
            if (tweets.Count() > 1)
            {
                var fromDate = tweets.Last().Published;
                var latestTweets = twitterService.GetTweets(query, fromDate);

                Assert.That(tweets.Count(), Is.GreaterThan(latestTweets.Count()));
            }
        }
    }
}

using System.Configuration;
using System.Linq;
using NUnit.Framework;
using Services.Twitter;

namespace Tests.Services
{
    [TestFixture]
    public class TwitterTests
    {
        [Test]
        public void GetTweets_Can_Successfully_Retrieve_Values_From_Twitter()
        {
            var query = ConfigurationManager.AppSettings["twitter"];
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
    }
}

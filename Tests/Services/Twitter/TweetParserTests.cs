namespace DotNetGroup.Tests.Services.Twitter
{
    using System.Linq;

    using DotNetGroup.Services.Twitter;

    using NUnit.Framework;

    [TestFixture]
    public class TweetParserTests
    {
        [Test]
        public void Given_Tweet_Contains_Url_It_Is_Correctly_Parsed()
        {
            var tweet = "test http://localhost url";

            var tweetParser = new TweetParser();

            var urls = tweetParser.GetUrls(tweet).ToList();

            Assert.That(urls.Count, Is.EqualTo(1));
            Assert.That(urls.Single(), Is.EqualTo("http://localhost"));
        }

        [Test]
        public void Given_Tweet_Contains_Mention_It_Is_Correctly_Parsed()
        {
            var tweet = "test @mention!";

            var tweetParser = new TweetParser();

            var mentions = tweetParser.GetMentions(tweet).ToList();

            Assert.That(mentions.Count, Is.EqualTo(1));
            Assert.That(mentions.Single(), Is.EqualTo("mention"));
        }

        [Test]
        public void Given_Tweet_Contains_Hashtag_It_Is_Correctly_Parsed()
        {
            var tweet = "test #hashtag.";

            var tweetParser = new TweetParser();

            var hashtags = tweetParser.GetHashtags(tweet).ToList();

            Assert.That(hashtags.Count, Is.EqualTo(1));
            Assert.That(hashtags.Single(), Is.EqualTo("hashtag"));
        }

        [Test]
        public void Given_Same_Hashtag_Is_Mentioned_Multiple_Times_Only_Single_Occurence_Returned()
        {
            var tweet = "test #hashtag and one more #hashtag";

            var tweetParser = new TweetParser();

            var hashtags = tweetParser.GetHashtags(tweet).ToList();

            Assert.That(hashtags.Count, Is.EqualTo(1));
        }
    }
}

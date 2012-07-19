namespace DotNetGroup.Tests.Services.Twitter
{
    using DotNetGroup.Services.Twitter;

    using LinqToTwitter;

    using NUnit.Framework;

    [TestFixture]
    public class TwitterSearchEntryHelperTests
    {
        [Test]
        public void Given_User_Name_And_Tweet_Id_Correct_Tweet_Status_Url_Returned()
        {
            var entry = new SearchEntry { FromUser = "user", ID = 123 };
            var helper = new TwitterSearchEntryHelper(entry);

            var statusUrl = helper.GetStatusUrl();

            Assert.That(statusUrl, Is.EqualTo("https://twitter.com/user/status/123"));
        }

        [Test]
        public void Given_User_Name_Correct_User_Url_Returned()
        {
            var entry = new SearchEntry { FromUser = "user", ID = 123 };
            var helper = new TwitterSearchEntryHelper(entry);

            var userUrl = helper.GetUserUrl();

            Assert.That(userUrl, Is.EqualTo("https://twitter.com/user"));
        }

        [Test]
        public void Given_Tweet_Status_Contains_Hashtag_It_Is_Returned()
        {
            var entry = new SearchEntry { Text = "This is #test tweet" };
            var helper = new TwitterSearchEntryHelper(entry);

            var hashtags = helper.GetHashtags();

            Assert.That(hashtags.Length, Is.EqualTo(1));
            Assert.That(hashtags[0], Is.EqualTo("test"));
        }

        [Test]
        public void Given_Tweet_Status_Correct_Status_Html_Returned()
        {
            var content = "This is #demo tweet to check http://twitter.com /cc @user";
            var entry = new SearchEntry { Text = content };
            var helper = new TwitterSearchEntryHelper(entry);

            var status = helper.GetStatusHtml();

            var html = "This is <a href=\"https://search.twitter.com/search?q=%23demo\" target=\"_blank\">#demo</a> " + 
                       "tweet to check <a href=\"http://twitter.com\" target=\"_blank\">http://twitter.com</a> " +
                       "/cc <a href=\"https://twitter.com/user\" target=\"_blank\">@user</a>";
            Assert.That(status, Is.EqualTo(html));
        }
    }
}

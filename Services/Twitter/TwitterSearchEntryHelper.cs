namespace DotNetGroup.Services.Twitter
{
    using System.Linq;

    using LinqToTwitter;

    public interface ITwitterSearchEntryHelper
    {
        string GetStatusUrl();

        string GetUserUrl();

        string GetStatusHtml();

        string[] GetHashtags();
    }

    public class TwitterSearchEntryHelper : ITwitterSearchEntryHelper
    {
        private readonly SearchEntry entry;
        private readonly ITweetParser tweetParser;

        public TwitterSearchEntryHelper(SearchEntry entry)
            : this(entry, new TweetParser())
        {
        }

        public TwitterSearchEntryHelper(SearchEntry entry, ITweetParser tweetParser)
        {
            this.entry = entry;
            this.tweetParser = tweetParser;
        }

        public string GetStatusUrl()
        {
            return string.Format("https://twitter.com/{0}/status/{1}", this.entry.FromUser, this.entry.ID);
        }

        public string GetUserUrl()
        {
            return string.Format("https://twitter.com/{0}", this.entry.FromUser);
        }

        public string GetStatusHtml()
        {
            var tweet = this.entry.Text;

            foreach (var url in this.tweetParser.GetUrls(tweet))
            {
                tweet = tweet.Replace(url, string.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", url));
            }

            foreach (var mention in this.tweetParser.GetMentions(tweet))
            {
                tweet = tweet.Replace("@" + mention, string.Format("<a href=\"https://twitter.com/{0}\" target=\"_blank\">@{0}</a>", mention));
            }

            foreach (var hashtag in this.tweetParser.GetHashtags(tweet))
            {
                tweet = tweet.Replace("#" + hashtag, string.Format("<a href=\"https://search.twitter.com/search?q=%23{0}\" target=\"_blank\">#{0}</a>", hashtag));
            }

            return tweet;
        }

        public string[] GetHashtags()
        {
            return this.tweetParser.GetHashtags(this.entry.Text).ToArray();
        }
    }
}
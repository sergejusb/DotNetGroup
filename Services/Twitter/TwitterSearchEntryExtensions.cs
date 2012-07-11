namespace Services.Twitter
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    using LinqToTwitter;

    public static class TwitterSearchEntryExtensions
    {
        private static readonly Regex UrlPattern = new Regex("\\b(([\\w-]+://?|www[.])[^\\s()<>]+(?:\\([\\w\\d]+\\)|([^\\p{P}\\s]|/)))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex MentionPattern = new Regex("(^|\\W)@([A-Za-z0-9_]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex HashtagPattern = new Regex("[#]+[A-Za-z0-9-_]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string GetStatusUrl(this SearchEntry item)
        {
            return string.Format("https://twitter.com/{0}/status/{1}", item.FromUser, item.ID);
        }

        public static string GetUserUrl(this SearchEntry item)
        {
            return string.Format("https://twitter.com/{0}", item.FromUser);
        }

        public static string GetStatusHtml(this SearchEntry item)
        {
            var tweetText = item.Text;

            foreach (Match match in UrlPattern.Matches(tweetText))
            {
                tweetText = tweetText.Replace(match.Value, string.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", match.Value));
            }

            foreach (Match match in MentionPattern.Matches(tweetText))
            {
                if (match.Groups.Count == 3)
                {
                    var value = match.Groups[2].Value;
                    var text = "@" + value;
                    tweetText = tweetText.Replace(text, string.Format("<a href=\"https://twitter.com/{0}\" target=\"_blank\">{1}</a>", value, text));
                }
            }

            foreach (Match match in HashtagPattern.Matches(tweetText))
            {
                var query = Uri.EscapeDataString(match.Value);
                tweetText = tweetText.Replace(match.Value, string.Format("<a href=\"https://search.twitter.com/search?q={0}\" target=\"_blank\">{1}</a>", query, match.Value));
            }

            return tweetText;
        }

        public static string[] GetHashtags(this SearchEntry item)
        {
            var matches = HashtagPattern.Matches(item.Text);
            return matches.Count > 0 ? matches.Cast<Match>().Select(m => m.Groups[1].Value).Distinct().ToArray() : new string[0];
        }
    }
}
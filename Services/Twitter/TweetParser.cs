namespace Services.Twitter
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public interface ITweetParser
    {
        IEnumerable<string> GetUrls(string tweet);
        
        IEnumerable<string> GetMentions(string tweet);

        IEnumerable<string> GetHashtags(string tweet);
    }

    public class TweetParser : ITweetParser
    {
        private static readonly Regex UrlPattern = new Regex("\\b(([\\w-]+://?|www[.])[^\\s()<>]+(?:\\([\\w\\d]+\\)|([^\\p{P}\\s]|/)))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex MentionPattern = new Regex("(^|\\W)@([A-Za-z0-9_]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex HashtagPattern = new Regex("[#]+([A-Za-z0-9-_]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public IEnumerable<string> GetUrls(string tweet)
        {
            return UrlPattern.Matches(tweet).Cast<Match>().Select(m => m.Value).Distinct();
        }

        public IEnumerable<string> GetMentions(string tweet)
        {
            return MentionPattern.Matches(tweet).Cast<Match>().Where(m => m.Groups.Count == 3).Select(m => m.Groups[2].Value).Distinct();
        }

        public IEnumerable<string> GetHashtags(string tweet)
        {
            return HashtagPattern.Matches(tweet).Cast<Match>().Where(m => m.Groups.Count == 2).Select(m => m.Groups[1].Value).Distinct();
        }
    }
}
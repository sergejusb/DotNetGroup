namespace Services.Twitter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using LinqToTwitter;

    using Services.Model;

    public interface ITwitterService
    {
        IEnumerable<Item> GetTweets(string query, DateTime fromDate);
    }

    public class TwitterService : ITwitterService
    {
        private const int Count = 100;
        private static readonly Regex HashtagPattern = new Regex("#(\\w+)", RegexOptions.Compiled);

        public IEnumerable<Item> GetTweets(string query, DateTime fromDate)
        {
            try
            {
                using (var context = new TwitterContext())
                {
                    // for some reasons only Date part of DateTime is accepted as an argument for Twitter's since query
                    var date = fromDate == DateTime.MinValue ? DateTime.UtcNow.AddMonths(-1).Date : fromDate.Date;

                    var result = (from search in context.Search
                                  where search.Type == SearchType.Search
                                  && search.Query == query
                                  && search.Since == date
                                  && search.WithRetweets == false
                                  && search.PageSize == Count
                                  select search).First().Results;

                    // due to the aforementioned limitation need to perform additional filtering
                    result = result.Where(t => t.CreatedAt.LocalDateTime > fromDate).ToList();

                    return result.Select(t => new Item
                    {
                        Url = GetTweetUrl(t.FromUser, t.ID),
                        Published = t.CreatedAt.LocalDateTime,
                        AuthorImage = t.ProfileImageUrl,
                        AuthorName = t.FromUserName,
                        AuthorUri = GetUserUrl(t.FromUser),
                        Title = string.Empty,
                        Content = t.Text,
                        Tags = ExtractTags(t.Text),
                        ItemType = ItemType.Twitter
                    }).ToList();
                }
            }
            catch
            {
                return new List<Item>();
            }
        }

        private static string GetTweetUrl(string userName, object tweetId)
        {
            return string.Format("https://twitter.com/{0}/status/{1}", userName, tweetId);
        }

        private static string GetUserUrl(string userName)
        {
            return string.Format("https://twitter.com/{0}", userName);
        }

        private static string[] ExtractTags(string content)
        {
            var matches = HashtagPattern.Matches(content);
            return matches.Count > 0 ? matches.Cast<Match>().Select(m => m.Groups[1].Value).Distinct().ToArray() : new string[0];
        }
    }
}

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
                var context = new TwitterContext();

                // for some reasons only Date part of DateTime is accepted as an argument for Twitter's since query
                var date = fromDate == DateTime.MinValue ? DateTime.UtcNow.AddMonths(-1).Date : fromDate.Date;
                
                var result = (from search in context.Search
                              where search.Type == SearchType.Search
                              && search.Query == query
                              && search.Since == date
                              && search.WithRetweets == false
                              && search.PageSize == Count
                              select search).First().Entries;

                // due to the aforementioned limitation need to perform additional filtering
                result = result.Where(e => e.Published > fromDate).ToList();

                return result.Select(e => new Item
                {
                    Url = e.Alternate,
                    Published = e.Published,
                    AuthorImage = e.Image,
                    AuthorName = e.Author.Name,
                    AuthorUri = e.Author.URI,
                    Title = string.Empty,
                    Content = e.Content,
                    Tags = ExtractTags(e.Content),
                    ItemType = ItemType.Twitter
                }).ToList();
            }
            catch
            {
                return new List<Item>();
            }
        }

        private static string[] ExtractTags(string content)
        {
            var matches = HashtagPattern.Matches(content);
            return matches.Count > 0 ? matches.Cast<Match>().Select(m => m.Groups[1].Value).Distinct().ToArray() : new string[0];
        }
    }
}

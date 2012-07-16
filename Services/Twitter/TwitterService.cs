namespace Services.Twitter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LinqToTwitter;

    using Services.Model;

    public interface ITwitterService
    {
        IEnumerable<Item> GetTweets(string query, DateTime fromDate);
    }

    public class TwitterService : ITwitterService
    {
        private const int Count = 100;

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
                    result = result.Where(tweet => tweet.CreatedAt.LocalDateTime > fromDate).ToList();

                    return result.Select(tweet => new Item
                    {
                        Url = tweet.Helper().GetStatusUrl(),
                        Published = tweet.CreatedAt.LocalDateTime,
                        AuthorImage = tweet.ProfileImageUrl,
                        AuthorName = tweet.FromUserName,
                        AuthorUri = tweet.Helper().GetUserUrl(),
                        Title = string.Empty,
                        Content = tweet.Helper().GetStatusHtml(),
                        Tags = tweet.Helper().GetHashtags(),
                        ItemType = ItemType.Twitter
                    }).ToList();
                }
            }
            catch
            {
                return new List<Item>();
            }
        }
    }
}

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
                    result = result.Where(t => t.CreatedAt.LocalDateTime > fromDate).ToList();

                    return result.Select(t => new Item
                    {
                        Url = t.GetStatusUrl(),
                        Published = t.CreatedAt.LocalDateTime,
                        AuthorImage = t.ProfileImageUrl,
                        AuthorName = t.FromUserName,
                        AuthorUri = t.GetUserUrl(),
                        Title = string.Empty,
                        Content = t.GetStatusHtml(),
                        Tags = t.GetHashtags(),
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

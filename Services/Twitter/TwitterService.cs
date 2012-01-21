using System;
using System.Collections.Generic;
using System.Linq;
using LinqToTwitter;
using Services.Model;

namespace Services.Twitter
{
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
                var context = new TwitterContext();
                var result = (from search in context.Search
                              where search.Type == SearchType.Search
                              && search.Query == query
                              && search.Since == (fromDate == DateTime.MinValue ? DateTime.UtcNow.AddMonths(-1).Date : fromDate)
                              && search.WithRetweets == false
                              && search.PageSize == Count
                              select search).First().Entries;

                return result.Select(e => new Item
                {
                    Url = e.Alternate,
                    Published = e.Published,
                    AuthorImage = e.Image,
                    AuthorName = e.Author.Name,
                    AuthorUri = e.Author.URI,
                    Title = e.Title,
                    Content = e.Content,
                    ItemType = ItemType.Twitter
                }).ToList();
            }
            catch
            {
                return new List<Item>();
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using LinqToTwitter;

namespace Services.Twitter
{
    public interface ITwitterService
    {
        IEnumerable<Tweet> GetTweets(string query, int count);
    }

    public class TwitterService : ITwitterService
    {
        public IEnumerable<Tweet> GetTweets(string query, int count)
        {
            try
            {
                var context = new TwitterContext();

                var result = (from search in context.Search
                              where search.Type == SearchType.Search
                              && search.Query == query
                              && search.WithRetweets == false
                              && search.PageSize == count
                              select search).First().Entries;

                return result.Select(e => new Tweet
                {
                    Url = e.Alternate,
                    Published = e.Published,
                    AuthorImage = e.Image,
                    AuthorName = e.Author.Name,
                    AuthorUri = e.Author.URI,
                    Title = e.Title,
                    Content = e.Content,
                    Location = e.Location
                }).ToList();
            }
            catch
            {
                return new List<Tweet>();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Twitter
{
    public interface ITwitterAggregator
    {
        IEnumerable<Tweet> GetLatestTweets(int count = 50);
        IEnumerable<Tweet> GetLatestTweets(string url, int count = 50);
    }

    public class TwitterAggregator : ITwitterAggregator
    {
        private readonly ITwitterService _twitterService;
        private readonly string _query;

        public TwitterAggregator()
            : this(new TwitterService(), "#ltnet")
        {
        }

        public TwitterAggregator(ITwitterService twitterService, string query)
        {
            _twitterService = twitterService;
            _query = query;
        }

        public virtual IEnumerable<Tweet> GetLatestTweets(int count = 50)
        {
            return _twitterService.GetTweets(_query, count)
                    .Where(t => !t.Content.Contains("ltnet.tv")) // workaround: ignore competing hashtag for ltnet.tv
                    .ToList();
        }

        public IEnumerable<Tweet> GetLatestTweets(string url, int count = 50)
        {
            return GetLatestTweets(count)
                    .TakeWhile(t => !t.Url.Equals(url, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
        }
    }
}
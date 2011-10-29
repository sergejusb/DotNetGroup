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

        public TwitterAggregator()
            : this(new TwitterService())
        {
        }

        public TwitterAggregator(ITwitterService twitterService)
        {
            _twitterService = twitterService;
        }

        public virtual IEnumerable<Tweet> GetLatestTweets(int count = 50)
        {
            return _twitterService.GetTweets("#ltnet", count)
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
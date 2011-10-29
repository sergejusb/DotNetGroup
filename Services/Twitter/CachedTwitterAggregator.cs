using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Services.Twitter
{
    public class CachedTwitterAggregator : TwitterAggregator
    {
        private readonly string _key = typeof(CachedTwitterAggregator).Name;
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly TimeSpan _cachePeriod;

        public CachedTwitterAggregator(TimeSpan cachePeriod)
            : this(new TwitterService(), cachePeriod)
        {
        }

        public CachedTwitterAggregator(ITwitterService twitterService, TimeSpan cachePeriod)
            : base(twitterService)
        {
            _cachePeriod = cachePeriod;
        }

        public override IEnumerable<Tweet> GetLatestTweets(int count = 50)
        {
            if (!_cache.Contains(_key))
            {
                _cache.Set(_key, base.GetLatestTweets(count), DateTimeOffset.Now.Add(_cachePeriod));
            }

            return _cache.Get(_key) as IEnumerable<Tweet>;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Services.Rss
{
    public class CachedRssAggregator : RssAggregator
    {
        private readonly string _key = typeof(CachedRssAggregator).Name;
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly TimeSpan _cachePeriod;

        public CachedRssAggregator(TimeSpan cachePeriod)
            : this(new RssService(), new ConfigRssUrlProvider(), cachePeriod)
        {
        }

        public CachedRssAggregator(IRssService rssService, IRssUrlProvider urlProvider, TimeSpan cachePeriod)
            : base(rssService, urlProvider)
        {
            _cachePeriod = cachePeriod;
        }

        public override IEnumerable<Feed> GetLatestFeeds(int count = 25)
        {
            if (!_cache.Contains(_key))
            {
                _cache.Set(_key, base.GetLatestFeeds(count), DateTimeOffset.Now.Add(_cachePeriod));
            }

            return _cache.Get(_key) as IEnumerable<Feed>;
        }
    }
}
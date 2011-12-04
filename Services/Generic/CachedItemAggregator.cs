using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using Services.Model;

namespace Services.Generic
{
    public class CachedItemAggregator : BaseItemAggregator
    {
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string _key;
        private readonly IItemAggregator _aggregator;
        private readonly TimeSpan _cachePeriod;

        public CachedItemAggregator(IItemAggregator aggregator, TimeSpan cachePeriod)
        {
            _key = aggregator.GetType().Name;
            _aggregator = aggregator;
            _cachePeriod = cachePeriod;
        }

        public override IEnumerable<Item> GetLatest(int count)
        {
            if (!_cache.Contains(_key))
            {
                _cache.Set(_key, _aggregator.GetLatest(count), DateTimeOffset.Now.Add(_cachePeriod));
            }

            return _cache.Get(_key) as IEnumerable<Item>;
        }
    }
}
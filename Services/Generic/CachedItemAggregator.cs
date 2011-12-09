using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Services.Model;

namespace Services.Generic
{
    public interface ICachedItemAggregator : IItemAggregator
    {
        IEnumerable<Item> GetLatest(bool cached);
        IEnumerable<Item> GetLatest(string url, bool cached);
    }

    public class CachedItemAggregator : BaseItemAggregator, ICachedItemAggregator
    {
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string _key;
        private readonly IItemAggregator _aggregator;

        public CachedItemAggregator(IItemAggregator aggregator)
        {
            _key = aggregator.GetType().Name;
            _aggregator = aggregator;
        }

        public IEnumerable<Item> GetLatest(bool cached = true)
        {
            if (!_cache.Contains(_key))
            {
                _cache.Set(_key, _aggregator.GetLatest(), ObjectCache.InfiniteAbsoluteExpiration);
            }
            else if (!cached)
            {
                var cachedItems = _cache.Get(_key) as IEnumerable<Item>;
                var items = cachedItems.Any() ? _aggregator.GetLatest(cachedItems.First().Url).Union(cachedItems) : _aggregator.GetLatest();
                _cache.Set(_key, items, ObjectCache.InfiniteAbsoluteExpiration);
            }

            return _cache.Get(_key) as IEnumerable<Item>;
        }

        public override IEnumerable<Item> GetLatest()
        {
            return GetLatest();
        }

        public IEnumerable<Item> GetLatest(string url, bool cached = true)
        {
            return base.GetLatest(url);
        }
    }
}
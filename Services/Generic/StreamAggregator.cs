using System;
using System.Collections.Generic;
using System.Linq;
using Services.Model;
using Services.Rss;
using Services.Twitter;

namespace Services.Generic
{
    public interface IItemAggregator
    {
        IEnumerable<Item> GetLatest(DateTime fromDate);
    }

    public class StreamAggregator : IItemAggregator
    {
        private readonly IItemAggregator[] _itemAggregators;

        public StreamAggregator(params IItemAggregator[] itemAggregators)
        {
            if (itemAggregators == null)
                throw new ArgumentNullException("itemAggregators");

            _itemAggregators = itemAggregators;
        }

        public StreamAggregator()
            : this(new RssAggregator(), new TwitterAggregator())
        {
        }

        public IEnumerable<Item> GetLatest(DateTime fromDate)
        {
            return _itemAggregators.SelectMany(a => a.GetLatest(fromDate))
                .OrderBy(i => i.Published)
                .ToList();
        }
    }
}
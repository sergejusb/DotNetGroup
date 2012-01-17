using System;
using System.Collections.Generic;
using System.Linq;
using Services.Model;

namespace Services.Generic
{
    public interface IItemAggregator
    {
        IEnumerable<Item> GetLatest(DateTime fromDate);
    }

    public class ItemAggregator : IItemAggregator
    {
        private readonly IItemAggregator[] _itemAggregators;

        public ItemAggregator(params IItemAggregator[] itemAggregators)
        {
            if (itemAggregators == null)
                throw new ArgumentNullException("itemAggregators");

            _itemAggregators = itemAggregators;
        }

        public IEnumerable<Item> GetLatest(DateTime fromDate)
        {
            return _itemAggregators.SelectMany(a => a.GetLatest(fromDate))
                .OrderBy(i => i.Published)
                .ToList();
        }
    }
}
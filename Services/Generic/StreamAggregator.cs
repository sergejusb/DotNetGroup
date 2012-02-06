namespace Services.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Services.Model;
    using Services.Rss;
    using Services.Twitter;

    public interface IItemAggregator
    {
        IEnumerable<Item> GetLatest(DateTime fromDate);
    }

    public class StreamAggregator : IItemAggregator
    {
        private readonly IItemAggregator[] itemAggregators;

        public StreamAggregator()
            : this(new RssAggregator(), new TwitterAggregator())
        {
        }

        public StreamAggregator(params IItemAggregator[] itemAggregators)
        {
            if (itemAggregators == null)
            {
                throw new ArgumentNullException("itemAggregators");
            }

            this.itemAggregators = itemAggregators;
        }

        public IEnumerable<Item> GetLatest(DateTime fromDate)
        {
            return this.itemAggregators.SelectMany(a => a.GetLatest(fromDate))
                .OrderBy(i => i.Published)
                .ToList();
        }
    }
}
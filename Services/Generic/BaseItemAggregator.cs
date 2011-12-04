using System;
using System.Collections.Generic;
using System.Linq;
using Services.Model;

namespace Services.Generic
{
    public interface IItemAggregator
    {
        IEnumerable<Item> GetLatest(int count);
        IEnumerable<Item> GetLatest(string url, int count);
    }

    public abstract class BaseItemAggregator : IItemAggregator
    {
        public abstract IEnumerable<Item> GetLatest(int count);
        
        public virtual IEnumerable<Item> GetLatest(string url, int count)
        {
            return GetLatest(count)
                    .TakeWhile(f => !f.Url.Equals(url, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
        }
    }
}
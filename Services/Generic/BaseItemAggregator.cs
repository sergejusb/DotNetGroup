using System;
using System.Collections.Generic;
using System.Linq;
using Services.Model;

namespace Services.Generic
{
    public interface IItemAggregator
    {
        IEnumerable<Item> GetLatest();
        IEnumerable<Item> GetLatest(string url);
    }

    public abstract class BaseItemAggregator : IItemAggregator
    {
        public abstract IEnumerable<Item> GetLatest();
        
        public virtual IEnumerable<Item> GetLatest(string url)
        {
            var items = GetLatest();
            if (url != null)
            {
                items = items.TakeWhile(f => !f.Url.Equals(url, StringComparison.InvariantCultureIgnoreCase));
            }
            return items;
        }
    }
}
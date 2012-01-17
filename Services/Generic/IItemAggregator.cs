using System;
using System.Collections.Generic;
using Services.Model;

namespace Services.Generic
{
    public interface IItemAggregator
    {
        IEnumerable<Item> GetLatest(DateTime fromDate);
    }
}
using System;
using System.Linq;
using Services.Generic;
using Services.Processors;
using Services.Storage;

namespace Services
{
    public class StreamPersister
    {
        private readonly IItemAggregator _streamAggregator;
        private readonly IItemProcessor _itemProcessor;
        private readonly IStreamStorage _streamStorage;

        public StreamPersister(string connectionString, string database)
            : this(new StreamAggregator(), new ItemProcessor(), new StreamStorage(connectionString, database))
        {
        }

        public StreamPersister(IItemAggregator streamAggregator, IItemProcessor itemProcessor, IStreamStorage streamStorage)
        {
            if (streamAggregator == null)
                throw new ArgumentNullException("streamAggregator");
            if (itemProcessor == null)
                throw new ArgumentNullException("itemProcessor");
            if (streamStorage == null)
                throw new ArgumentNullException("streamStorage");

            _streamAggregator = streamAggregator;
            _itemProcessor = itemProcessor;
            _streamStorage = streamStorage;
        }

        public void PersistLatest()
        {
            var latestItem = _streamStorage.Top();
            var fromDate = latestItem != null ? latestItem.Published : DateTime.MinValue;

            var items = _streamAggregator.GetLatest(fromDate).ToList();
            items.ForEach(_itemProcessor.Process);

            _streamStorage.Save(items);
        }
    }
}

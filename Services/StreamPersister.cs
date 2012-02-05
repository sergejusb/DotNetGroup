using System;
using System.Linq;
using System.Threading.Tasks;
using Services.Generic;
using Services.Model;
using Services.Processors;
using Services.Storage;

namespace Services
{
    public class StreamPersister
    {
        private readonly IItemAggregator _streamAggregator;
        private readonly IItemProcessor _streamProcessor;
        private readonly IStreamStorage _streamStorage;

        public StreamPersister(string connectionString, string database)
            : this(new StreamAggregator(), new ItemProcessor(), new StreamStorage(connectionString, database))
        {
        }

        public StreamPersister(IItemAggregator streamAggregator, IItemProcessor streamProcessor, IStreamStorage streamStorage)
        {
            if (streamAggregator == null)
                throw new ArgumentNullException("streamAggregator");
            if (streamProcessor == null)
                throw new ArgumentNullException("streamProcessor");
            if (streamStorage == null)
                throw new ArgumentNullException("streamStorage");

            _streamAggregator = streamAggregator;
            _streamProcessor = streamProcessor;
            _streamStorage = streamStorage;
        }

        public void PersistLatest()
        {
            var latestItem = _streamStorage.Top() ?? new Item();
            var items = _streamAggregator.GetLatest(latestItem.Published).ToList();

            Parallel.ForEach(items, item => _streamProcessor.Process(item));

            _streamStorage.Save(items);
        }
    }
}

namespace Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Services.Generic;
    using Services.Model;
    using Services.Processors;
    using Services.Storage;

    public interface IStreamPersister
    {
        void PersistLatest();

        void Reprocess();
    }

    public class StreamPersister : IStreamPersister
    {
        private readonly IItemAggregator streamAggregator;
        private readonly IItemProcessor streamProcessor;
        private readonly IStreamStorage streamStorage;

        public StreamPersister(string connectionString, string database)
            : this(new StreamAggregator(), new ItemProcessor(), new StreamStorage(connectionString, database))
        {
        }

        public StreamPersister(IItemAggregator streamAggregator, IItemProcessor streamProcessor, IStreamStorage streamStorage)
        {
            if (streamAggregator == null)
            {
                throw new ArgumentNullException("streamAggregator");
            }

            if (streamProcessor == null)
            {
                throw new ArgumentNullException("streamProcessor");
            }

            if (streamStorage == null)
            {
                throw new ArgumentNullException("streamStorage");
            }

            this.streamAggregator = streamAggregator;
            this.streamProcessor = streamProcessor;
            this.streamStorage = streamStorage;
        }

        public void PersistLatest()
        {
            var latestItem = this.streamStorage.Top() ?? new Item();
            var items = this.streamAggregator.GetLatest(latestItem.Published).ToList();

            Parallel.ForEach(items, item => this.streamProcessor.Process(item));

            this.streamStorage.Save(items);
        }

        public void Reprocess()
        {
            var items = this.streamStorage.GetLatest(fromDate: null, type: null, limit: null).ToList();

            Parallel.ForEach(items, item => this.streamProcessor.Process(item));

            this.streamStorage.Save(items);
        }
    }
}

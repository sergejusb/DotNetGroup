namespace DotNetGroup.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DotNetGroup.Services.Model;
    using DotNetGroup.Services.Processors;
    using DotNetGroup.Services.Storage;

    public interface IStreamReprocessor
    {
        IEnumerable<Item> Reprocess();
    }

    public class StreamReprocessor : IStreamReprocessor
    {
        private readonly IItemProcessor streamProcessor;
        private readonly IStreamStorage streamStorage;

        public StreamReprocessor(string connectionString, string database)
            : this(new ItemProcessor(), new StreamStorage(connectionString, database))
        {
        }

        public StreamReprocessor(IItemProcessor streamProcessor, IStreamStorage streamStorage)
        {
            if (streamProcessor == null)
            {
                throw new ArgumentNullException("streamProcessor");
            }

            if (streamStorage == null)
            {
                throw new ArgumentNullException("streamStorage");
            }

            this.streamProcessor = streamProcessor;
            this.streamStorage = streamStorage;
        }

        public IEnumerable<Item> Reprocess()
        {
            var items = this.streamStorage.GetLatest(type: null, fromDate: null, toDate: null, limit: null).ToList();

            Parallel.ForEach(items, item => this.streamProcessor.Process(item));

            this.streamStorage.Save(items);

            return items;
        }
    }
}
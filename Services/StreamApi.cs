using System;
using System.Collections.Generic;
using Services.Model;
using Services.Storage;

namespace Services
{
    public interface IStreamApi
    {
        Item Get(string id);
        IEnumerable<Item> Get(DateTime fromDate);
        IEnumerable<Item> Get(DateTime fromDate, int limit);
        IEnumerable<Item> Get(DateTime fromDate, ItemType type);
        IEnumerable<Item> Get(DateTime fromDate, ItemType type, int limit);
    }

    public class StreamApi : IStreamApi
    {
        private readonly IStreamStorage _streamStorage;
        private readonly int _limit;

        public StreamApi(string connectionString, string database, int limit = 100)
            : this(new StreamStorage(connectionString, database), limit)
        {
        }

        public StreamApi(IStreamStorage streamStorage, int limit = 100)
        {
            if (streamStorage == null)
                throw new ArgumentNullException("streamStorage");

            _streamStorage = streamStorage;
            _limit = limit;
        }

        public Item Get(string id)
        {
            return _streamStorage.Get(id);
        }

        public IEnumerable<Item> Get(DateTime fromDate)
        {
            return _streamStorage.GetLatest(fromDate, null, _limit);
        }

        public IEnumerable<Item> Get(DateTime fromDate, int limit)
        {
            return _streamStorage.GetLatest(fromDate, null, limit);
        }

        public IEnumerable<Item> Get(DateTime fromDate, ItemType type)
        {
            return _streamStorage.GetLatest(fromDate, type, _limit);
        }

        public IEnumerable<Item> Get(DateTime fromDate, ItemType type, int limit)
        {
            return _streamStorage.GetLatest(fromDate, type, limit);
        }
    }
}
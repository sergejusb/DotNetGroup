using System;
using System.Collections.Generic;
using MongoDB.Bson;
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
        private readonly IStreamStorage _storage;
        private readonly int _limit;

        public StreamApi(IStreamStorage storage, int limit = 100)
        {
            _storage = storage;
            _limit = limit;
        }

        public Item Get(string id)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id, out objectId))
                throw new ArgumentException("ID is of not valid format", "id");

            return _storage.Get(objectId);
        }

        public IEnumerable<Item> Get(DateTime fromDate)
        {
            return _storage.GetLatest(fromDate, ItemType.Any, _limit);
        }

        public IEnumerable<Item> Get(DateTime fromDate, int limit)
        {
            return _storage.GetLatest(fromDate, ItemType.Any, limit);
        }

        public IEnumerable<Item> Get(DateTime fromDate, ItemType type)
        {
            return _storage.GetLatest(fromDate, type, _limit);
        }

        public IEnumerable<Item> Get(DateTime fromDate, ItemType type, int limit)
        {
            return _storage.GetLatest(fromDate, type, limit);
        }
    }
}
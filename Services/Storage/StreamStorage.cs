using System;
using System.Collections.Generic;
using System.Linq;
using FluentMongo.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Services.Model;

namespace Services.Storage
{
    public interface IStreamStorage
    {
        Item Top();
        Item Get(ObjectId id);
        IEnumerable<Item> GetLatest(DateTime fromDate, ItemType type, int limit);
        void Save(IEnumerable<Item> items);
    }

    public class StreamStorage : IStreamStorage
    {
        private readonly MongoDatabase _database;

        public StreamStorage(string connectionString, string database)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");
            if (String.IsNullOrEmpty(database))
                throw new ArgumentNullException("database");

            _database = MongoServer.Create(connectionString).GetDatabase(database);
        }

        public Item Top()
        {
            return Items.AsQueryable()
                        .OrderByDescending(i => i.Published)
                        .FirstOrDefault();
        }

        public Item Get(ObjectId id)
        {
            return Items.AsQueryable().SingleOrDefault(i => i.Id == id);
        }

        public IEnumerable<Item> GetLatest(DateTime fromDate, ItemType type, int limit)
        {
            var query = Items.AsQueryable();
            query = query.Where(i => i.Published > fromDate);
            query = type == ItemType.Any ? query : query.Where(i => i.ItemType == type);
            query = query.OrderByDescending(i => i.Published);
            query = limit > 0 ? query.Take(limit) : query;

            return query.ToList();
        }

        public void Save(IEnumerable<Item> items)
        {
            Items.InsertBatch(items);
        }

        private MongoCollection<Item> Items
        {
            get { return _database.GetCollection<Item>("Items"); }
        }
    }
}
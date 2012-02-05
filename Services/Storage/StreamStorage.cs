using System;
using System.Collections.Generic;
using System.Linq;
using FluentMongo.Linq;
using MongoDB.Driver;
using Services.Model;

namespace Services.Storage
{
    public interface IStreamStorage
    {
        Item Top();
        Item Get(string id);
        IEnumerable<Item> GetLatest(DateTime? fromDate, ItemType? type, int? limit);
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

        public Item Get(string id)
        {
            return Items.AsQueryable().SingleOrDefault(i => i.Id == id);
        }

        public IEnumerable<Item> GetLatest(DateTime? fromDate, ItemType? type, int? limit)
        {
            var query = Items.AsQueryable();
            if (fromDate.HasValue) query = query.Where(i => i.Published > fromDate.Value);
            if (type.HasValue) query = query.Where(i => i.ItemType == type.Value);
            query = query.OrderByDescending(i => i.Published);
            if (limit.HasValue) query = query.Take(limit.Value);

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
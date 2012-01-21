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
        IEnumerable<Item> Get(DateTime fromDate);
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

        public IEnumerable<Item> Get(DateTime fromDate)
        {
            return Items.AsQueryable()
                        .Where(i => i.Published > fromDate)
                        .OrderByDescending(i => i.Published)
                        .ToList();
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
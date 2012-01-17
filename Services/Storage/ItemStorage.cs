using System;
using System.Collections.Generic;
using System.Linq;
using FluentMongo.Linq;
using MongoDB.Driver;
using Services.Model;

namespace Services.Storage
{
    public class ItemStorage
    {
        private readonly MongoDatabase _database;

        public ItemStorage(string connectionString, string database)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");
            if (String.IsNullOrEmpty(database))
                throw new ArgumentNullException("database");

            _database = MongoServer.Create(connectionString).GetDatabase(database);
        }

        public void Save(IEnumerable<Item> items)
        {
            Items.InsertBatch(items);
        }

        public IEnumerable<Item> Get()
        {
            return Items.AsQueryable().ToList();
        }

        private MongoCollection<Item> Items
        {
            get { return _database.GetCollection<Item>("Items"); }
        }
    }
}
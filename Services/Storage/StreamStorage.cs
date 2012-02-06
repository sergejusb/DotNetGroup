namespace Services.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentMongo.Linq;

    using MongoDB.Driver;

    using Services.Model;

    public interface IStreamStorage
    {
        Item Top();

        Item Get(string id);

        IEnumerable<Item> GetLatest(ItemType? type, DateTime? from, DateTime? to, int? limit);

        void Save(IEnumerable<Item> items);
    }

    public class StreamStorage : IStreamStorage
    {
        private readonly MongoDatabase database;

        public StreamStorage(string connectionString, string database)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }

            this.database = MongoServer.Create(connectionString).GetDatabase(database);
        }

        private MongoCollection<Item> Items
        {
            get { return this.database.GetCollection<Item>("Items"); }
        }

        public Item Top()
        {
            return this.Items.AsQueryable()
                             .OrderByDescending(i => i.Published)
                             .FirstOrDefault();
        }

        public Item Get(string id)
        {
            return this.Items.AsQueryable().SingleOrDefault(i => i.Id == id);
        }

        public IEnumerable<Item> GetLatest(ItemType? type, DateTime? from, DateTime? to, int? limit)
        {
            var query = this.Items.AsQueryable();
            
            if (type.HasValue)
            {
                query = query.Where(i => i.ItemType == type.Value);
            }

            if (from.HasValue)
            {
                query = query.Where(i => i.Published > from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(i => i.Published < to.Value);
            }

            query = query.OrderByDescending(i => i.Published);
            
            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            return query.ToList();
        }

        public void Save(IEnumerable<Item> items)
        {
            foreach (var item in items)
            {
                this.Items.Save(item);
            }
        }
    }
}
namespace DotNetGroup.Services.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DotNetGroup.Services.Model;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    public interface IStreamStorage
    {
        Item Top();

        Item Get(string id);

        IEnumerable<Item> GetLatest(int limit);

        IEnumerable<Item> GetNewer(Item item, int limit);

        IEnumerable<Item> GetOlder(Item item, int limit);

        void Save(IEnumerable<Item> items);

        IEnumerable<Item> GetLatest(ItemType? type, DateTime? fromDate, DateTime? toDate, int? limit);
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
            return this.Items.AsQueryable()
                             .SingleOrDefault(i => i.Id == id);
        }

        public IEnumerable<Item> GetLatest(int limit)
        {
            return this.Items.AsQueryable()
                             .OrderByDescending(i => i.Published)
                             .Take(limit)
                             .ToList();
        }

        public IEnumerable<Item> GetNewer(Item item, int limit)
        {
            return this.Items.AsQueryable()
                             .OrderByDescending(i => i.Published)
                             .Where(i => i.Published > item.Published)
                             .Take(limit)
                             .ToList();
        }

        public IEnumerable<Item> GetOlder(Item item, int limit)
        {
            return this.Items.AsQueryable()
                             .OrderByDescending(i => i.Published)
                             .Where(i => i.Published < item.Published)
                             .Take(limit)
                             .ToList();
        }

        public void Save(IEnumerable<Item> items)
        {
            foreach (var item in items)
            {
                this.Items.Save(item);
            }
        }

        public IEnumerable<Item> GetLatest(ItemType? type, DateTime? fromDate, DateTime? toDate, int? limit)
        {
            var query = this.Items.AsQueryable();

            if (type.HasValue)
            {
                query = query.Where(i => i.ItemType == type.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(i => i.Published > fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(i => i.Published < toDate.Value);
            }

            query = query.OrderByDescending(i => i.Published);

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            return query.ToList();
        }
    }
}
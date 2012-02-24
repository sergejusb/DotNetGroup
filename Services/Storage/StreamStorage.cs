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

        IEnumerable<Item> GetLatest(ItemType? type, DateTime? fromDate, DateTime? toDate, int? limit);

        int Count(ItemType? type, DateTime? fromDate, DateTime? toDate, int? limit);

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

        public IEnumerable<Item> GetLatest(ItemType? type, DateTime? fromDate, DateTime? toDate, int? limit)
        {
           return this.GetItemsQuery(type, fromDate, toDate, limit).ToList();
        }

        public int Count(ItemType? type, DateTime? fromDate, DateTime? toDate, int? limit)
        {
            return this.GetItemsQuery(type, fromDate, toDate, limit).Count();
        }

        public void Save(IEnumerable<Item> items)
        {
            foreach (var item in items)
            {
                // if item with same URL already exists, update existing item instead of inserting new
                var publishedEarlier = this.Items.AsQueryable().SingleOrDefault(i => i.Url == item.Url);
                if (publishedEarlier != null)
                {
                    item.Id = publishedEarlier.Id;
                }

                this.Items.Save(item);
            }
        }

        private IQueryable<Item> GetItemsQuery(ItemType? type, DateTime? fromDate, DateTime? toDate, int? limit)
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
            return query;
        }
    }
}
namespace Tests.Services.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using FluentMongo.Linq;

    using MongoDB.Driver;

    using NUnit.Framework;

    using Ploeh.AutoFixture;

    using global::Services.Model;

    using global::Services.Storage;

    using Tests.Helpers;

    [TestFixture]
    public class StreamStorageTests
    {
        private static readonly string ConnectionString = ConfigurationManager.AppSettings["db.connection"];
        private static readonly string DatabaseName = ConfigurationManager.AppSettings["db.database"];

        private MongoServer server;

        private MongoCollection<Item> Items
        {
            get { return this.server.GetDatabase(DatabaseName).GetCollection<Item>("Items"); }
        }

        [SetUp]
        public void SetupTest()
        {
            this.server = MongoServer.Create(ConnectionString);
        }

        [TearDown]
        public void CleanupTest()
        {
            this.server.DropDatabase(DatabaseName);
            this.server.Disconnect();
        }

        [DB, Test]
        public void Given_Null_Arguments_Constructor_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new StreamStorage(null, DatabaseName));
            Assert.Throws<ArgumentNullException>(() => new StreamStorage(ConnectionString, null));
        }

        [DB, Test]
        public void Given_MongoDB_Is_Running_ItemStorage_Can_Successfully_Connect()
        {
            new StreamStorage(ConnectionString, DatabaseName);
        }

        [DB, Test]
        public void Given_Non_Valid_Id_Get_Returns_Null()
        {
            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItem = storage.Get("id");

            Assert.IsNull(gotItem);
        }

        [DB, Test]
        public void Given_Existing_Item_Get_Returns_It_By_Id()
        {
            var item = BuildItems(count: 1).Single();
            this.Items.Insert(item);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItem = storage.Get(item.Id);

            Assert.IsNotNull(gotItem);
            Assert.AreEqual(item.Url, gotItem.Url);
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetLatest_Returns_All_10_Items()
        {
            var items = BuildItems(count: 10);
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetLatest(null, null, null, null).ToList();

            Assert.That(items.Count, Is.EqualTo(gotItems.Count));
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetLatest_Returns_Them_Sorted_By_Date_Descending()
        {
            var items = BuildItems(count: 10);
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetLatest(null, null, null, null).ToList();

            Assert.That(gotItems, Is.Ordered.Descending.By("Published"));
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetLatest_Returns_Rss_Items_Only()
        {
            var items = BuildItems(count: 10).OrderBy(i => i.Published);
            var numberOfRssItems = items.Count(i => i.ItemType == ItemType.Rss);
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetLatest(ItemType.Rss, null, null, null).ToList();

            Assert.AreEqual(numberOfRssItems, gotItems.Count);
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetLatest_Returns_Items_Newer_Than_Given_Date()
        {
            var items = BuildItems(count: 10).OrderBy(i => i.Published);
            var fromDate = items.First().Published;
            var numberOfItems = items.Count(i => i.Published > fromDate);
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetLatest(null, fromDate, null, null).ToList();

            Assert.AreEqual(numberOfItems, gotItems.Count);
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetLatest_Returns_Items_Older_Than_Given_Date()
        {
            var items = BuildItems(count: 10).OrderByDescending(i => i.Published);
            var toDate = items.First().Published;
            var numberOfItems = items.Count(i => i.Published < toDate);
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetLatest(null, null, toDate, null).ToList();

            Assert.AreEqual(numberOfItems, gotItems.Count);
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetLatest_Returns_Limited_Number_Of_Items()
        {
            var limit = 1;
            var items = BuildItems(count: 10).OrderBy(i => i.Published);
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetLatest(null, null, null, limit).ToList();

            Assert.AreEqual(limit, gotItems.Count);
        }

        [DB, Test]
        public void Given_Existing_10_Items_Top_Returns_Most_Recent_Item()
        {
            var items = BuildItems(count: 10).OrderBy(i => i.Published);
            var recentItem = items.Last();
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItem = storage.Top();

            Assert.AreEqual(recentItem.Url, gotItem.Url);
        }

        [DB, Test]
        public void ItemStorage_Can_Save_10_New_Items()
        {
            var items = BuildItems(count: 10);
            var storage = new StreamStorage(ConnectionString, DatabaseName);

            storage.Save(items);

            var savedItems = this.Items.AsQueryable().ToList();

            Assert.That(items.Count, Is.EqualTo(savedItems.Count));
        }

        [DB, Test]
        public void ItemStorage_Can_Save_5_New_And_5_Modified_Items()
        {
            this.Items.InsertBatch(BuildItems(count: 5));

            var existingItems = this.Items.AsQueryable().ToList();
            foreach (var item in existingItems)
            {
                item.Tags = new[] { "test" };
            }

            var newItems = BuildItems(count: 5);
            var items = existingItems.Union(newItems).ToList();

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            storage.Save(items);

            var savedItems = this.Items.AsQueryable().ToList();

            Assert.That(items.Count(), Is.EqualTo(savedItems.Count));
        }

        [DB, Test]
        public void ItemStorage_Can_Save_5_Modified_Items_And_Retrieve_Them_Back()
        {
            this.Items.InsertBatch(BuildItems(count: 5));

            var items = this.Items.AsQueryable().ToList();
            foreach (var item in items)
            {
                item.Tags = new[] { "test" };
            }

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            storage.Save(items);

            var savedItems = this.Items.AsQueryable().ToList();

            foreach (var item in savedItems)
            {
                Assert.IsTrue(item.Tags.Contains("test"));
            }
        }

        [DB, Test]
        public void Given_Existing_10_Items_Count_Returns_Same_Number()
        {
            var count = 10;
            this.Items.InsertBatch(BuildItems(count));

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var actualCount = storage.Count(null, null, null, null);

            Assert.That(actualCount, Is.EqualTo(count));
        }

        private static IList<Item> BuildItems(int count)
        {
            return new Fixture()
                .Build<Item>()
                .Without(i => i.Id)
                .Without(i => i.Tags)
                .Do(i => i.ItemType = (ItemType)(1 + new Random().Next(1)))
                .Do(i => i.Published = DateTime.Now.AddDays(new Random().Next(count)).AddHours(new Random().Next(count)))
                .CreateMany(count)
                .ToList();
        }
    }
}

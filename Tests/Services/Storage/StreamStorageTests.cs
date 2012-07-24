namespace DotNetGroup.Tests.Services.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using DotNetGroup.Services.Model;
    using DotNetGroup.Services.Storage;
    using DotNetGroup.Tests.Helpers;

    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    using NUnit.Framework;

    using Ploeh.AutoFixture;

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
            var storage = new StreamStorage(ConnectionString, DatabaseName);
            Assert.IsNotNull(storage);
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
        public void Given_Existing_10_Items_Top_Returns_Most_Recent_Item()
        {
            var items = BuildItems(count: 10);
            var latestItem = items.Last();
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItem = storage.Top();

            Assert.That(latestItem.Id, Is.EqualTo(gotItem.Id));
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetLatest_Returns_All_10_Items()
        {
            var noLimit = int.MaxValue;
            var items = BuildItems(count: 10);
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetLatest(noLimit).ToList();

            Assert.That(items.Count, Is.EqualTo(gotItems.Count));
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetLatest_Returns_Limited_Number_Of_5_Items()
        {
            var limit = 5;
            var items = BuildItems(count: 10);
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetLatest(limit).ToList();

            Assert.AreEqual(limit, gotItems.Count);
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetLatest_Returns_Them_Sorted_By_Date_Descending()
        {
            var noLimit = int.MaxValue;
            var items = BuildItems(count: 10);
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetLatest(noLimit).ToList();

            Assert.That(gotItems, Is.Ordered.Descending.By("Published"));
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetNewer_Returns_Items_Newer_Than_Given_Oldest_Item()
        {
            var items = BuildItems(count: 10);
            var oldestItem = items.First();
            var numberOfItems = items.Count - 1;
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetNewer(oldestItem, items.Count).ToList();

            Assert.AreEqual(numberOfItems, gotItems.Count);
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetNewer_Returns_Limited_Number_Of_5_Items_Newer_Than_Given_Oldest_Item()
        {
            var limit = 5;
            var items = BuildItems(count: 10);
            var oldestItem = items.First();
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetNewer(oldestItem, limit).ToList();

            Assert.AreEqual(limit, gotItems.Count);
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetNewer_Returns_Them_Sorted_By_Date_Descending()
        {
            var noLimit = int.MaxValue;
            var items = BuildItems(count: 10);
            var oldestItem = items.First();
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetNewer(oldestItem, noLimit).ToList();

            Assert.That(gotItems, Is.Ordered.Descending.By("Published"));
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetOlder_Returns_Items_Older_Than_Given_Newest_Item()
        {
            var items = BuildItems(count: 10);
            var newestItem = items.Last();
            var numberOfItems = items.Count - 1;
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetOlder(newestItem, items.Count).ToList();

            Assert.AreEqual(numberOfItems, gotItems.Count);
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetOlder_Returns_Limited_Number_Of_5_Items_Older_Than_Given_Newest_Item()
        {
            var limit = 5;
            var items = BuildItems(count: 10);
            var newestItem = items.Last();
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetOlder(newestItem, limit).ToList();

            Assert.AreEqual(limit, gotItems.Count);
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetOlder_Returns_Them_Sorted_By_Date_Descending()
        {
            var noLimit = int.MaxValue;
            var items = BuildItems(count: 10);
            var newestItem = items.Last();
            this.Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetOlder(newestItem, noLimit).ToList();

            Assert.That(gotItems, Is.Ordered.Descending.By("Published"));
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

        private static IList<Item> BuildItems(int count)
        {
            return new Fixture()
                .Build<Item>()
                .Without(i => i.Id)
                .Without(i => i.Tags)
                .Do(i => i.ItemType = (ItemType)(1 + new Random().Next(1)))
                .Do(i => i.Published = DateTime.Now.AddDays(new Random().Next(count)).AddHours(new Random().Next(count)))
                .CreateMany(count)
                .OrderBy(i => i.Published)
                .ToList();
        }
    }
}

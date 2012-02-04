using System;
using System.Collections.Generic;
using System.Linq;
using FluentMongo.Linq;
using MongoDB.Driver;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Services.Model;
using Services.Storage;
using Tests.Helpers;

namespace Tests.Services.Storage
{
    [TestFixture]
    public class StreamStorageTests
    {
        private const string ConnectionString = "mongodb://localhost";
        private const string DatabaseName = "Test";
        private const int NoLimit = -1;

        private MongoServer _server;

        [DB, Test]
        public void Given_MongoDB_Is_Running_ItemStorage_Can_Successfully_Connect()
        {
            new StreamStorage(ConnectionString, DatabaseName);
        }
        
        [DB, Test]
        public void Given_Existing_Item_Get_Returns_It_By_Id()
        {
            var item = BuildItems(count: 1).Single();
            Items.Insert(item);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItem = storage.Get(item.Id);

            Assert.IsNotNull(gotItem);
            Assert.AreEqual(item.Url, gotItem.Url);
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetLatest_Returns_All_10_Items()
        {
            var items = BuildItems(count: 10);
            Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetLatest(DateTime.MinValue, ItemType.Any, NoLimit).ToList();

            Assert.That(items.Count, Is.EqualTo(gotItems.Count));
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetLatest_Returns_Them_Sorted_By_Date_Descending()
        {
            var items = BuildItems(count: 10);
            Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetLatest(DateTime.MinValue, ItemType.Any, NoLimit).ToList();

            Assert.That(gotItems, Is.Ordered.Descending.By("Published"));
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetLatest_Returns_Items_Newer_Than_Given_Date()
        {
            var items = BuildItems(count: 10).OrderBy(i => i.Published);
            var fromDate = items.First().Published;
            var numberOfItems = items.Count(i => i.Published > fromDate);
            Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetLatest(fromDate, ItemType.Any, NoLimit).ToList();

            Assert.AreEqual(numberOfItems, gotItems.Count);
        }

        [DB, Test] 
        public void Given_Existing_10_Items_GetLatest_Returns_Rss_Items_Only()
        {
            var items = BuildItems(count: 10).OrderBy(i => i.Published);
            var numberOfRssItems = items.Count(i => i.ItemType == ItemType.Rss);
            Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetLatest(DateTime.MinValue, ItemType.Rss, NoLimit).ToList();

            Assert.AreEqual(numberOfRssItems, gotItems.Count);
        }

        [DB, Test]
        public void Given_Existing_10_Items_GetLatest_Returns_Limited_Number_Of_Items()
        {
            var limit = 1;
            var items = BuildItems(count: 10).OrderBy(i => i.Published);
            Items.InsertBatch(items);

            var storage = new StreamStorage(ConnectionString, DatabaseName);

            var gotItems = storage.GetLatest(DateTime.MinValue, ItemType.Any, limit).ToList();

            Assert.AreEqual(limit, gotItems.Count);
        }

        [DB, Test]
        public void Given_Existing_10_Items_Top_Returns_Most_Recent_Item()
        {
            var items = BuildItems(count: 10).OrderBy(i => i.Published);
            var recentItem = items.Last();
            Items.InsertBatch(items);

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

            var savedItems = Items.AsQueryable().ToList();

            Assert.That(items.Count, Is.EqualTo(savedItems.Count));
        }

        [DB, Test]
        public void Given_Null_Arguments_Constructor_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new StreamStorage(null, DatabaseName));
            Assert.Throws<ArgumentNullException>(() => new StreamStorage(ConnectionString, null));
        }

        [SetUp]
        public void SetupTest()
        {
            _server = MongoServer.Create(ConnectionString);
        }

        [TearDown]
        public void CleanupTest()
        {
            _server.DropDatabase(DatabaseName);
            _server.Disconnect();
        }

        private MongoCollection<Item> Items
        {
            get { return _server.GetDatabase(DatabaseName).GetCollection<Item>("Items"); }
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

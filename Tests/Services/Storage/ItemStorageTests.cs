using System;
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
    public class ItemStorageTests
    {
        public const string ConnectionString = "mongodb://localhost";
        public const string DatabaseName = "Test";
        
        private MongoServer _server;

        [Integration, Test]
        public void Given_MongoDB_Is_Running_ItemStorage_Can_Successfully_Connect()
        {
            var storage = new ItemStorage(ConnectionString, DatabaseName);
        }

        [Integration, Test]
        public void ItemStorage_Can_Save_10_New_Items()
        {
            var fixture = new Fixture();
            var items = fixture.Build<Item>().Without(i => i.Id).CreateMany(count: 10).ToList();
            
            var storage = new ItemStorage(ConnectionString, DatabaseName);

            storage.Save(items);

            var savedItems = Items.AsQueryable().ToList();

            Assert.That(items.Count, Is.EqualTo(savedItems.Count));
        }

        [Integration, Test]
        public void Given_Existing_10_Items_ItemStorage_Can_Get_All_10_Items()
        {
            var fixture = new Fixture();
            var items = fixture.Build<Item>().Without(i => i.Id).CreateMany(count: 10).ToList();
            Items.InsertBatch(items);

            var storage = new ItemStorage(ConnectionString, DatabaseName);

            var gotItems = storage.Get().ToList();

            Assert.That(items.Count, Is.EqualTo(gotItems.Count));
        }

        [Integration, Test]
        public void Given_Null_Arguments_Constructor_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new ItemStorage(null, DatabaseName));
            Assert.Throws<ArgumentNullException>(() => new ItemStorage(ConnectionString, null));
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
    }
}

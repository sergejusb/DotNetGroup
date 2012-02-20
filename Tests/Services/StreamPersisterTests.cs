namespace Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using MongoDB.Driver;

    using Moq;

    using NUnit.Framework;

    using Ploeh.AutoFixture;

    using global::Services;

    using global::Services.Generic;

    using global::Services.Model;

    using global::Services.Processors;

    using global::Services.Storage;

    using Tests.Helpers;

    [TestFixture]
    public class StreamPersisterTests
    {
        private static readonly string ConnectionString = ConfigurationManager.AppSettings["db.connection"];
        private static readonly string DatabaseName = ConfigurationManager.AppSettings["db.database"];

        [Test]
        public void Given_Null_Argument_Constructor_Throws()
        {
            var fakeStreamAggregator = new Mock<IItemAggregator>(MockBehavior.Loose).Object;
            var fakeItemProcessor = new Mock<IItemProcessor>(MockBehavior.Loose).Object;
            var fakeStreamStorage = new Mock<IStreamStorage>(MockBehavior.Loose).Object;

            Assert.Throws<ArgumentNullException>(() => new StreamPersister(null, fakeItemProcessor, fakeStreamStorage));
            Assert.Throws<ArgumentNullException>(() => new StreamPersister(fakeStreamAggregator, null, fakeStreamStorage));
            Assert.Throws<ArgumentNullException>(() => new StreamPersister(fakeStreamAggregator, fakeItemProcessor, null));
        }

        [Test]
        public void Persister_Can_Successfully_Persist_New_Items_In_Memory()
        {
            var items = this.BuildItems(numberOfFeeds: 10, numberOfTweets: 20);
            var oldItem = this.BuildItem(DateTime.MinValue);
            var savedItems = new List<Item>();

            var fakeStreamAggregator = new Mock<IItemAggregator>();
            fakeStreamAggregator.Setup(a => a.GetLatest(DateTime.MinValue)).Returns(items);

            var fakeStreamProcessor = new Mock<IItemProcessor>(MockBehavior.Loose);

            var fakeStreamStorage = new Mock<IStreamStorage>();
            fakeStreamStorage.Setup(s => s.Top()).Returns(oldItem);
            fakeStreamStorage.Setup(s => s.Save(It.IsAny<IEnumerable<Item>>())).Callback<IEnumerable<Item>>(savedItems.AddRange);

            var streamPersister = new StreamPersister(fakeStreamAggregator.Object, fakeStreamProcessor.Object, fakeStreamStorage.Object);
            streamPersister.PersistLatest();

            CollectionAssert.AreEquivalent(items, savedItems);

            fakeStreamAggregator.Verify(a => a.GetLatest(oldItem.Published), Times.Once());
            fakeStreamProcessor.Verify(p => p.Process(It.IsAny<Item>()), Times.Exactly(items.Count));
            fakeStreamStorage.Verify(s => s.Top(), Times.Once());
            fakeStreamStorage.Verify(s => s.Save(It.IsAny<IEnumerable<Item>>()), Times.Once());
        }

        [DB, Test]
        public void Persister_Can_Successfully_Persist_New_Items_In_Database()
        {
            try
            {
                var streamPersister = new StreamPersister(ConnectionString, DatabaseName);

                streamPersister.PersistLatest();
            }
            finally
            {
                MongoServer.Create(ConnectionString).DropDatabase(DatabaseName);
            }
        }

        [DB, Test]
        public void Persister_Can_Reprocess_Existing_Items_In_Memory()
        {
            var items = this.BuildItems(numberOfFeeds: 10, numberOfTweets: 20);
            
            var fakeStreamAggregator = new Mock<IItemAggregator>();
            var fakeStreamProcessor = new Mock<IItemProcessor>(MockBehavior.Loose);
            var fakeStreamStorage = new Mock<IStreamStorage>(MockBehavior.Loose);
            fakeStreamStorage.Setup(s => s.GetLatest(null, null, null, null)).Returns(items);

            var streamPersister = new StreamPersister(fakeStreamAggregator.Object, fakeStreamProcessor.Object, fakeStreamStorage.Object);
            streamPersister.Reprocess();

            fakeStreamProcessor.Verify(p => p.Process(It.IsAny<Item>()), Times.Exactly(items.Count));
            fakeStreamStorage.Verify(s => s.GetLatest(null, null, null, null), Times.Once());
            fakeStreamStorage.Verify(s => s.Save(It.IsAny<IEnumerable<Item>>()), Times.Once());
        }

        [DB, Test]
        public void Persister_Can_Reprocess_Existing_Items_In_Database()
        {
            try
            {
                var streamPersister = new StreamPersister(ConnectionString, DatabaseName);
                streamPersister.PersistLatest();
                
                streamPersister.Reprocess();
            }
            finally
            {
                MongoServer.Create(ConnectionString).DropDatabase(DatabaseName);
            }
        }

        private Item BuildItem(DateTime date)
        {
            return new Fixture()
                .Build<Item>()
                .Without(i => i.Id)
                .With(i => i.Tags, new List<string> { "ltnet" })
                .With(i => i.Published, date)
                .With(i => i.ItemType, ItemType.Rss)
                .CreateAnonymous();
        }

        private IList<Item> BuildItems(int numberOfFeeds, int numberOfTweets)
        {
            var items = new List<Item>();

            items.AddRange(new Fixture()
                .Build<Item>()
                .Without(i => i.Id)
                .With(i => i.Tags, new List<string> { "ASP.NET MVC", "Windows Azure" })
                .With(i => i.Published, DateTime.Now.AddDays(new Random().Next(numberOfFeeds)))
                .With(i => i.ItemType, ItemType.Rss)
                .CreateMany(numberOfFeeds));

            items.AddRange(new Fixture()
                .Build<Item>()
                .Without(i => i.Id)
                .With(i => i.Tags, new List<string> { "Hadoop 1.0", "Windows Azure" })
                .With(i => i.Published, DateTime.Now.AddDays(new Random().Next(numberOfTweets)))
                .With(i => i.ItemType, ItemType.Twitter)
                .CreateMany(numberOfTweets));

            return items;
        }
    }
}

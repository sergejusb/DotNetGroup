using System;
using System.Collections.Generic;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Services;
using Services.Generic;
using Services.Model;
using Services.Processors;
using Services.Storage;
using Tests.Helpers;

namespace Tests.Services
{
    [TestFixture]
    public class StreamPersisterTests
    {
        [Test]
        public void Persister_Can_Successfully_Persist_New_Items_In_Memory()
        {
            var items = BuildItems(numberOfFeeds: 10, numberOfTweets: 20);
            var oldItem = BuildItem(DateTime.MinValue);
            var savedItems = new List<Item>();

            var fakeStreamAggregator = new Mock<IItemAggregator>();
            fakeStreamAggregator.Setup(a => a.GetLatest(DateTime.MinValue)).Returns(items);

            var fakeItemProcessor = new Mock<IItemProcessor>(MockBehavior.Loose);

            var fakeStreamStorage = new Mock<IStreamStorage>();
            fakeStreamStorage.Setup(s => s.Top()).Returns(oldItem);
            fakeStreamStorage.Setup(s => s.Save(It.IsAny<IEnumerable<Item>>())).Callback<IEnumerable<Item>>(savedItems.AddRange);

            var streamPersister = new StreamPersister(fakeStreamAggregator.Object, fakeItemProcessor.Object, fakeStreamStorage.Object);
            streamPersister.PersistLatest();

            CollectionAssert.AreEquivalent(items, savedItems);

            fakeStreamAggregator.Verify(a => a.GetLatest(oldItem.Published), Times.Once());
            fakeItemProcessor.Verify(p => p.Process(It.IsAny<Item>()), Times.Exactly(items.Count));
            fakeStreamStorage.Verify(s => s.Top(), Times.Once());
            fakeStreamStorage.Verify(s => s.Save(It.IsAny<IEnumerable<Item>>()), Times.Once());
        }

        [DB, Test]
        public void Persister_Can_Successfully_Persist_New_Items_In_Database()
        {
            var connectionString = "mongodb://localhost";
            var databaseName = "Test";

            try
            {
                var streamPersister = new StreamPersister(connectionString, databaseName);

                streamPersister.PersistLatest();
            }
            finally
            {
                MongoServer.Create(connectionString).DropDatabase(databaseName);
            }
        }

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
                .CreateMany(numberOfFeeds)
            );

            items.AddRange(new Fixture()
                .Build<Item>()
                .Without(i => i.Id)
                .With(i => i.Tags, new List<string> { "Hadoop 1.0", "Windows Azure" })
                .With(i => i.Published, DateTime.Now.AddDays(new Random().Next(numberOfTweets)))
                .With(i => i.ItemType, ItemType.Twitter)
                .CreateMany(numberOfTweets)
            );

            return items;
        }
    }
}

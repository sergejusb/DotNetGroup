namespace DotNetGroup.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using DotNetGroup.Services;
    using DotNetGroup.Services.Model;
    using DotNetGroup.Services.Processors;
    using DotNetGroup.Services.Storage;
    using DotNetGroup.Tests.Helpers;

    using MongoDB.Driver;

    using Moq;

    using NUnit.Framework;

    using Ploeh.AutoFixture;

    [TestFixture]
    public class StreamReprocessorTests
    {
        private static readonly string ConnectionString = ConfigurationManager.AppSettings["db.connection"];
        private static readonly string DatabaseName = ConfigurationManager.AppSettings["db.database"];

        [DB, Test]
        public void Can_Reprocess_Existing_Items_In_Memory()
        {
            var items = this.BuildItems(numberOfFeeds: 10, numberOfTweets: 20);

            var fakeStreamProcessor = new Mock<IItemProcessor>(MockBehavior.Loose);
            var fakeStreamStorage = new Mock<IStreamStorage>(MockBehavior.Loose);
            fakeStreamStorage.Setup(s => s.GetLatest(null, null, null, null)).Returns(items);

            var streamPersister = new StreamReprocessor(fakeStreamProcessor.Object, fakeStreamStorage.Object);
            streamPersister.Reprocess();

            fakeStreamProcessor.Verify(p => p.Process(It.IsAny<Item>()), Times.Exactly(items.Count));
            fakeStreamStorage.Verify(s => s.GetLatest(null, null, null, null), Times.Once());
            fakeStreamStorage.Verify(s => s.Save(It.IsAny<IEnumerable<Item>>()), Times.Once());
        }

        [DB, Test]
        public void Can_Reprocess_Existing_Items_In_Database()
        {
            try
            {
                var streamPersister = new StreamReprocessor(ConnectionString, DatabaseName);

                streamPersister.Reprocess();
            }
            finally
            {
                MongoServer.Create(ConnectionString).DropDatabase(DatabaseName);
            }
        }

        private IList<Item> BuildItems(int numberOfFeeds, int numberOfTweets)
        {
            var items = new List<Item>();

            items.AddRange(new Fixture()
                .Build<Item>()
                .Without(i => i.Id)
                .With(i => i.Tags, new[] { "ASP.NET MVC", "Windows Azure" })
                .With(i => i.Published, DateTime.Now.AddDays(new Random().Next(numberOfFeeds)))
                .With(i => i.ItemType, ItemType.Rss)
                .CreateMany(numberOfFeeds));

            items.AddRange(new Fixture()
                .Build<Item>()
                .Without(i => i.Id)
                .With(i => i.Tags, new[] { "Hadoop 1.0", "Windows Azure" })
                .With(i => i.Published, DateTime.Now.AddDays(new Random().Next(numberOfTweets)))
                .With(i => i.ItemType, ItemType.Twitter)
                .CreateMany(numberOfTweets));

            return items;
        }
    }
}
using System;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using Services;
using Services.Model;
using Services.Storage;
using Tests.Helpers;

namespace Tests.Services
{
    [TestFixture]
    public class StreamApiTests
    {
        [Test]
        public void Given_Null_Argument_Constructor_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new StreamApi(null));
        }

        [DB, Test]
        public void StreamApi_Can_Connect_To_Database()
        {
            var connectionString = "mongodb://localhost";
            var databaseName = "Test";

            try
            {
                var streamApi = new StreamApi(connectionString, databaseName);
                streamApi.Get(DateTime.MinValue);
            }
            finally
            {
                MongoServer.Create(connectionString).DropDatabase(databaseName);
            }
        }

        [Test]
        public void Given_Valid_Id_Api_Returns_Item()
        {
            var id = new Item().Id;
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApi(fakeStorage.Object);

            streamApi.Get(id);

            fakeStorage.Verify(s => s.Get(id), Times.Once());
        }

        [Test]
        public void Given_Date_Api_Returns_Items()
        {
            var limit = 10;
            var dateFrom = DateTime.UtcNow.AddDays(-1);
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApi(fakeStorage.Object, limit);

            streamApi.Get(dateFrom);

            fakeStorage.Verify(s => s.GetLatest(dateFrom, null, limit), Times.Once());
        }

        [Test]
        public void Given_Date_Api_Returns_Rss_Items()
        {
            var limit = 10;
            var dateFrom = DateTime.UtcNow.AddDays(-1);
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApi(fakeStorage.Object, limit);

            streamApi.Get(dateFrom, ItemType.Rss);

            fakeStorage.Verify(s => s.GetLatest(dateFrom, ItemType.Rss, limit), Times.Once());
        }

        [Test]
        public void Given_Date_And_Limit_Api_Returns_Limited_Number_Of_Items()
        {
            var limit = 10;
            var dateFrom = DateTime.UtcNow.AddDays(-1);
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApi(fakeStorage.Object);

            streamApi.Get(dateFrom, limit);

            fakeStorage.Verify(s => s.GetLatest(dateFrom, null, limit), Times.Once());
        }

        [Test]
        public void Given_Date_And_Limit_Api_Returns_Limited_Number_Of_Tweets()
        {
            var limit = 10;
            var dateFrom = DateTime.UtcNow.AddDays(-1);
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApi(fakeStorage.Object);

            streamApi.Get(dateFrom, ItemType.Twitter, limit);

            fakeStorage.Verify(s => s.GetLatest(dateFrom, ItemType.Twitter, limit), Times.Once());
        }
    }
}

namespace Tests.Api
{
    using System;

    using global::Api.Controllers;

    using MongoDB.Driver;

    using Moq;

    using NUnit.Framework;

    using global::Services.Model;

    using global::Services.Storage;

    using Tests.Helpers;

    [TestFixture]
    public class StreamApiControllerTests
    {
        [Test]
        public void StreamApiControler_Can_Be_Created_With_Default_Contructor()
        {
            new StreamApiController();
        }

        [Test]
        public void Given_Null_Argument_Constructor_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new StreamApiController(null));
        }

        [DB, Test]
        public void StreamApiController_Can_Connect_To_Database()
        {
            var connectionString = "mongodb://localhost";
            var databaseName = "Test";

            try
            {
                var streamApi = new StreamApiController(connectionString, databaseName);
                streamApi.Stream();
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
            var streamApi = new StreamApiController(fakeStorage.Object);

            streamApi.Get(id);

            fakeStorage.Verify(s => s.Get(id), Times.Once());
        }

        [Test]
        public void Given_Date_Api_Returns_Items()
        {
            var maxItems = 100;
            var dateFrom = DateTime.UtcNow.AddDays(-1);
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApiController(fakeStorage.Object);

            streamApi.Stream(dateFrom);

            fakeStorage.Verify(s => s.GetLatest(dateFrom, null, maxItems), Times.Once());
        }

        [Test]
        public void Given_Date_Api_Returns_Rss_Items()
        {
            var maxItems = 100;
            var dateFrom = DateTime.UtcNow.AddDays(-1);
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApiController(fakeStorage.Object);

            streamApi.Stream(dateFrom, ItemType.Rss);

            fakeStorage.Verify(s => s.GetLatest(dateFrom, ItemType.Rss, maxItems), Times.Once());
        }

        [Test]
        public void Given_Date_And_Limit_Api_Returns_Limited_Number_Of_Items()
        {
            var limit = 10;
            var dateFrom = DateTime.UtcNow.AddDays(-1);
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApiController(fakeStorage.Object);

            streamApi.Stream(dateFrom, limit: limit);

            fakeStorage.Verify(s => s.GetLatest(dateFrom, null, limit), Times.Once());
        }

        [Test]
        public void Given_Date_And_Limit_Api_Returns_Limited_Number_Of_Tweets()
        {
            var limit = 10;
            var dateFrom = DateTime.UtcNow.AddDays(-1);
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApiController(fakeStorage.Object);

            streamApi.Stream(dateFrom, ItemType.Twitter, limit);

            fakeStorage.Verify(s => s.GetLatest(dateFrom, ItemType.Twitter, limit), Times.Once());
        }
    }
}

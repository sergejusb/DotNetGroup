namespace Tests.Api
{
    using System;
    using System.Configuration;

    using global::Api.Controllers;
    using global::Api.Models;

    using MongoDB.Driver;

    using Moq;

    using NUnit.Framework;

    using global::Services.Model;

    using global::Services.Storage;

    using Tests.Helpers;

    [TestFixture]
    public class StreamApiControllerTests
    {
        private static readonly string ConnectionString = ConfigurationManager.AppSettings["db.connection"];
        private static readonly string DatabaseName = ConfigurationManager.AppSettings["db.database"];

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
            try
            {
                var streamApi = new StreamApiController(ConnectionString, DatabaseName);
                streamApi.Stream(new StreamFilter());
            }
            finally
            {
                MongoServer.Create(ConnectionString).DropDatabase(DatabaseName);
            }
        }

        [Test]
        public void Given_Valid_Id_Api_Returns_Item()
        {
            var filter = new GetFilter { Id = new Item().Id };
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApiController(fakeStorage.Object);

            streamApi.Get(filter);

            fakeStorage.Verify(s => s.Get(filter.Id), Times.Once());
        }

        [Test]
        public void With_No_Arguments_Api_Returns_Items()
        {
            var filter = new StreamFilter();
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApiController(fakeStorage.Object);

            streamApi.Stream(filter);

            fakeStorage.Verify(s => s.GetLatest(filter.Type, filter.From, filter.To, filter.Limit), Times.Once());
        }

        [Test]
        public void Given_Stream_Filter_Api_Returns_Items()
        {
            var filter = new StreamFilter
            {
                Type = ItemType.Twitter,
                From = DateTime.UtcNow.AddDays(-1),
                Limit = 10
            };
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApiController(fakeStorage.Object);

            streamApi.Stream(filter);

            fakeStorage.Verify(s => s.GetLatest(filter.Type, filter.From, filter.To, filter.Limit), Times.Once());
        }
        
        [Test]
        public void Given_Stream_Filter_Api_Returns_Items_Count()
        {
            var filter = new StreamFilter
            {
                Type = ItemType.Twitter,
                From = DateTime.UtcNow.AddDays(-1),
                Limit = 10
            };
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApiController(fakeStorage.Object);

            streamApi.Count(filter);

            fakeStorage.Verify(s => s.Count(filter.Type, filter.From, filter.To, filter.Limit), Times.Once());
        }
    }
}

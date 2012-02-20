namespace Tests.Api
{
    using System;
    using System.Configuration;
    using System.Web.Mvc;

    using global::Api.Controllers;
    using global::Api.Extensions;
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
        public void Given_Date_Api_Returns_Items()
        {
            var filter = new StreamFilter
            {
                From = DateTime.UtcNow.AddDays(-1)
            };
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApiController(fakeStorage.Object);

            streamApi.Stream(filter);

            fakeStorage.Verify(s => s.GetLatest(filter.Type, filter.From, filter.To, filter.Limit), Times.Once());
        }

        [Test]
        public void Given_Date_Api_Returns_Rss_Items()
        {
            var filter = new StreamFilter
            {
                Type = ItemType.Rss,
                From = DateTime.UtcNow.AddDays(-1)
            };
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApiController(fakeStorage.Object);

            streamApi.Stream(filter);

            fakeStorage.Verify(s => s.GetLatest(filter.Type, filter.From, filter.To, filter.Limit), Times.Once());
        }

        [Test]
        public void Given_Date_And_Limit_Api_Returns_Limited_Number_Of_Items()
        {
            var filter = new StreamFilter
            {
                From = DateTime.UtcNow.AddDays(-1),
                Limit = 10
            };
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApiController(fakeStorage.Object);

            streamApi.Stream(filter);

            fakeStorage.Verify(s => s.GetLatest(filter.Type, filter.From, filter.To, filter.Limit), Times.Once());
        }

        [Test]
        public void Given_Date_And_Limit_Api_Returns_Limited_Number_Of_Tweets()
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
        public void Given_No_Callback_For_Get_Json_Result_Is_Returned()
        {
            var filter = new GetFilter();
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApiController(fakeStorage.Object);

            var result = streamApi.Get(filter);

            Assert.IsInstanceOf<JsonResult>(result);
        }

        [Test]
        public void Given_Callback_For_Get_Jsonp_Result_Is_Returned()
        {
            var filter = new GetFilter
            {
                Callback = "callback"
            };
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApiController(fakeStorage.Object);

            var result = streamApi.Get(filter);

            Assert.IsInstanceOf<JsonpResult>(result);
        }

        [Test]
        public void Given_No_Callback_For_Stream_Json_Result_Is_Returned()
        {
            var filter = new StreamFilter();
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApiController(fakeStorage.Object);

            var result = streamApi.Stream(filter);

            Assert.IsInstanceOf<JsonResult>(result);
        }

        [Test]
        public void Given_Callback_For_Stream_Jsonp_Result_Is_Returned()
        {
            var filter = new StreamFilter
            {
                Callback = "callback"
            };
            var fakeStorage = new Mock<IStreamStorage>();
            var streamApi = new StreamApiController(fakeStorage.Object);

            var result = streamApi.Stream(filter);

            Assert.IsInstanceOf<JsonpResult>(result);
        }
    }
}

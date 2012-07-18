namespace Tests.Api
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;

    using global::Api.Controllers;
    using global::Api.Models;

    using Moq;

    using NUnit.Framework;

    using global::Services.Model;

    using global::Services.Storage;

    [TestFixture]
    public class StreamControllerTests
    {
        [Test]
        public void StreamApiControler_Can_Be_Created_With_Default_Contructor()
        {
            var controller = new StreamController();

            Assert.IsNotNull(controller);
        }

        [Test]
        public void Given_Null_Argument_Constructor_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new StreamController(null));
        }

        [Test]
        public void Given_Valid_Id_Api_Returns_Item()
        {
            var filter = new StreamFilter { Id = new Item().Id };
            var fakeStorage = new Mock<IStreamStorage>();
            fakeStorage.Setup(s => s.Get(It.IsAny<string>())).Returns(new Item());
            var streamApi = this.BuildStreamController(fakeStorage.Object);

            streamApi.Get(filter);

            fakeStorage.Verify(s => s.Get(filter.Id), Times.Once());
        }

        [Test]
        public void With_No_Arguments_Api_Returns_Items()
        {
            var filter = new StreamFilter();
            var fakeStorage = new Mock<IStreamStorage>();
            fakeStorage.Setup(s => s.GetLatest(filter.Type, filter.From, filter.To, filter.Limit)).Returns(new List<Item>());
            var streamApi = this.BuildStreamController(fakeStorage.Object);

            streamApi.Get(filter);

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
            var streamApi = this.BuildStreamController(fakeStorage.Object);

            streamApi.Get(filter);

            fakeStorage.Verify(s => s.GetLatest(filter.Type, filter.From, filter.To, filter.Limit), Times.Once());
        }

        private StreamController BuildStreamController(IStreamStorage fakeStorage)
        {
            return new StreamController(fakeStorage)
            {
                Request = new HttpRequestMessage
                {
                    Properties =
                    {
                        { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() }
                    }
                }
            };
        }
    }
}

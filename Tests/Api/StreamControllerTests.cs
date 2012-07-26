namespace DotNetGroup.Tests.Api
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;

    using DotNetGroup.Api.Controllers;
    using DotNetGroup.Api.Models;
    using DotNetGroup.Services.Model;
    using DotNetGroup.Services.Storage;

    using Moq;

    using NUnit.Framework;

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
        public void Given_Filter_With_Id_Get_Is_Called()
        {
            var filter = new StreamFilter { Id = "Id" };
            var fakeStorage = new Mock<IStreamStorage>();
            fakeStorage.Setup(s => s.Get(It.IsAny<string>())).Returns(new Item());
            var streamApi = this.BuildStreamController(fakeStorage.Object);

            streamApi.Get(filter);

            fakeStorage.Verify(s => s.Get(filter.Id), Times.Once());
        }

        [Test]
        public void Given_Empty_Filter_GetLatest_Is_Called()
        {
            var filter = new StreamFilter();
            var fakeStorage = new Mock<IStreamStorage>();
            fakeStorage.Setup(s => s.GetLatest(It.IsAny<int>())).Returns(new List<Item>());
            var streamApi = this.BuildStreamController(fakeStorage.Object);

            streamApi.Get(filter);

            fakeStorage.Verify(s => s.GetLatest(filter.Limit), Times.Once());
        }

        [Test]
        public void Given_Filter_With_Max_Id_GetOlder_Is_Called()
        {
            var item = new Item();
            var filter = new StreamFilter { Max_Id = "Max_Id" };
            var fakeStorage = new Mock<IStreamStorage>();
            fakeStorage.Setup(s => s.Get(It.IsAny<string>())).Returns(item);
            fakeStorage.Setup(s => s.GetOlder(It.IsAny<Item>(), It.IsAny<int>())).Returns(new List<Item>());
            var streamApi = this.BuildStreamController(fakeStorage.Object);

            streamApi.Get(filter);

            fakeStorage.Verify(s => s.GetOlder(item, filter.Limit), Times.Once());
        }

        [Test]
        public void Given_Filter_With_Min_Id_GetNewer_Is_Called()
        {
            var item = new Item();
            var filter = new StreamFilter { Min_Id = "Min_Id" };
            var fakeStorage = new Mock<IStreamStorage>();
            fakeStorage.Setup(s => s.Get(It.IsAny<string>())).Returns(item);
            fakeStorage.Setup(s => s.GetNewer(It.IsAny<Item>(), It.IsAny<int>())).Returns(new List<Item>());
            var streamApi = this.BuildStreamController(fakeStorage.Object);

            streamApi.Get(filter);

            fakeStorage.Verify(s => s.GetNewer(item, filter.Limit), Times.Once());
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

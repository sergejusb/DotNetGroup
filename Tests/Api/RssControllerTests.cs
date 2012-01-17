using System;
using System.Collections.Generic;
using Api.Controllers;
using Moq;
using NUnit.Framework;
using Services.Generic;
using Services.Model;

namespace Tests.Api
{
    [TestFixture]
    public class RssControllerTests
    {
        [Test]
        public void RssControler_Can_Be_Created_With_Default_Contructor()
        {
            new RssController();
        }

        [Test]
        public void By_Calling_Json_Action_GetLatestTweets_Is_Called_Once()
        {
            var rssAggregatorFake = new Mock<IItemAggregator>();
            rssAggregatorFake.Setup(a => a.GetLatest(It.IsAny<DateTime>())).Returns(new List<Item>());
            var rssController = new RssController(rssAggregatorFake.Object);

            rssController.Json(null);

            rssAggregatorFake.Verify(a => a.GetLatest(It.IsAny<DateTime>()), Times.Once());
        }
    }
}

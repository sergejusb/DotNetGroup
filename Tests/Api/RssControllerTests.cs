using System.Collections.Generic;
using Api.Controllers;
using Moq;
using NUnit.Framework;
using Services.Rss;

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
            var rssAggregatorFake = new Mock<IRssAggregator>();
            rssAggregatorFake.Setup(a => a.GetLatestFeeds(It.IsAny<int>())).Returns(new List<Feed>());
            var rssController = new RssController(rssAggregatorFake.Object);

            rssController.Json();

            rssAggregatorFake.Verify(a => a.GetLatestFeeds(It.IsAny<int>()), Times.Once());
        }
    }
}

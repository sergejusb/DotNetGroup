using System.Collections.Generic;
using Api.Controllers;
using Moq;
using NUnit.Framework;
using Services.Twitter;

namespace Tests.Api
{
    [TestFixture]
    public class TwitterControllerTests
    {
        [Test]
        public void TwitterControler_Can_Be_Created_With_Default_Contructor()
        {
            new TwitterController();
        }

        [Test]
        public void By_Calling_Json_Action_GetLatestTweets_Is_Called_Once()
        {
            var twitterAggregatorFake = new Mock<ITwitterAggregator>();
            twitterAggregatorFake.Setup(a => a.GetLatestTweets(It.IsAny<int>())).Returns(new List<Tweet>());
            var twitterController = new TwitterController(twitterAggregatorFake.Object);

            twitterController.Json();

            twitterAggregatorFake.Verify(a => a.GetLatestTweets(It.IsAny<int>()), Times.Once());
        }
    }
}
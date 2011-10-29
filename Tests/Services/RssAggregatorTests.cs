using Moq;
using NUnit.Framework;
using Services.Rss;

namespace Tests.Services
{
    [TestFixture]
    public class RssAggregatorTests
    {
        [Test]
        public void Given_Existing_Feeds_GetLatestFeeds_Returns_Feeds_In_Correct_Order()
        {
            var rssServiceFake = new Mock<IRssService>();
            
        }
    }
}

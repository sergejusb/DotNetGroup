namespace DotNetGroup.Tests.Api
{
    using DotNetGroup.Api.Models;

    using NUnit.Framework;

    [TestFixture]
    public class StreamFilterTests
    {
        [Test]
        public void Given_No_From_Date_StreamFilter_Returns_Non_Null_Date()
        {
            var filter = new StreamFilter();

            Assert.IsNotNull(filter.From);
        }

        [Test]
        public void Given_No_Limit_StreamFilter_Returns_Default_Limit()
        {
            var filter = new StreamFilter();

            Assert.That(filter.Limit, Is.EqualTo(StreamFilter.DefaultLimit));
        }

        [Test]
        public void Given_Bigger_Limit_Than_Maximum_Allowed_Later_Is_Used()
        {
            var filter = new StreamFilter { Limit = StreamFilter.MaxAllowedLimit + 1 };

            Assert.That(filter.Limit, Is.EqualTo(StreamFilter.MaxAllowedLimit));
        }
    }
}
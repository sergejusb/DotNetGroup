namespace Tests.Api
{
    using global::Api.Models;

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
        public void Given_No_Limit_StreamFilter_Returns_Non_Null_Limit()
        {
            var filter = new StreamFilter();

            Assert.IsNotNull(filter.Limit);
        }
    }
}
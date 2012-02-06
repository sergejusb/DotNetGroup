namespace Tests.Api
{
    using NUnit.Framework;

    using global::Api.Controllers;
    using global::Api.Models;

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

            Assert.IsNotNull(filter.From);
        }
    }
}
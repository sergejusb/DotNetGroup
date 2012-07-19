namespace DotNetGroup.Tests.Services.Twitter
{
    using System.Configuration;
    using System.Linq;

    using DotNetGroup.Services.Twitter;

    using NUnit.Framework;

    [TestFixture]
    public class QueryConfigProviderTests
    {
        [Test]
        public void Given_AppConfig_Has_Twitter_Query_GetValues_Successfully_Returens_It()
        {
            var numberOfTwitterQueries = 1;
            Assert.IsNotNullOrEmpty(ConfigurationManager.AppSettings["twitter.query"]);

            var queryProvider = new QueryConfigProvider();

            var queries = queryProvider.GetValues();

            Assert.IsNotNull(queries);
            Assert.AreEqual(numberOfTwitterQueries, queries.Count());
        }
    }
}

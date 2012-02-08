namespace Tests.Services.Web
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using global::Services.Model;
    using global::Services.Web;

    [TestFixture]
    public class JsonClientTests
    {
        [Test]
        public void Given_Valid_Url_Api_Returns_Deserialized_Items()
        {
            var url = "http://api.dotnetgroup.dev/?type=rss";

            var items = new JsonClient().Get<IEnumerable<Item>>(url);

            CollectionAssert.IsNotEmpty(items);
        }
    }
}

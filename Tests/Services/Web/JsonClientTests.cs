namespace DotNetGroup.Tests.Services.Web
{
    using System.Collections.Generic;

    using DotNetGroup.Services.Model;
    using DotNetGroup.Services.Web;

    using NUnit.Framework;

    [TestFixture]
    public class JsonClientTests
    {
        [Test]
        public void Given_Valid_Url_Api_Returns_Deserialized_Items()
        {
            var url = "http://api.dotnetgroup.dev/v1/stream";

            var items = new JsonClient().Get<IEnumerable<Item>>(url);

            CollectionAssert.IsNotEmpty(items);
        }

        [Test]
        public void Given_Valid_Url_Api_Returns_Serialized_Items()
        {
            var url = "http://api.dotnetgroup.dev/v1/stream";

            var json = new JsonClient().Get(url);

            CollectionAssert.IsNotEmpty(json);
        }
    }
}

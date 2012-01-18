using NUnit.Framework;
using Services.Web;

namespace Tests.Services.Web
{
    [TestFixture]
    public class UrlResolverTests
    {
        [Test]
        public void Given_Shorten_Url_Resolve_Successfully_Resolves_Orignial_Url()
        {
            var shortenUrl = "http://t.co/53rFHuMG";
            var originalUrl = "http://sergejus.blogas.lt/itishnikai-7-jau-online-1586.html";
            var resolver = new UrlResolver();

            var resolvedUrl = resolver.Resolve(shortenUrl);

            Assert.AreEqual(originalUrl, resolvedUrl);
        }

        [Test]
        public void Given_Full_Url_Resolve_Sucsessfully_Returns_Same_Url()
        {
            var originalUrl = "http://sergejus.blogas.lt/itishnikai-7-jau-online-1586.html";
            var resolver = new UrlResolver();

            var resolvedUrl = resolver.Resolve(originalUrl);

            Assert.AreEqual(originalUrl, resolvedUrl);

        }

        [Test]
        public void Given_Web_Exception_Occures_Resolve_Returns_Original_Url()
        {
            var invalidUrl = "http";
            var resolver = new UrlResolver();

            var resolvedUrl = resolver.Resolve(invalidUrl);

            Assert.AreEqual(invalidUrl, resolvedUrl);
        }
    }
}

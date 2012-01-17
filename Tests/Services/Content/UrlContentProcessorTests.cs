using NUnit.Framework;
using Services.Content;

namespace Tests.Services.Content
{
    [TestFixture]
    public class UrlContentProcessorTests
    {
        [Test]
        public void Given_Tweet_With_The_Sorten_Url_Content_After_Processing_Href_Tag_Is_Successfully_Replaced_With_The_Original()
        {
            var contentBeforeProcessing = "ITishnikai #7 jau online! Svečiuose Romualdas (@<a class=\" \" href=\"http://twitter.com/rstonkus\">rstonkus</a>) Stonkus <em><a href=\"http://search.twitter.com/search?q=%23ltnet\" title=\"#ltnet\" class=\" \">#ltnet</a></em> <a href=\"http://t.co/lzi51BTM\">http://t.co/lzi51BTM</a>";
            var contentAfterProcessing = "ITishnikai #7 jau online! Svečiuose Romualdas (@<a class=\" \" href=\"http://twitter.com/rstonkus\">rstonkus</a>) Stonkus <em><a href=\"http://search.twitter.com/search?q=%23ltnet\" title=\"#ltnet\" class=\" \">#ltnet</a></em> <a href=\"http://t.co/lzi51BTM\">http://sergejus.blogas.lt/itishnikai-7-jau-online-1586.html</a>";
            var processor = new UrlContentProcessor();

            var result = processor.Process(contentBeforeProcessing);

            Assert.AreEqual(contentAfterProcessing, result);
        }
    }
}

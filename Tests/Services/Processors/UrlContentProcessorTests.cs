using NUnit.Framework;
using Services.Model;
using Services.Processors;

namespace Tests.Services.Processors
{
    [TestFixture]
    public class UrlContentProcessorTests
    {
        [Test]
        public void Given_Tweet_With_The_Sorten_Url_Content_After_Processing_Href_Tag_Is_Successfully_Replaced_With_The_Original()
        {
            var contentBeforeProcessing = "ITishnikai #7 jau online! Svečiuose Romualdas (@<a class=\" \" href=\"http://twitter.com/rstonkus\">rstonkus</a>) Stonkus <em><a href=\"http://search.twitter.com/search?q=%23ltnet\" title=\"#ltnet\" class=\" \">#ltnet</a></em> <a href=\"http://t.co/lzi51BTM\">http://t.co/lzi51BTM</a>";
            var contentAfterProcessing = "ITishnikai #7 jau online! Svečiuose Romualdas (@<a class=\" \" href=\"http://twitter.com/rstonkus\">rstonkus</a>) Stonkus <em><a href=\"http://search.twitter.com/search?q=%23ltnet\" title=\"#ltnet\" class=\" \">#ltnet</a></em> <a href=\"http://t.co/lzi51BTM\">http://sergejus.blogas.lt/itishnikai-7-jau-online-1586.html</a>";

            var item = new Item { Content = contentBeforeProcessing };
            
            new UrlContentProcessor().Process(item);

            Assert.AreEqual(contentAfterProcessing, item.Content);
        }

        [Test]
        public void Given_Content_With_No_Url_Original_Content_Is_Returned()
        {
            var content = "ITishnikai #7 jau online!";
            
            var item = new Item { Content = content };

            new UrlContentProcessor().Process(item);

            Assert.AreEqual(content, item.Content);
        }
    }
}

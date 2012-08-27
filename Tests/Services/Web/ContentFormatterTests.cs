namespace DotNetGroup.Tests.Services.Web
{
    using DotNetGroup.Services.Web;

    using NUnit.Framework;

    [TestFixture]
    public class ContentFormatterTests
    {
        [Test]
        public void Given_Url_Text_Exeeds_Max_Allowed_Length_It_Is_Shortened()
        {
            var contentBeforeFormatting = "<a href=\"http://t.co/lzi51BTM\">http://sergejus.blogas.lt/itishnikai-7-jau-online-1586.html</a>";
            var contentAfterFormatting = "<a href=\"http://t.co/lzi51BTM\" title=\"http://sergejus.blogas.lt/itishnikai-7-jau-online-1586.html\">sergejus.blogas.lt/../itishnikai-7-jau-online-1586.html</a>";

            var contentFormatter = new ContentFormatter(maxUrlTextLength: 30);

            var formattedContent = contentFormatter.Format(contentBeforeFormatting);

            Assert.That(formattedContent, Is.EqualTo(contentAfterFormatting));
        }

        [Test]
        public void Given_Url_Text_Does_Not_Exceed_Max_Allowed_Length_Title_Attribute_Is_Added_And_Http_Scheme_Removed()
        {
            var contentBeforeFormatting = "<a href=\"http://t.co/lzi51BTM\">http://sergejus.blogas.lt/itishnikai-7-jau-online-1586.html</a>";
            var contentAfterFormatting = "<a href=\"http://t.co/lzi51BTM\" title=\"http://sergejus.blogas.lt/itishnikai-7-jau-online-1586.html\">sergejus.blogas.lt/itishnikai-7-jau-online-1586.html</a>";

            var contentFormatter = new ContentFormatter(maxUrlTextLength: 100);

            var formattedContent = contentFormatter.Format(contentBeforeFormatting);

            Assert.That(formattedContent, Is.EqualTo(contentAfterFormatting));
        }
    }
}

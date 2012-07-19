namespace DotNetGroup.Tests.Services.Processors
{
    using DotNetGroup.Services.Model;
    using DotNetGroup.Services.Processors;

    using NUnit.Framework;

    [TestFixture]
    public class HtmlProcessorTests
    {
        private const string FirstParagraph = "<p>This is first para with <a href=\"#\">a link</a></p>";
        private const string SecondParagraph = "<p>This is second para with <ul><li>unordered</li><li>list</li></ul></p>";
        private const string ThirdParagraph = "<p>This is third para with the <p>inner para</p></p>";
        private const string Content = FirstParagraph + SecondParagraph + ThirdParagraph;

        [Test]
        public void Given_25_Simbol_Limit_Only_First_Para_Is_Returned()
        {
            var limit = 25;
            var processedContent = "<p>This is first para with <a href=\"#\">a link</a>[...]</p>";
            var item = new Item { Content = Content };

            new HtmlProcessor(limit).Process(item);

            Assert.AreEqual(processedContent, item.Content);
        }

        [Test]
        public void Given_50_Simbol_Limit_First_Two_Paragraphs_Are_Returned()
        {
            var limit = 50;
            var processedContent = "<p>This is first para with <a href=\"#\">a link</a></p>" +
                                   "<p>This is second para with <ul><li>unordered</li><li>list</li></ul>[...]</p>";
            var item = new Item { Content = Content };

            new HtmlProcessor(limit).Process(item);

            Assert.AreEqual(processedContent, item.Content);
        }

        [Test]
        public void Given_1000_Simbol_Limit_All_Content_Is_Returned()
        {
            var limit = 1000;
            var item = new Item { Content = Content };

            new HtmlProcessor(limit).Process(item);

            Assert.AreEqual(Content, item.Content);   
        }

        [Test]
        public void Given_Not_Valid_First_Tag_All_Content_Is_Returned()
        {
            var limit = 25;
            var content = "GitHub for Windows - <a href=\"https://t.co/4cE6M3co\">https://github.com/blog/1127-github-for-windows</a> <a href=\"http://search.twitter.com/search?q=%23github\" title=\"#github\" class=\" \">#github</a> <a href=\"http://search.twitter.com/search?q=%23git\" title=\"#git\" class=\" \">#git</a> <em><a href=\"http://search.twitter.com/search?q=%23ltnet\" \">#ltnet</a></em>";
            var item = new Item { Content = content };

            new HtmlProcessor(limit).Process(item);

            Assert.AreEqual(content, item.Content);
        }

        [Test]
        public void Given_Text_Node_Between_Element_Nodes_It_Is_Processed()
        {
            var content = "<p>This is just </p> <p>paragraph</p>";
            var processedContent = "<p>This is just </p><p>paragraph</p>";

            var item = new Item { Content = content };

            new HtmlProcessor().Process(item);

            Assert.AreEqual(processedContent, item.Content);
        }

        [Test]
        public void Given_Not_Valid_Tag_After_Paragraph_It_Is_Handled()
        {
            var content = "<p>This is just </p> <ul> <li>paragraph</li> </ul>";

            var item = new Item { Content = content };

            new HtmlProcessor().Process(item);

            Assert.AreEqual("<p>This is just [...]</p>", item.Content);
        }
    }
}

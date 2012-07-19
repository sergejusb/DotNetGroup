namespace DotNetGroup.Tests.Services.Processors
{
    using DotNetGroup.Services.Model;
    using DotNetGroup.Services.Processors;

    using NUnit.Framework;

    [TestFixture]
    public class FacebookProcessorTests
    {
        [Test]
        public void Given_Tweet_With_No_Frames_Content_Is_Returned()
        {
            var content = "ITishnikai #7 jau online!";

            var item = new Item { Content = content };

            new FacebookProcessor().Process(item);

            Assert.AreEqual(content, item.Content);
        }

        [Test]
        public void Given_Tweet_With_Non_Facebook_Frame_Content_Is_Returned()
        {
            var content = "ITishnikai #7 jau online! <iframe height=\"355\" marginheight=\"0\" src=\"http://www.slideshare.net/slideshow/embed_code/9593236?rel=0\" frameborder=\"0\" width=\"425\" marginwidth=\"0\" scrolling=\"no\"></iframe>";

            var item = new Item { Content = content };

            new FacebookProcessor().Process(item);

            Assert.AreEqual(content, item.Content);
        }

        [Test]
        public void Given_Tweet_With_Facebook_Frame_Content_Is_Returned()
        {
            var contentBeforeProcessing = "ITishnikai #7 jau online! <p><iframe src=\"http://www.facebook.com/plugins/like.php?locale=lt_LT&amp;href=http%3A%2F%2Fsergejus.blogas.lt%2Fitishnikai-11-it-darbo-interviu-1605.html&amp;layout=button_count&amp;show_faces=true&amp;width=150&amp;action=like&amp;colorscheme=light&amp;height=25\" scrolling=\"no\" frameborder=\"0\" style=\"border:none; overflow:hidden; width:150px; height:25px;\" allowTransparency=\"true\"></iframe> </p>";
            var contentAfterProcessing = "ITishnikai #7 jau online! <p> </p>";

            var item = new Item { Content = contentBeforeProcessing };

            new FacebookProcessor().Process(item);

            Assert.AreEqual(contentAfterProcessing, item.Content);
        }
    }
}

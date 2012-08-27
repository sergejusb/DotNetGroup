namespace DotNetGroup.Tests.Web.ViewModels
{
    using DotNetGroup.Services.Model;
    using DotNetGroup.Web.ViewModels;

    using NUnit.Framework;

    using Ploeh.AutoFixture;

    [TestFixture]
    public class StreamItemMapperTests
    {
        [Test]
        public void StreamItem_Can_Be_Mapped_From_Item()
        {
            var item = this.BuildItem();

            var mapper = new StreamItemMapper();
            var streamItem = mapper.MapFrom(item);

            Assert.AreEqual(item.AuthorImage, streamItem.AuthorImage);
            Assert.AreEqual(item.AuthorName, streamItem.AuthorName);
            Assert.AreEqual(item.AuthorUri, streamItem.AuthorUri);
            Assert.AreEqual(item.Content, streamItem.Content);
            Assert.AreEqual(item.Id, streamItem.Id);
            Assert.AreEqual(item.ItemType.ToString().ToLower(), streamItem.ItemType);
            Assert.AreEqual(item.Published.ToString("MMM dd HH:mm:ss"), streamItem.Published);
            Assert.AreEqual(item.Tags, streamItem.Tags);
            Assert.AreEqual(item.Title, streamItem.Title);
            Assert.AreEqual(item.Url, streamItem.Url);
        }

        private Item BuildItem()
        {
            return new Fixture().CreateAnonymous<Item>();
        }
    }
}

namespace DotNetGroup.Tests.Services.Processors
{
    using DotNetGroup.Services.Model;
    using DotNetGroup.Services.Processors;

    using NUnit.Framework;

    [TestFixture]
    public class TagsProcessorTests
    {
        [Test]
        public void Given_Tags_With_Mixed_Casing_After_Processing_All_Tags_Are_Lower_Case()
        {
            var tagsBeforeProcessing = new[] { "ASP.NET MVC" };
            var tagsAfterProcessing = new[] { "asp.net mvc" };

            var item = new Item { Tags = tagsBeforeProcessing };

            new TagsProcessor().Process(item);

            CollectionAssert.AreEquivalent(tagsAfterProcessing, item.Tags);
        }

        [Test]
        public void Given_ltnet_Tag_It_Is_Remvoed()
        {
            var tagsBeforeProcessing = new[] { "ASP.NET MVC", "ltnet" };
            var tagsAfterProcessing = new[] { "asp.net mvc" };

            var item = new Item { Tags = tagsBeforeProcessing };

            new TagsProcessor().Process(item);

            CollectionAssert.AreEquivalent(tagsAfterProcessing, item.Tags);
        }
    }
}

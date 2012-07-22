namespace DotNetGroup.Tests.Services.Processors
{
    using System.Linq;

    using DotNetGroup.Services.Model;
    using DotNetGroup.Services.Processors;
    using DotNetGroup.Tests.Services.Processors.LuceneHelpers;

    using Lucene.Net.Search;

    using NUnit.Framework;

    using SimpleLucene.Impl;

    [TestFixture]
    public class LuceneProcessorTests
    {
        [SetUp]
        public void Init()
        {
        }

        [Test]
        public void Given_Item_Should_Add_To_Index()
        {
            var writer = new MemoryIndexWriter(true);
            var processor = new LuceneProcessor(writer);

            var item = new Item
            {
                Url = "http://dotnetgroup.lt",
                Tags = new[] { "c#", "dotnet" },
                Content = "<b>sample content</b>",
                Title = "title",
                AuthorName = "author"
            };

            processor.Process(item);

            var searcher = new MemoryIndexSearcher(writer.Directory, readOnly: false);
            var searchService = new SearchService(searcher);

            var results = searchService.SearchIndex(new MatchAllDocsQuery()).Results;

            Assert.AreEqual(1, results.Count());
        }
    }
}
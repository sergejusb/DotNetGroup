namespace Tests.Services.Processors
{
    using System.Linq;

    using Lucene.Net.Search;

    using NUnit.Framework;

    using global::Services.Model;

    using global::Services.Processors;

    using SimpleLucene.Impl;

    using Tests.Services.Processors.LuceneHelpers;

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
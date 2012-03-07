using Lucene.Net.Search;
using Lucene.Net.Store;
using SimpleLucene;

namespace Tests.Services.Processors.LuceneHelpers
{
    public class MemoryIndexSearcher : IIndexSearcher
    {
        private readonly bool readOnly;
        private readonly RAMDirectory directory;

        public MemoryIndexSearcher(RAMDirectory directory, bool readOnly)
        {
            this.readOnly = readOnly;
            this.directory = directory;
        }

        public Searcher Create()
        {
            return new IndexSearcher(directory, readOnly);
        }
    }
}
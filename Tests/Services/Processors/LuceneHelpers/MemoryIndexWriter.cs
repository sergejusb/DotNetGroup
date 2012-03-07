using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using SimpleLucene;

namespace Tests.Services.Processors.LuceneHelpers
{
    public class MemoryIndexWriter : IIndexWriter
    {
        public bool CreateIndex { get; private set; }

        public RAMDirectory Directory { get; set; }

        public MemoryIndexWriter(bool createIndex)
        {
            this.Directory = new RAMDirectory();
            this.CreateIndex = createIndex;
            this.IndexOptions = new IndexOptions();
        }

        public IndexOptions IndexOptions { get; set; }

        public IndexWriter Create()
        {
            var ramDirectory = new RAMDirectory();
            this.Directory = ramDirectory;
            return new IndexWriter(ramDirectory,
                                        new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29),
                                        CreateIndex,
                                        IndexWriter.MaxFieldLength.UNLIMITED);
        }
    }
}
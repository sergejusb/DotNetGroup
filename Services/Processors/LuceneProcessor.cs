using System.IO;
using Services.Model;
using SimpleLucene;
using SimpleLucene.Impl;

namespace Services.Processors
{
    public class LuceneProcessor : IItemProcessor
    {
        private readonly IIndexWriter indexWriter;
        private readonly object lockingObject = new object();

        public LuceneProcessor(IIndexWriter indexWriter)
        {
            this.indexWriter = indexWriter;
        }

        public LuceneProcessor(string indexPath) 
        {       
            indexWriter = new DirectoryIndexWriter(new DirectoryInfo(indexPath));            
        }        

        public void Process(Item item)
        {
            lock (lockingObject)
            {
                using (var indexService = new IndexService(indexWriter))
                {
                    indexService.IndexEntity(item, new ItemIndexDefinition());
                }
            }
        }     
    }
}
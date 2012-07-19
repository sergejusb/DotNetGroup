namespace DotNetGroup.Services.Processors
{
    using System.IO;

    using DotNetGroup.Services.Model;

    using SimpleLucene;
    using SimpleLucene.Impl;

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
            this.indexWriter = new DirectoryIndexWriter(new DirectoryInfo(indexPath));            
        }        

        public void Process(Item item)
        {
            lock (this.lockingObject)
            {
                using (var indexService = new IndexService(this.indexWriter))
                {
                    indexService.IndexEntity(item, new ItemIndexDefinition());
                }
            }
        }     
    }
}
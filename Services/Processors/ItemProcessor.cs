using System.Configuration;

namespace Services.Processors
{
    using System;

    using Services.Model;

    public interface IItemProcessor
    {
        void Process(Item item);
    }

    public class ItemProcessor : IItemProcessor
    {
        private readonly IItemProcessor[] itemProcessors;

        public ItemProcessor()
            : this(new UrlContentProcessor(), new TagsProcessor(), new FacebookProcessor(), new LuceneProcessor(ConfigurationManager.AppSettings["lucene.indexPath"]))
        {
        }

        public ItemProcessor(params IItemProcessor[] itemProcessors)
        {
            if (itemProcessors == null)
            {
                throw new ArgumentNullException("itemProcessors");
            }

            this.itemProcessors = itemProcessors;
        }

        public void Process(Item item)
        {
            foreach (var itemProcessor in this.itemProcessors)
            {
                itemProcessor.Process(item);
            }
        }
    }
}
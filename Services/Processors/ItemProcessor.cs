using System;
using Services.Model;

namespace Services.Processors
{
    public interface IItemProcessor
    {
        void Process(Item item);
    }

    public class ItemProcessor : IItemProcessor
    {
        private readonly IItemProcessor[] _itemProcessors;

        public ItemProcessor()
            : this(new UrlContentProcessor(), new TagsProcessor(), new FacebookProcessor())
        {
        }

        public ItemProcessor(params IItemProcessor[] itemProcessors)
        {
            if (itemProcessors == null)
                throw new ArgumentNullException("itemProcessors");

            _itemProcessors = itemProcessors;
        }

        public void Process(Item item)
        {
            foreach (var itemProcessor in _itemProcessors)
            {
                itemProcessor.Process(item);
            }
        }
    }
}
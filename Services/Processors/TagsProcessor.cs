using System.Linq;
using Services.Model;

namespace Services.Processors
{
    public class TagsProcessor : IItemProcessor
    {
        public void Process(Item item)
        {
            if (item.Tags != null)
            {
                item.Tags = item.Tags.Select(t => t.ToLowerInvariant()).ToList();
            }
        }
    }
}

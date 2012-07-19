namespace DotNetGroup.Services.Processors
{
    using System.Linq;

    using DotNetGroup.Services.Model;

    public class TagsProcessor : IItemProcessor
    {
        public void Process(Item item)
        {
            if (item.Tags != null)
            {
                item.Tags = item.Tags
                                .Select(t => t.ToLowerInvariant())
                                .Where(t => !t.Equals("ltnet"))
                                .ToArray();
            }
        }
    }
}

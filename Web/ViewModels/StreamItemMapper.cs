namespace DotNetGroup.Web.ViewModels
{
    using DotNetGroup.Services.Model;
    using DotNetGroup.Services.Web;

    public interface IStreamItemMapper
    {
        StreamItem MapFrom(Item item);
    }

    public class StreamItemMapper : IStreamItemMapper
    {
        private readonly IContentFormatter contentFormatter;

        public StreamItemMapper()
            : this(new ContentFormatter())
        {
        }

        public StreamItemMapper(IContentFormatter contentFormatter)
        {
            this.contentFormatter = contentFormatter;
        }

        public StreamItem MapFrom(Item item)
        {
            return new StreamItem
            {
                Id = item.Id,
                Url = item.Url,
                Published = item.Published.ToString("MMM dd HH:mm:ss"),
                AuthorName = item.AuthorName,
                AuthorUri = item.AuthorUri,
                AuthorImage = item.AuthorImage ?? "http://placehold.it/48x48",
                Title = item.Title,
                Content = this.contentFormatter.Format(item.Content),
                Tags = item.Tags,
                ItemType = item.ItemType.ToString().ToLower()
            };
        }
    }
}
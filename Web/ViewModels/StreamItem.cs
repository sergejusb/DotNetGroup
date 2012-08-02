namespace DotNetGroup.Web.ViewModels
{
    using DotNetGroup.Services.Model;

    public class StreamItem
    {
        public StreamItem(Item item)
        {
            this.Id = item.Id;
            this.Url = item.Url;
            this.Published = item.Published.ToString("MMM dd HH:mm:ss");
            this.AuthorName = item.AuthorName;
            this.AuthorUri = item.AuthorUri;
            this.AuthorImage = item.AuthorImage ?? "http://placehold.it/48x48";
            this.Title = item.Title;
            this.Content = item.Content;
            this.Tags = item.Tags;
            this.ItemType = item.ItemType.ToString().ToLower();
        }

        public string Id { get; set; }

        public string Url { get; set; }

        public string Published { get; set; }

        public string AuthorName { get; set; }

        public string AuthorUri { get; set; }

        public string AuthorImage { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string[] Tags { get; set; }

        public string ItemType { get; set; }
    }
}
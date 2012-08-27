namespace DotNetGroup.Web.ViewModels
{
    public class StreamItem
    {
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
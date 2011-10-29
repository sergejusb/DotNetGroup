using System;

namespace Services.Rss
{
    public class Feed
    {
        public string Url { get; set; }

        public DateTime Published { get; set; }

        public string AuthorName { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; }

        public string[] Categories { get; set; }
    }
}
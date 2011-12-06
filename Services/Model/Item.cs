using System;

namespace Services.Model
{
    public class Item
    {
        public string Url { get; set; }

        public DateTime Published { get; set; }

        public string AuthorName { get; set; }

        public string AuthorUri { get; set; }

        public string AuthorImage { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public ItemType ItemType { get; set; }
    }
}

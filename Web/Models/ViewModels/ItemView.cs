using System.Collections.Generic;
using Services.Model;

namespace Web.Models.ViewModels
{
    public class ItemView
    {
        public string Title { get; set; }

        public string AuthorName { get; set; }

        public string AuthorUri { get; set; }

        public string AuthorImage { get; set; }

        public string Content { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public ItemType ItemType { get; set; }

        public string Id { get; set; }

        public string Url { get; set; }
    }
}
using System.Collections.Generic;
using global::Services.Model;

namespace Web.Mobile.Models.ViewModels
{
    public class ItemCompactView
    {
        public string Title { get; set; }

        public string AuthorName { get; set; }

        public string AuthorUri { get; set; }

        public string AuthorImage { get; set; }

        public string SampleContent { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public ItemType ItemType { get; set; }

        public string Id { get; set; }

        public string Url { get; set; }
    }
}
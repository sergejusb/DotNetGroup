using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Services.Model
{
    public class Item
    {
        private ObjectId _id;

        public Item()
        {
            _id = ObjectId.GenerateNewId();
            Tags = new List<string>();
        }

        public string Id
        {
            get
            {
                return _id.ToString();
            }
            set
            {
                if (!ObjectId.TryParse(value, out _id))
                    throw new ArgumentException("ID is of not valid format", "value");
            }
        }

        public string Url { get; set; }

        public DateTime Published { get; set; }

        public string AuthorName { get; set; }

        public string AuthorUri { get; set; }

        public string AuthorImage { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public IList<string> Tags { get; set; }

        public ItemType ItemType { get; set; }
    }
}

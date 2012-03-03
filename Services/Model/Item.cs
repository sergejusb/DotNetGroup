namespace Services.Model
{
    using System;

    using MongoDB.Bson;

    public class Item
    {
        private ObjectId id;

        public Item()
        {
            this.id = ObjectId.GenerateNewId();
            this.Tags = new string[0];
        }

        public string Id
        {
            get
            {
                return this.id.ToString();
            }
            set
            {
                if (!ObjectId.TryParse(value, out this.id))
                {
                    throw new ArgumentException("ID is of not valid format", "value");
                }
            }
        }

        public string Url { get; set; }

        public DateTime Published { get; set; }

        public string AuthorName { get; set; }

        public string AuthorUri { get; set; }

        public string AuthorImage { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string[] Tags { get; set; }

        public ItemType ItemType { get; set; }
    }
}

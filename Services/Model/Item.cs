namespace DotNetGroup.Services.Model
{
    using System;

    using DotNetGroup.Services.Utililty;

    public class Item
    {
        private readonly IHashProvider hashProvider;

        public Item()
            : this(new HashProvider())
        {
        }

        public Item(IHashProvider hashProvider)
        {
            this.hashProvider = hashProvider;
            this.Tags = new string[0];
        }

        public string Id
        {
            get
            {
                if (string.IsNullOrEmpty(this.Url))
                {
                    throw new NullReferenceException("Item URL is not set");
                }

                return this.hashProvider.ComputeHash(this.Url);
            }

            set
            {
                // XML serializer requires to have both public getter and setter
                // MongoDB driver requires Id property to have both getter and setter
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

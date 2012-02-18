namespace Api.Models
{
    using System;

    using Services.Model;

    public class StreamFilter
    {
        private const int PastDays = 14;
        private const int MaxItems = 100;

        private DateTime? from;
        private int? limit;

        public ItemType? Type { get; set; }

        public DateTime? To { get; set; }

        public DateTime? From
        {
            get
            {
                return this.from ?? DateTime.UtcNow.AddDays(-PastDays).Date;
            }
            set
            {
                this.from = value;
            }
        }

        public int? Limit
        {
            get
            {
                return this.limit ?? MaxItems;
            }
            set
            {
                this.limit = value;
            }
        }
    }
}
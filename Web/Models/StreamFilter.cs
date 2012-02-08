namespace Web.Models
{
    using System;

    using global::Services.Model;

    public class StreamFilter
    {
        public ItemType? Type { get; set; }

        public DateTime? To { get; set; }

        public DateTime? From { get; set; }

        public int? Limit { get; set; }
    }
}
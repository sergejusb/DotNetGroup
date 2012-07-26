namespace DotNetGroup.Api.Models
{
    using System;

    using DotNetGroup.Services.Model;
    using DotNetGroup.Services.Utililty;

    public class StreamFilter
    {
        internal const int MaxAllowedLimit = 100;
        internal const int DefaultLimit = 25;
        internal const int PastDays = 14;

        private DateTime? from;
        private int limit;

        public string Id { get; set; }

        public string Max_Id { get; set; }

        public string Min_Id { get; set; }

        public ItemType? Type { get; set; }

        public DateTime? To { get; set; }

        public DateTime? From
        {
            get
            {
                return this.from ?? SystemDateTime.UtcNow().AddDays(-PastDays).Date;
            }

            set
            {
                this.from = value;
            }
        }

        public int Limit
        {
            get
            {
                return Math.Min(this.limit == 0 ? DefaultLimit : this.limit, MaxAllowedLimit);
            }

            set
            {
                this.limit = value;
            }
        }

        public string Callback { get; set; }
    }
}
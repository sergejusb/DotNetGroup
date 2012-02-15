namespace Web.Mobile.Models
{
    using System;
    using System.Web.Routing;

    using global::Services.Model;

    public class StreamFilter
    {
        private const int DefaultLimit = 10;        
        private int? limit;

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public int? Limit
        {
            get
            {
                return this.limit ?? DefaultLimit;
            }
            set
            {
                this.limit = value;
            }
        }    

        public ItemType? Type { get; set; }

        public RouteValueDictionary AsRouteValuesDictionary()  
        {
            return new RouteValueDictionary(this);
        }

        public StreamFilter WithItemType(ItemType itemType)
        {
            Type = itemType;
            return this;
        }        

        public StreamFilter WithToDate(DateTime date)
        {
            To = date;
            return this;
        }

        public StreamFilter WithFromDate(DateTime date)
        {
            From = date;
            return this;
        }
    }
}
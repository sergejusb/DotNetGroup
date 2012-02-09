namespace Web.Mobile.Models
{
    using System;
    using System.Web.Routing;

    using global::Services.Model;

    public class StreamFilter
    {
        private const int MaxItems = 100;        
        private int? limit;

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

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

        public ItemType? Type { get; set; }

        public RouteValueDictionary AsRouteValuesDictionary()  
        {
            return new RouteValueDictionary(this);
        }

        public StreamFilter WithItemType(ItemType itemType)
        {
            this.Type = itemType;
            return this;
        }
    }
}
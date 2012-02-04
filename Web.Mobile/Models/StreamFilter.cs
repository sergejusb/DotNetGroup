namespace Web.Mobile.Models
{
    using System;
    using System.Web.Routing;

    using global::Services.Model;

    public class StreamFilter
    {
        private int pageSize = 10;

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public int PageSize
        {
            get
            {
                return this.pageSize;
            }
            set
            {
                this.pageSize = value;
            }
        }

        public int? PageIndex { get; set; }

        public ItemType? ItemType { get; set; }

        public RouteValueDictionary AsRouteValuesDictionary()  
        {
            return new RouteValueDictionary(this);
        }

        public StreamFilter WithItemType(ItemType itemType)
        {
            ItemType = itemType;
            return this;
        }
    }
}
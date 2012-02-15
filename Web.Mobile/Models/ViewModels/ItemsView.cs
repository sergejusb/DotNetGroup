namespace Web.Mobile.Models.ViewModels
{
    using System.Collections.Generic;

    public class ItemsView
    {
        public StreamFilter Filter { get; set; }
        public IEnumerable<ItemCompactView> Items { get; set; }
    }
}
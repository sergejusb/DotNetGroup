namespace Web.Mobile.Services
{
    using System.Collections.Generic;

    using global::Services.Model;

    using global::Services.Web;

    using Web.Mobile.Models;

    public interface IStreamService
    {
        Item GetItem(string objectId);

        IEnumerable<Item> GetItems(StreamFilter filter);
    }

    public class StreamService : IStreamService
    {
        private readonly string baseUrl;
        private readonly IJsonClient jsonClient;

        public StreamService(string baseUrl)
            : this(baseUrl, new JsonClient())
        {
        }

        public StreamService(string baseUrl, IJsonClient jsonClient)
        {
            this.baseUrl = baseUrl;
            this.jsonClient = jsonClient;
        }

        public IEnumerable<Item> GetItems(StreamFilter filter)
        {
            var url = new UrlBuilder(this.baseUrl)
                .WithParameter("type", filter.Type)
                .WithParameter("from", filter.From)
                .WithParameter("to", filter.To)
                .WithParameter("limit", filter.Limit)
                .Build();

            return this.jsonClient.Get<IEnumerable<Item>>(url);
        }

        public Item GetItem(string objectId)
        {
            var url = new UrlBuilder(this.baseUrl)
                .WithPart("get")
                .WithPart(objectId)
                .Build();
            
            return this.jsonClient.Get<Item>(url);
        }
    }
}
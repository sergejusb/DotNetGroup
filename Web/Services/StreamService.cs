namespace Web.Services
{
    using System;
    using System.Collections.Generic;

    using global::Services.Model;

    using global::Services.Web;

    public interface IStreamService
    {
        IEnumerable<Item> GetItems(ItemType? type, DateTime? from, DateTime? to, int? limit);
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

        public IEnumerable<Item> GetItems(ItemType? type, DateTime? from, DateTime? to, int? limit)
        {
            var urlBuilder = new UrlBuilder(this.baseUrl);
            
            if (type.HasValue)
            {
                urlBuilder.AddParameter("type", type);
            }

            if (from.HasValue)
            {
                urlBuilder.AddParameter("from", from);
            }

            if (to.HasValue)
            {
                urlBuilder.AddParameter("to", from);
            }

            if (limit.HasValue)
            {
                urlBuilder.AddParameter("limit", from);
            }

            return this.jsonClient.Get<IEnumerable<Item>>(urlBuilder.Build());
        }
    }
}
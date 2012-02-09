using System.Collections.Generic;
using Services.Model;
using Services.Web;
using Web.Mobile.Models;

namespace Web.Mobile.Services
{    
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
            var urlBuilder = new UrlBuilder(this.baseUrl);

            urlBuilder
                .WithIfNotBlank("type", filter.Type)
                .WithIfNotBlank("from", filter.From)
                .WithIfNotBlank("to", filter.To)
                .WithIfNotBlank("limit", filter.Limit);         

            return this.jsonClient.Get<IEnumerable<Item>>(urlBuilder.Build());
        }

        public Item GetItem(string objectId)
        {
            var urlBuilder = new UrlBuilder(this.baseUrl);
            return this.jsonClient.Get<Item>(urlBuilder.Build());
        }       
    }
}
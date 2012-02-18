namespace Web.Services
{
    using global::Services.Web;

    using Web.Models;

    public interface IStreamService
    {
        string GetItems(StreamFilter filter);
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

        public string GetItems(StreamFilter filter)
        {
            var url = this.BuildUrl(filter);

            return this.jsonClient.Get(url);
        }

        private string BuildUrl(StreamFilter filter)
        {
            return new UrlBuilder(this.baseUrl)
                .WithParameter("type", filter.Type)
                .WithParameter("from", filter.From)
                .WithParameter("to", filter.To)
                .WithParameter("limit", filter.Limit)
                .Build();
        }
    }
}
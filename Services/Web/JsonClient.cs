namespace Services.Web
{
    using System.Net;
    using System.Text;
    using System.Web.Script.Serialization;

    public interface IJsonClient
    {
        string Get(string url);

        TResult Get<TResult>(string url);
    }

    public class JsonClient : IJsonClient
    {
        private readonly JavaScriptSerializer jsonSerializer;

        public JsonClient()
        {
            this.jsonSerializer = new JavaScriptSerializer();
        }

        public string Get(string url)
        {
            using (var webClient = new WebClient { Encoding = Encoding.UTF8 })
            {
                return webClient.DownloadString(url);
            }
        }

        public TResult Get<TResult>(string url)
        {
            var json = this.Get(url);

            return this.jsonSerializer.Deserialize<TResult>(json);
        }
    }
}
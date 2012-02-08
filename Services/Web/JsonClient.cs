namespace Services.Web
{
    using System.Net;
    using System.Text;
    using System.Web.Script.Serialization;

    public interface IJsonClient
    {
        TResult Get<TResult>(string url);
    }

    public class JsonClient : IJsonClient
    {
        private readonly JavaScriptSerializer jsonSerializer;

        public JsonClient()
        {
            this.jsonSerializer = new JavaScriptSerializer();
        }

        public TResult Get<TResult>(string url)
        {
            using (var webClient = new WebClient { Encoding = Encoding.UTF8 })
            {
                return this.jsonSerializer.Deserialize<TResult>(webClient.DownloadString(url));
            }
        }
    }
}
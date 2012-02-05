using System.Collections.Generic;
using System.Net;
using Services.Model;
using System.Text;
using System.Web.Script.Serialization;

namespace Web.Services
{
    public interface IStreamService
    {
        IEnumerable<Item> GetItems();
    }

    public class StreamService : IStreamService
    {
        private readonly JavaScriptSerializer _jsonSerializer;

        public StreamService()
        {
            _jsonSerializer = new JavaScriptSerializer();
        }

        public IEnumerable<Item> GetItems()
        {
            using (var webClient = new WebClient { Encoding = Encoding.UTF8 })
            {
                var url = "http://api.dotnetgroup.dev/?from=2011-01-01";
                return _jsonSerializer.Deserialize<IEnumerable<Item>>(webClient.DownloadString(url));
            }
        }
    }
}
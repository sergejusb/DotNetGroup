using System.Collections.Generic;
using System.Net;
using System.Web;
using Services.Model;
using Web.Mobile.Models;

namespace Web.Mobile.Services
{
    using System.Text;
    using System.Web.Script.Serialization;

    public interface IStreamService
    {
        Item GetItem(string objectId);
        IEnumerable<Item> GetItems(StreamFilter filter);
    }

    public class StreamService : IStreamService
    {
        private readonly JavaScriptSerializer _jsonSerializer;

        public StreamService()
        {
            _jsonSerializer = new JavaScriptSerializer();
        }

        public IEnumerable<Item> GetItems(StreamFilter filter)
        {
            using (var webClient = new WebClient { Encoding = Encoding.UTF8 })
            {
                var url = "http://api.dotnetgroup.dev/";
                if (filter.ItemType.HasValue)
                    url += filter.ItemType + "/";
                url += "?limit=" + filter.PageSize;
                if (filter.DateFrom.HasValue)
                    url += "&from=" + HttpUtility.UrlEncodeUnicode(filter.DateFrom.Value.ToString());


                return _jsonSerializer.Deserialize<IEnumerable<Item>>(webClient.DownloadString(url));
            }
        }

        public Item GetItem(string id)
        {
            using (var webClient = new WebClient { Encoding = Encoding.UTF8 })
            {
                var url = "http://api.dotnetgroup.dev/get/" + id;
                return _jsonSerializer.Deserialize<Item>(webClient.DownloadString(url));
            }
        }
    }
}
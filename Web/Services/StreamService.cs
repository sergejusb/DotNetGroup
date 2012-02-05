using System;
using System.Collections.Generic;
using System.Net;
using Services.Model;
using System.Text;
using System.Web.Script.Serialization;

namespace Web.Services
{
    public interface IStreamService
    {
        IEnumerable<Item> GetItems(ItemType? type, DateTime? from, int? limit);
    }

    public class StreamService : IStreamService
    {
        private readonly JavaScriptSerializer _jsonSerializer;

        public StreamService()
        {
            _jsonSerializer = new JavaScriptSerializer();
        }

        public IEnumerable<Item> GetItems(ItemType? type, DateTime? from, int? limit)
        {
            using (var webClient = new WebClient { Encoding = Encoding.UTF8 })
            {
                var url = "http://api.dotnetgroup.dev/";
                if (type.HasValue) url += type + "/";
                url += "?from=" + (from.HasValue ? from : DateTime.UtcNow.AddYears(-1).Date);
                if (limit.HasValue) url += "&limit=" + limit.Value;
                
                return _jsonSerializer.Deserialize<IEnumerable<Item>>(webClient.DownloadString(url));
            }
        }
    }
}
using System.Collections.Generic;
using System.Net;
using Services.Model;
using Web.Mobile.Models;

namespace Web.Mobile.Services
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Web.Script.Serialization;

    using MongoDB.Bson;

    public interface IStreamService
    {
        IEnumerable<Item> GetItems(StreamFilter filter);
        Item GetItem(ObjectId objectId);
    }

    public class StreamService : IStreamService
    {
        public IEnumerable<Item> GetItems(StreamFilter filter)
        {
            // TODO replace with actual implementation, that gives combined sream by using filter
            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                var jsonSerializer = new JavaScriptSerializer();
                var rssItems = jsonSerializer.Deserialize<IEnumerable<Item>>(webClient.DownloadString("http://api.dotnetgroup.dev/rss/json"));
                var twitterItems = jsonSerializer.Deserialize<IEnumerable<Item>>(webClient.DownloadString("http://api.dotnetgroup.dev/twitter/json?from=2010-01-01"));
                var items = rssItems.Union(twitterItems).OrderBy(x => Guid.NewGuid()).AsQueryable();

                if (filter == null)
                {
                    return items;
                }

                if (filter.DateFrom.HasValue)
                {
                    items = items.Where(x => x.Published >= filter.DateFrom.Value.Date);
                }

                if (filter.DateTo.HasValue)
                {
                    items = items.Where(x => x.Published <= filter.DateFrom.Value.Date.AddDays(1));
                }

                if (filter.ItemType.HasValue)
                {
                    items = items.Where(x => x.ItemType == filter.ItemType);
                }

                if (filter.PageIndex.HasValue)
                {
                    items = items.Skip(filter.PageIndex.Value * filter.PageSize).Take(filter.PageSize);
                }

                return items;
            }
        }

        public Item GetItem(ObjectId objectId)
        {
            // todo: replace with actual implementation
            return this.GetItems(null).FirstOrDefault(x => x.Id == objectId);
        }
    }
}
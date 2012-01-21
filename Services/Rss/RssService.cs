using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using Services.Model;

namespace Services.Rss
{
    public interface IRssService
    {
        IEnumerable<Item> GetFeeds(string url, DateTime fromDate); 
    }

    public class RssService : IRssService
    {
        public IEnumerable<Item> GetFeeds(string url, DateTime fromDate)
        {
            try
            {
                using (var reader = XmlReader.Create(url))
                {
                    var result = SyndicationFeed.Load(reader).Items.TakeWhile(i => i.PublishDate.DateTime > fromDate);
                    
                    return result.Select(i => new Item
                    {
                        Url = i.Id,
                        Published = i.LastUpdatedTime.DateTime,
                        AuthorName = i.Authors[0].Name,
                        AuthorUri = i.Authors[0].Uri,
                        Title = i.Title.Text,
                        Content = ((TextSyndicationContent)i.Content).Text,
                        Tags = i.Categories.Select(c => c.Name).ToList(),
                        ItemType = ItemType.Rss
                    }).ToList();
                }
            }
            catch
            {
                return new List<Item>();
            }
        }
    }
}

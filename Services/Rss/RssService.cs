using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using Services.Model;

namespace Services.Rss
{
    public interface IRssService
    {
        IEnumerable<Item> GetFeeds(string url);
    }

    public class RssService : IRssService
    {
        public IEnumerable<Item> GetFeeds(string url)
        {
            try
            {
                using (var reader = XmlReader.Create(url))
                {
                    var result = SyndicationFeed.Load(reader).Items;

                    return result.Select(i => new Item
                    {
                        Url = i.Id,
                        Published = i.LastUpdatedTime.DateTime,
                        AuthorName = i.Authors[0].Name,
                        Title = i.Title.Text,
                        Content = ((TextSyndicationContent)i.Content).Text,
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

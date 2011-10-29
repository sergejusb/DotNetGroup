using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Services.Rss
{
    public interface IRssService
    {
        IEnumerable<Feed> GetFeeds(string url);
    }

    public class RssService : IRssService
    {
        public IEnumerable<Feed> GetFeeds(string url)
        {
            try
            {
                using (var reader = XmlReader.Create(url))
                {
                    var result = SyndicationFeed.Load(reader).Items;

                    return result.Select(i => new Feed
                    {
                        Url = i.Id,
                        Published = i.LastUpdatedTime.DateTime,
                        AuthorName = i.Authors[0].Name,
                        Title = i.Title.Text,
                        Summary = i.Summary.Text,
                        Content = ((TextSyndicationContent)i.Content).Text,
                        Categories = i.Categories.Select(c => c.Name).ToArray()
                    }).ToList();
                }
            }
            catch
            {
                return new List<Feed>();
            }
        }
    }
}

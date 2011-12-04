using System.Collections.Generic;
using System.Linq;
using Services.Generic;
using Services.Model;

namespace Services.Rss
{
    public class RssAggregator : BaseItemAggregator
    {
        private readonly IRssService _rssService;
        private readonly IConfigProvider _urlProvider;

        public RssAggregator()
            : this(new RssService(), new UrlConfigProvider())
        {
        }

        public RssAggregator(IRssService rssService, IConfigProvider urlProvider)
        {
            _rssService = rssService;
            _urlProvider = urlProvider;
        }

        public override IEnumerable<Item> GetLatest(int count)
        {
            return _urlProvider.GetValues()
                    .SelectMany(url => _rssService.GetFeeds(url))
                    .OrderByDescending(f => f.Published)
                    .Take(count)
                    .ToList();
        }
    }
}
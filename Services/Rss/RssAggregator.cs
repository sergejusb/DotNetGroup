using System.Collections.Generic;
using System.Linq;

namespace Services.Rss
{
    public interface IRssAggregator
    {
        IEnumerable<Feed> GetLatestFeeds(int count = 25);
    }

    public class RssAggregator : IRssAggregator
    {
        private readonly IRssService _rssService;
        private readonly IEnumerable<string> _rssUrls;

        public RssAggregator(IEnumerable<string> rssUrls)
            : this(new RssService(), rssUrls)
        {
        }

        public RssAggregator(IRssService rssService, IEnumerable<string> rssUrls)
        {
            _rssService = rssService;
            _rssUrls = rssUrls;
        }

        public IEnumerable<Feed> GetLatestFeeds(int count = 25)
        {
            return _rssUrls.SelectMany(url => _rssService.GetFeeds(url))
                .OrderByDescending(f => f.Published)
                .Take(count)
                .ToList();
        }
    }
}
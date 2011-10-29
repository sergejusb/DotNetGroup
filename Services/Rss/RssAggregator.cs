using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Rss
{
    public interface IRssAggregator
    {
        IEnumerable<Feed> GetLatestFeeds(int count = 25);
        IEnumerable<Feed> GetLatestFeeds(string url, int count = 25);
    }

    public class RssAggregator : IRssAggregator
    {
        private readonly IRssService _rssService;
        private readonly IRssUrlProvider _urlProvider;

        public RssAggregator()
            : this(new RssService(), new ConfigRssUrlProvider())
        {
        }

        public RssAggregator(IRssService rssService, IRssUrlProvider urlProvider)
        {
            _rssService = rssService;
            _urlProvider = urlProvider;
        }

        public virtual IEnumerable<Feed> GetLatestFeeds(int count = 25)
        {
            return _urlProvider.GetUrls()
                    .SelectMany(url => _rssService.GetFeeds(url))
                    .OrderByDescending(f => f.Published)
                    .Take(count)
                    .ToList();
        }

        public IEnumerable<Feed> GetLatestFeeds(string url, int count = 25)
        {
            return GetLatestFeeds(count)
                    .TakeWhile(f => !f.Url.Equals(url, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
        }
    }
}
namespace DotNetGroup.Services.Rss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DotNetGroup.Services.Generic;
    using DotNetGroup.Services.Model;

    public class RssAggregator : IItemAggregator
    {
        private readonly IRssService rssService;
        private readonly IConfigProvider urlProvider;

        public RssAggregator()
            : this(new RssService(), new UrlConfigProvider())
        {
        }

        public RssAggregator(IRssService rssService, IConfigProvider urlProvider)
        {
            if (rssService == null)
            {
                throw new ArgumentNullException("rssService");
            }

            if (urlProvider == null)
            {
                throw new ArgumentNullException("urlProvider");
            }

            this.rssService = rssService;
            this.urlProvider = urlProvider;
        }

        public IEnumerable<Item> GetLatest(DateTime fromDate)
        {
            return this.urlProvider.GetValues()
                                   .SelectMany(url => this.rssService.GetFeeds(url, fromDate))
                                   .OrderBy(f => f.Published)
                                   .ToList();
        }
    }
}
namespace Services.Twitter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Services.Generic;
    using Services.Model;

    public class TwitterAggregator : IItemAggregator
    {
        private readonly ITwitterService twitterService;
        private readonly IConfigProvider queryProvider;

        public TwitterAggregator()
            : this(new TwitterService(), new QueryConfigProvider())
        {
        }

        public TwitterAggregator(ITwitterService twitterService, IConfigProvider queryProvider)
        {
            if (twitterService == null)
            {
                throw new ArgumentNullException("twitterService");
            }

            if (queryProvider == null)
            {
                throw new ArgumentNullException("queryProvider");
            }

            this.twitterService = twitterService;
            this.queryProvider = queryProvider;
        }

        public IEnumerable<Item> GetLatest(DateTime fromDate)
        {
            return this.queryProvider.GetValues()
                                     .SelectMany(q => this.twitterService.GetTweets(q, fromDate))
                                     .OrderBy(t => t.Published)
                                     .ToList();
        }
    }
}
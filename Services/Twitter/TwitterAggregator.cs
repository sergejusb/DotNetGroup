using System;
using System.Collections.Generic;
using System.Linq;
using Services.Generic;
using Services.Model;

namespace Services.Twitter
{
    public class TwitterAggregator : IItemAggregator
    {
        private readonly ITwitterService _twitterService;
        private readonly IConfigProvider _queryProvider;

        public TwitterAggregator()
            : this(new TwitterService(), new QueryConfigProvider())
        {
        }

        public TwitterAggregator(ITwitterService twitterService, IConfigProvider queryProvider)
        {
            _twitterService = twitterService;
            _queryProvider = queryProvider;
        }

        public IEnumerable<Item> GetLatest(DateTime fromDate)
        {
            return _queryProvider.GetValues()
                    .SelectMany(q => _twitterService.GetTweets(q, fromDate))
                    .OrderBy(t => t.Published)
                    .ToList();
        }
    }
}
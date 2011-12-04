using System.Collections.Generic;
using System.Linq;
using Services.Generic;
using Services.Model;

namespace Services.Twitter
{
    public class TwitterAggregator : BaseItemAggregator
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

        public override IEnumerable<Item> GetLatest(int count)
        {
            return _queryProvider.GetValues()
                    .SelectMany(q => _twitterService.GetTweets(q, count))
                    .OrderByDescending(t => t.Published)
                    .Take(count)
                    .ToList();
        }
    }
}
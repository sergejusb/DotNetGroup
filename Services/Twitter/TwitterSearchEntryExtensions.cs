namespace DotNetGroup.Services.Twitter
{
    using System;

    using LinqToTwitter;

    public static class TwitterSearchEntryExtensions
    {
        static TwitterSearchEntryExtensions()
        {
            TweetHelperFactory = entry => new TwitterSearchEntryHelper(entry);
        }

        internal static Func<SearchEntry, ITwitterSearchEntryHelper> TweetHelperFactory { get; set; }

        public static ITwitterSearchEntryHelper Helper(this SearchEntry entry)
        {
            return TweetHelperFactory(entry);
        }
    }
}
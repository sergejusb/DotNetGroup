namespace DotNetGroup.Web
{
    using System;
    using System.Runtime.Caching;

    using DotNetGroup.Services;
    using DotNetGroup.Services.Utililty;

    public class StreamUpdater
    {
        private readonly string cacheKey = typeof(StreamUpdater).Name;

        private readonly IStreamPersister streamPersister;
        private readonly TimeSpan period;

        public StreamUpdater(IStreamPersister streamPersister, TimeSpan period)
        {
            this.streamPersister = streamPersister;
            this.period = period;
        }

        public void Poll()
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = SystemDateTime.UtcNow().ToLocalTime().Add(this.period),
                RemovedCallback = this.CacheItemRemoved
            };

            MemoryCache.Default.Set(this.cacheKey, new object(), cacheItemPolicy);
        }

        private void CacheItemRemoved(CacheEntryRemovedArguments args)
        {
            this.streamPersister.PersistLatest();

            this.Poll();
        }
    }
}
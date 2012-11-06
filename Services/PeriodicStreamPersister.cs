namespace DotNetGroup.Services
{
    using System;
    using System.Runtime.Caching;

    using DotNetGroup.Services.Utililty;

    public interface IPeriodicStreamPersister
    {
        void Start();

        void Stop();
    }

    public class PeriodicStreamPersister : IPeriodicStreamPersister
    {
        private readonly string cacheKey = typeof(PeriodicStreamPersister).Name;

        private readonly IStreamPersister streamPersister;
        private readonly TimeSpan period;

        public PeriodicStreamPersister(string connectionString, string database, TimeSpan period)
            : this(new StreamPersister(connectionString, database), period)
        {
        }

        public PeriodicStreamPersister(IStreamPersister streamPersister, TimeSpan period)
        {
            if (streamPersister == null)
            {
                throw new ArgumentNullException("streamPersister");
            }

            this.streamPersister = streamPersister;
            this.period = period;
        }

        public void Start()
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = SystemDateTime.UtcNow().ToLocalTime().Add(this.period),
                RemovedCallback = this.CacheItemRemoved
            };

            MemoryCache.Default.Set(this.cacheKey, new object(), cacheItemPolicy);
        }

        public void Stop()
        {
            MemoryCache.Default.Remove(this.cacheKey);
        }

        private void CacheItemRemoved(CacheEntryRemovedArguments args)
        {
            this.streamPersister.PersistLatest();

            this.Start();
        }
    }
}
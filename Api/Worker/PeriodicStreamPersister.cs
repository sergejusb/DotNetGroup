using DotNetGroup.Api.Worker;

[assembly: WebActivator.PreApplicationStartMethod(typeof(PeriodicStreamPersister), "Start")]

namespace DotNetGroup.Api.Worker
{
    using System;
    using System.Configuration;
    using System.Threading;

    using DotNetGroup.Services;

    public static class PeriodicStreamPersister
    {
        private static readonly IStreamPersister StreamPersister;
        private static readonly JobHost JobHost = new JobHost();
        private static readonly Timer Timer = new Timer(OnTimerElapsed);

        static PeriodicStreamPersister()
        {
            StreamPersister = new StreamPersister(ConfigurationManager.AppSettings["db.connection"], ConfigurationManager.AppSettings["db.database"]);
        }

        public static void Start()
        {
            Timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(60));
        }

        private static void OnTimerElapsed(object state)
        {
            JobHost.DoWork(StreamPersister.PersistLatest);
        }

    }
}
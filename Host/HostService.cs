namespace DotNetGroup.Host
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.ServiceProcess;
    using System.Threading;
    using System.Threading.Tasks;

    using DotNetGroup.Services;

    using log4net;

    public class HostService : ServiceBase
    {
        public const string Name = "StreamHost";
        public const string DisplayName = "DotNetGroup Stream Host";

        private readonly IStreamPersister streamPersister;
        private readonly TimeSpan refreshPeriod;
        private readonly ILog logger;

        private CancellationTokenSource tokenSource;
        private Task task;

        public HostService(ILog logger)
        {
            var connectionString = ConfigurationManager.AppSettings["db.connection"];
            var database = ConfigurationManager.AppSettings["db.database"];
            this.streamPersister = new StreamPersister(connectionString, database);
            this.refreshPeriod = TimeSpan.Parse(ConfigurationManager.AppSettings["sys.refreshPeriod"] ?? "00:01:00");
            this.logger = logger;
        }

        public HostService(IStreamPersister streamPersister, TimeSpan refreshPeriod, ILog logger)
        {
            this.streamPersister = streamPersister;
            this.refreshPeriod = refreshPeriod;
            this.logger = logger;
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public void Start(string[] args)
        {
            if (Environment.UserInteractive)
            {
                SetupEventLog();

                if (HasReprocessArgument(args))
                {
                    this.task = Task.Factory.StartNew(() => this.streamPersister.Reprocess());
                }
                else
                {
                    this.OnStart(args);   
                }

                task.Wait();
            }
            else
            {
                Run(this);
            }
        }

        internal static void SetupEventLog()
        {
            const string EventLogName = "Stream Host";
            const string EventSource = "DotNetGroup Stream Host";

            if (!EventLog.SourceExists(EventSource))
            {
                EventLog.CreateEventSource(EventSource, EventLogName);
                var log = new EventLog(EventLogName, ".", EventSource);
                log.WriteEntry("Log created", EventLogEntryType.Information);
            }
        }

        protected override void OnStart(string[] args)
        {
            this.logger.InfoFormat("Starting service {0}", DisplayName);
            this.tokenSource = new CancellationTokenSource();

            this.task = Task.Factory.StartNew(() =>
            {
                while (!this.tokenSource.IsCancellationRequested)
                {
                    this.logger.InfoFormat("Refreshing stream...");
                    this.streamPersister.PersistLatest();
                    Thread.Sleep(this.refreshPeriod);
                }
            });

            base.OnStart(args);
        }

        protected override void OnStop()
        {
            this.logger.InfoFormat("Stopping service {0}", DisplayName);
            base.OnStop();

            try
            {
                this.tokenSource.Cancel();
            }
            catch (Exception ex)
            {
                this.logger.Warn(string.Format("Error while stopping stopping service {0}", DisplayName), ex);
            }
        }

        private static bool HasReprocessArgument(IEnumerable<string> args)
        {
            return args != null && args.Any(arg => arg.Equals("-r"));
        }
    }
}
namespace Host
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Threading;

    using Services;

    public class Program
    {
        public static void Main(string[] args)
        {
            var reprocessArgs = new[] { "-r", "-reprocess" };
            var refreshPeriod = TimeSpan.FromSeconds(int.Parse(ConfigurationManager.AppSettings["sys.refreshPeriodInSec"] ?? "60"));
            var connectionString = ConfigurationManager.AppSettings["db.connection"];
            var database = ConfigurationManager.AppSettings["db.database"];
            var streamPersister = new StreamPersister(connectionString, database);

            if (args != null && args.Length == 1 && reprocessArgs.Contains(args[0].ToLower()))
            {
                streamPersister.Reprocess();
            }
            else
            {
                while (true)
                {
                    Console.WriteLine("Running...");

                    streamPersister.PersistLatest();

                    Console.WriteLine("Sleeping for " + refreshPeriod.TotalSeconds + " sec");
                    Thread.Sleep(refreshPeriod);
                }
            }
        }
    }
}

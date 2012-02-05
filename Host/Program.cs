using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using Services;

namespace Host
{
    public class Program
    {
        static void Main(string[] args)
        {
            var reprocessArgs = new[] { "-r", "-reprocess" };
            var refreshPeriod = TimeSpan.FromSeconds(Int32.Parse(ConfigurationManager.AppSettings["sys.refreshPeriodInSec"] ?? "60"));
            var connectionString = ConfigurationManager.AppSettings["db.connection"];
            var database = ConfigurationManager.AppSettings["db.database"];
            var streamPersister = new StreamPersister(connectionString, database);

            if (args != null && reprocessArgs.Contains(args[0].ToLower()))
            {
                streamPersister.Reprocess();
            }
            else
            {
                while (true)
                {
                    Console.WriteLine("Running...");

                    streamPersister.PersistLatest();

                    Console.WriteLine("Sleeping for " + refreshPeriod.Seconds + " sec");
                    Thread.Sleep(refreshPeriod);
                }
            }
        }
    }
}

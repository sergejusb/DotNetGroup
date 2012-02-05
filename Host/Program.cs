using System;
using System.Configuration;
using System.Threading;
using Services;

namespace Host
{
    public class Program
    {
        static void Main()
        {
            var refreshPeriod = TimeSpan.FromSeconds(Int32.Parse(ConfigurationManager.AppSettings["sys.refreshPeriodInSec"] ?? "60"));
            var connectionString = ConfigurationManager.AppSettings["db.connection"];
            var database = ConfigurationManager.AppSettings["db.database"];
            var streamPersister = new StreamPersister(connectionString, database);

            while(true)
            {
                Console.WriteLine("Running...");
                
                streamPersister.PersistLatest();

                Console.WriteLine("Sleeping for " + refreshPeriod.Seconds + " sec");
                Thread.Sleep(refreshPeriod);
            }
        }
    }
}

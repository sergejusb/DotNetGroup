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
            Console.WriteLine("Running...");

            var refreshPeriod = TimeSpan.FromSeconds(Int32.Parse(ConfigurationManager.AppSettings["sys.refreshPeriodInSec"] ?? "60"));
            var connectionString = ConfigurationManager.AppSettings["db.connection"];
            var database = ConfigurationManager.AppSettings["db.database"];
            var streamPersister = new StreamPersister(connectionString, database);

            while(true)
            {
                streamPersister.PersistLatest();
                Thread.Sleep(refreshPeriod);
            }
        }
    }
}

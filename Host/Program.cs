namespace DotNetGroup.Host
{
    using System;
    using System.Configuration.Install;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using log4net;
    using log4net.Config;

    public class Program
    {
        public static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            var logger = LogManager.GetLogger(typeof(HostService).Name);

            try
            {
                if (args != null && args.Length > 0)
                {
                    for (var i = 0; i < args.Length; i++)
                    {
                        args[i] = args[i].Trim().ToLowerInvariant();
                    }

                    var exeName = Path.GetFileName(Assembly.GetEntryAssembly().Location);
                    if (!string.IsNullOrEmpty(exeName))
                    {
                        if (args.Contains("uninstall"))
                        {
                            using (var installer = new AssemblyInstaller(exeName, null) { UseNewContext = true })
                            {
                                installer.Uninstall(null);
                                return;
                            }
                        }
                        
                        if (args.Contains("install"))
                        {
                            using (var installer = new AssemblyInstaller(exeName, null) { UseNewContext = true })
                            {
                                installer.Install(null);
                                installer.Commit(null);
                                return;
                            }
                        }   
                    }
                }

                new HostService(logger).Start(args);
            }
            catch (Exception e)
            {
                logger.Error("An exception has occurred during startup!", e);
            }
        }
    }
}

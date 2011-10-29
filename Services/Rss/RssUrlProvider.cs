using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Services.Rss
{
    public interface IRssUrlProvider
    {
        IEnumerable<string> GetUrls();
    }

    public class ConfigRssUrlProvider : IRssUrlProvider
    {
        public IEnumerable<string> GetUrls()
        {
            return ConfigurationManager.AppSettings.AllKeys
                    .Where(k => k.StartsWith("rss."))
                    .Select(k => ConfigurationManager.AppSettings[k])
                    .ToList();
        }
    }
}

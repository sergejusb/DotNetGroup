using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Services.Generic
{
    public interface IConfigProvider
    {
        IEnumerable<string> GetValues();
    }

    public abstract class BaseConfigProvider : IConfigProvider
    {
        protected abstract string Prefix { get; }

        public IEnumerable<string> GetValues()
        {
            return ConfigurationManager.AppSettings.AllKeys
                    .Where(k => k.StartsWith(Prefix))
                    .Select(k => ConfigurationManager.AppSettings[k])
                    .ToList();
        }
    }
}

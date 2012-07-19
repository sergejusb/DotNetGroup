namespace DotNetGroup.Services.Generic
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

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
                    .Where(key => key.StartsWith(this.Prefix))
                    .Select(key => ConfigurationManager.AppSettings[key])
                    .ToList();
        }
    }
}

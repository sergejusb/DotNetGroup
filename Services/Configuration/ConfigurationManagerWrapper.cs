namespace DotNetGroup.Services.Configuration
{
    using System.Collections.Specialized;
    using System.Configuration;

    public interface IConfigurationManager
    {
        NameValueCollection AppSettings
        {
            get;
        }

        string ConnectionStrings(string name);

        T GetSection<T>(string sectionName);
    }

    public class ConfigurationManagerWrapper : IConfigurationManager
    {
        public NameValueCollection AppSettings
        {
            get
            {
                return ConfigurationManager.AppSettings;
            }
        }

        public string ConnectionStrings(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public T GetSection<T>(string sectionName)
        {
            return (T)ConfigurationManager.GetSection(sectionName);
        }
    }
}

namespace DotNetGroup.Services.Rss
{
    using DotNetGroup.Services.Generic;

    public class UrlConfigProvider : BaseConfigProvider
    {
        protected override string Prefix
        {
            get { return "rss."; }
        }
    }
}

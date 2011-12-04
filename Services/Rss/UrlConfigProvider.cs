using Services.Generic;

namespace Services.Rss
{
    public class UrlConfigProvider : BaseConfigProvider
    {
        protected override string Prefix
        {
            get { return "rss."; }
        }
    }
}

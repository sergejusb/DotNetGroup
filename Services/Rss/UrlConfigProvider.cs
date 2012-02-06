namespace Services.Rss
{
    using Services.Generic;

    public class UrlConfigProvider : BaseConfigProvider
    {
        protected override string Prefix
        {
            get { return "rss."; }
        }
    }
}

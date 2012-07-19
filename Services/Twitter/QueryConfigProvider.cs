namespace DotNetGroup.Services.Twitter
{
    using DotNetGroup.Services.Generic;

    public class QueryConfigProvider : BaseConfigProvider
    {
        protected override string Prefix
        {
            get { return "twitter."; }
        }
    }
}

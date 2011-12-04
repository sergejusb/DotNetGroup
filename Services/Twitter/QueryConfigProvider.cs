using Services.Generic;

namespace Services.Twitter
{
    public class QueryConfigProvider : BaseConfigProvider
    {
        protected override string Prefix
        {
            get { return "twitter."; }
        }
    }
}

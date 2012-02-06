namespace Services.Twitter
{
    using Services.Generic;

    public class QueryConfigProvider : BaseConfigProvider
    {
        protected override string Prefix
        {
            get { return "twitter."; }
        }
    }
}

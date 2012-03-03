namespace Host.Api
{
    using System;
    using System.Configuration;
    using System.Web.Http;
    using System.Web.Http.Routing;
    using System.Web.Http.SelfHost;

    using Host.Api.Formatters;

    public class ApiServer : IDisposable
    {
        private readonly HttpSelfHostServer server;

        public ApiServer()
            : this(ConfigurationManager.AppSettings["host.address"])
        {
        }

        public ApiServer(string baseAddress)
        {
            if (string.IsNullOrEmpty(baseAddress))
            {
                throw new ArgumentNullException("baseAddress");
            }

            this.BaseUri = new Uri(baseAddress);

            var config = new HttpSelfHostConfiguration(this.BaseUri);
            config.Formatters.Insert(0, new JsonpMediaTypeFormatter());

            var defaultRouteDefaults = new HttpRouteValueDictionary
                {
                    { "controller", "stream" },
                    { "id", RouteParameter.Optional }
                };
            config.Routes.Add("Default", new HttpRoute("{controller}/{id}", defaultRouteDefaults));

            this.server = new HttpSelfHostServer(config);
            this.server.OpenAsync().Wait();
        }

        public Uri BaseUri { get; private set; }

        public void Dispose()
        {
            if (this.server != null)
            {
                this.server.Dispose();
            }
        }
    }
}
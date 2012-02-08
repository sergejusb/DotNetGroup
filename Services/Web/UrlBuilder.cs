namespace Services.Web
{
    using System;
    using System.Collections.Generic;

    public interface IUrlBuilder
    {
        void AddPart(object value);

        void AddParameter(string name, object value);

        string Build();
    }

    public class UrlBuilder : IUrlBuilder
    {
        private readonly Uri baseUrl;
        private readonly IList<string> relativeUrlParts;
        private readonly IList<string> queryStringParameters;

        public UrlBuilder(string baseUrl)
        {
            if (baseUrl == null)
            {
                throw new ArgumentNullException("baseUrl");
            }

            if (!Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
            {
                throw new ArgumentException("Base URL format is not valid", "baseUrl");
            }

            this.baseUrl = new Uri(baseUrl);
            this.relativeUrlParts = new List<string>();
            this.queryStringParameters = new List<string>();
        }

        public void AddPart(object value)
        {
            this.relativeUrlParts.Add(value.ToString());
        }

        public void AddParameter(string name, object value)
        {
            this.queryStringParameters.Add(name + "=" + value);
        }

        public string Build()
        {
            var relativeUrl = new Uri(string.Join("/", this.relativeUrlParts), UriKind.Relative);
            var queryString = string.Join("&", this.queryStringParameters);

            var uriBuilder = new UriBuilder(new Uri(this.baseUrl, relativeUrl))
            {
                Query = queryString
            };

            return uriBuilder.Uri.AbsoluteUri;
        }
    }
}
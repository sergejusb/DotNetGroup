using System;
using HtmlAgilityPack;

namespace Services.Web
{
    public class UrlContentProcessor
    {
        private readonly IUrlResolver _urlResolver;

        public UrlContentProcessor()
            : this(new UrlResolver())
        {
        }

        public UrlContentProcessor(IUrlResolver urlResolver)
        {
            _urlResolver = urlResolver;
        }

        public string Process(string content)
        {
            var html = new HtmlDocument();
            html.LoadHtml(content);

            foreach (var href in html.DocumentNode.SelectNodes("//a"))
            {
                if (Uri.IsWellFormedUriString(href.InnerText, UriKind.Absolute))
                {
                    href.InnerHtml = _urlResolver.Resolve(href.InnerText);
                }
            }

            return html.DocumentNode.InnerHtml;
        }
    }
}

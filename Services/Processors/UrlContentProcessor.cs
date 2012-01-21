using System;
using HtmlAgilityPack;
using Services.Model;
using Services.Web;

namespace Services.Processors
{
    public class UrlContentProcessor : IItemProcessor
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

        public void Process(Item item)
        {
            if (!String.IsNullOrEmpty(item.Content))
            {
                var html = new HtmlDocument();
                html.LoadHtml(item.Content);

                var hrefs = html.DocumentNode.SelectNodes("//a");
                if (hrefs != null)
                {
                    foreach (var href in hrefs)
                    {
                        if (Uri.IsWellFormedUriString(href.InnerText, UriKind.Absolute))
                        {
                            href.InnerHtml = _urlResolver.Resolve(href.InnerText);
                        }
                    }

                    item.Content = html.DocumentNode.InnerHtml;
                }
            }
        }
    }
}

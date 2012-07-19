namespace DotNetGroup.Services.Processors
{
    using System;

    using DotNetGroup.Services.Model;
    using DotNetGroup.Services.Web;

    using HtmlAgilityPack;

    public class UrlContentProcessor : IItemProcessor
    {
        private readonly IUrlResolver urlResolver;

        public UrlContentProcessor()
            : this(new UrlResolver())
        {
        }

        public UrlContentProcessor(IUrlResolver urlResolver)
        {
            if (urlResolver == null)
            {
                throw new ArgumentNullException("urlResolver");
            }

            this.urlResolver = urlResolver;
        }

        public void Process(Item item)
        {
            if (!string.IsNullOrEmpty(item.Content))
            {
                item.Content = this.ExpandUrls(item.Content);
            }
        }

        private string ExpandUrls(string content)
        {
            var html = new HtmlDocument();
            html.LoadHtml(content);

            var hrefs = html.DocumentNode.SelectNodes("//a");
            if (hrefs != null)
            {
                foreach (var href in hrefs)
                {
                    if (Uri.IsWellFormedUriString(href.InnerText, UriKind.Absolute))
                    {
                        href.InnerHtml = this.urlResolver.Resolve(href.InnerText);
                    }
                }

                return html.DocumentNode.InnerHtml;
            }

            return content;
        }
    }
}

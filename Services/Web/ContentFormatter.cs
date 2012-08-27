namespace DotNetGroup.Services.Web
{
    using System;
    using System.Linq;

    using HtmlAgilityPack;

    public interface IContentFormatter
    {
        string Format(string content);
    }

    public class ContentFormatter : IContentFormatter
    {
        public ContentFormatter(int maxUrlTextLength = 50)
        {
            this.MaxUrlTextLength = maxUrlTextLength;
        }

        public int MaxUrlTextLength { get; private set; }

        public string Format(string content)
        {
            var html = new HtmlDocument();
            html.LoadHtml(content);

            var hrefs = html.DocumentNode.SelectNodes("//a");
            if (hrefs != null)
            {
                foreach (var href in hrefs)
                {
                    Uri uri;
                    if (Uri.TryCreate(href.InnerText, UriKind.Absolute, out uri))
                    {
                        href.SetAttributeValue("title", href.InnerText);
                        href.InnerHtml = href.InnerText.Length > this.MaxUrlTextLength
                                             ? string.Format("{0}/../{1}", uri.Host, uri.Segments.Last())
                                             : uri.Host + uri.PathAndQuery;
                    }
                }
            }

            return html.DocumentNode.InnerHtml;
        }
    }
}

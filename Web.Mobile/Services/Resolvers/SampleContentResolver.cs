namespace Web.Mobile.Services.Resolvers
{
    using System;    
    using System.Linq;
    using System.Text.RegularExpressions;

    using AutoMapper;

    using HtmlAgilityPack;

    using global::Services.Model;

    public class SampleContentResolver : ValueResolver<Item, string>
    {        
        public const int CutoffLength = 200;
        public const int LongestWordLength = 20;
        private static readonly Regex RemoveHtmlTagsRegex = new Regex(@"<[^>]*>");

        protected override string ResolveCore(Item source)
        {
            string content = source.Content;
            if (string.IsNullOrWhiteSpace(content))
            {
                return content;
            }

            var cleanText = SanitizeHtml(content);
            var markupLength = cleanText.Length - RemoveHtmlTagsRegex.Replace(cleanText, "").Length;
            var totalCutoffLength = CutoffLength + markupLength;

            if (cleanText.Length > totalCutoffLength)
            {
                var nextSpace = content.LastIndexOf(" ", totalCutoffLength, StringComparison.Ordinal);
                cleanText = cleanText.Substring(0, nextSpace > totalCutoffLength - LongestWordLength ? nextSpace : totalCutoffLength);                

                cleanText = CloseTags(cleanText);              

                return string.Format("{0} ...", cleanText);
            }

            return cleanText;
        }

        private string CloseTags(string html)
        {
            var doc = new HtmlDocument
                {
                    OptionAutoCloseOnEnd = true
                };

            doc.LoadHtml(html);            

            var brokenNode = doc.DocumentNode.DescendantNodes().FirstOrDefault(x => (x.Name != "a" && x.Name != "#text") || (x.Name == "a" && x.Attributes.Count == 0));

            if (brokenNode != null)
            {
                doc.DocumentNode.RemoveChild(brokenNode);
            }

            return doc.DocumentNode.WriteTo();
        }

        private string SanitizeHtml(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            string[] elementWhitelist = { "a", "u", "b", "i", "h1", "h2", "h3", "h4", "h5", "h6", "span", "div", "blockquote", "em", "sub", "sup", "s", "font", "ul", "li", "ol", "p", "#text" };
            string[] attributeWhiteList = { "href" };

            var hnc = doc.DocumentNode.DescendantNodes().ToList();            

            for (var i = hnc.Count - 1; i >= 0; i--)
            {
                var htmlNode = hnc[i];
                if (!elementWhitelist.Contains(htmlNode.Name.ToLower()))
                {
                    htmlNode.Remove();
                    continue;
                }

                if (htmlNode.Name.ToLower() != "a")
                {
                    htmlNode.Name = "#remove";
                } 

                for (int att = htmlNode.Attributes.Count - 1; att >= 0; att--)
                {
                    var attribute = htmlNode.Attributes[att];
                    
                    if (!attributeWhiteList.Contains(attribute.Name.ToLower()))
                    {
                        attribute.Remove();
                    }                    

                    if (attribute.Name.ToLower() == "src" || attribute.Name.ToLower() == "href")
                    {                        
                        if (!attribute.Value.StartsWith("http"))
                        {
                            attribute.Value = "#";
                        }
                    }
                }
            }

            return doc.DocumentNode.WriteTo().Replace("<#remove>", "").Replace("</#remove>", "");
        }
    }
}
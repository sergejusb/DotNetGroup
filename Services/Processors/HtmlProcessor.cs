namespace DotNetGroup.Services.Processors
{
    using System.Collections.Generic;
    using System.Linq;

    using DotNetGroup.Services.Model;

    using HtmlAgilityPack;

    public class HtmlProcessor : IItemProcessor
    {
        private const string MoreContentSymbol = "[...]";

        private readonly string[] allowedTags = new[] { "p", "h1", "h2", "h3", "h4", "h5", "h6" };
        private readonly int limit;

        public HtmlProcessor(int limit = 250)
        {
            this.limit = limit;
        }

        public void Process(Item item)
        {
            if (!string.IsNullOrEmpty(item.Content))
            {
                item.Content = this.TrimContent(item.Content);
            }
        }

        private string TrimContent(string content)
        {
            var summary = new List<HtmlNode>();

            var html = new HtmlDocument();
            html.LoadHtml(content);

            var length = 0;
            var node = html.DocumentNode.FirstChild;

            if (!this.allowedTags.Contains(node.Name))
            {
                return content;
            }

            if (node.InnerText.EndsWith(MoreContentSymbol))
            {
                // content is already processed
                return content;
            }

            var loop = true;
            while (loop && node != null)
            {
                if (this.allowedTags.Contains(node.Name))
                {
                    length += node.InnerText.Length;
                    if (length >= this.limit)
                    {
                        node.ChildNodes.Append(HtmlNode.CreateNode(MoreContentSymbol));
                        loop = false;
                    }

                    summary.Add(node);
                    node = node.NextSibling;
                }
                else if (node.NodeType == HtmlNodeType.Text)
                {
                    // skip it
                    node = node.NextSibling;
                }
                else
                {
                    summary.Last().ChildNodes.Append(HtmlNode.CreateNode(MoreContentSymbol));
                    loop = false;
                }
            }

            return string.Concat(summary.Select(n => n.OuterHtml));
        }
    }
}

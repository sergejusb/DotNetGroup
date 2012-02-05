using System;
using HtmlAgilityPack;
using Services.Model;

namespace Services.Processors
{
    public class FacebookProcessor : IItemProcessor
    {
        public void Process(Item item)
        {
            if (!String.IsNullOrEmpty(item.Content))
            {
                item.Content = RemoveFrames(item.Content);
            }
        }

        private string RemoveFrames(string content)
        {
            var html = new HtmlDocument();
            html.LoadHtml(content);

            var frames = html.DocumentNode.SelectNodes("//iframe[contains(@src,'facebook.com/plugins')]");
            if (frames != null)
            {
                foreach (var frame in frames)
                {
                    frame.ParentNode.RemoveChild(frame, false);
                }

                return html.DocumentNode.InnerHtml;
            }

            return content;
        }
    }
}

namespace Services.Processors
{
    using HtmlAgilityPack;

    using Services.Model;

    public class FacebookProcessor : IItemProcessor
    {
        public void Process(Item item)
        {
            if (!string.IsNullOrEmpty(item.Content))
            {
                item.Content = this.RemoveFrames(item.Content);
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

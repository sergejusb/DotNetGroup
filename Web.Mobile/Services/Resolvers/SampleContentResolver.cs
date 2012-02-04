namespace Web.Mobile.Services.Resolvers
{
    using System;
    using System.Text.RegularExpressions;

    using AutoMapper;

    using global::Services.Model;

    public class SampleContentResolver : ValueResolver<Item, string>
    {
        private static readonly Regex RemoveHtmlTagsRegex = new Regex(@"<[^>]*>");
        public const int CutoffLength = 200;
        public const int LongestWordLength = 20;

        protected override string ResolveCore(Item source)
        {
            string content = source.Content;
            if (string.IsNullOrWhiteSpace(content))
            {
                return content;
            }

            var cleanText = RemoveHtmlTagsRegex.Replace(content, "");

            if (cleanText.Length > CutoffLength)
            {
                var nextSpace = content.LastIndexOf(" ", CutoffLength, StringComparison.Ordinal);
                return string.Format("{0} ...", cleanText.Substring(0, nextSpace > CutoffLength - LongestWordLength ? nextSpace : CutoffLength));
            }

            return cleanText;
        }
    }
}
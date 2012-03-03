/*
 * Credits: Thinktecture
 * The source code has been taken from https://github.com/ChristianWeyer/Thinktecture.Web.Http/blob/master/Thinktecture.Web.Http/Formatters/JsonpFormatter.cs
 */

namespace Host.Api.Formatters
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;

    public class JsonpMediaTypeFormatter : JsonMediaTypeFormatter
    {
        private string callbackQueryParameter;

        public JsonpMediaTypeFormatter()
        {
            this.SupportedMediaTypes.Add(DefaultMediaType);
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/javascript"));

            this.MediaTypeMappings.Add(new UriPathExtensionMapping("jsonp", DefaultMediaType));
        }

        public string CallbackQueryParameter
        {
            get { return this.callbackQueryParameter ?? "callback"; }
            set { this.callbackQueryParameter = value; }
        }

        protected override Task OnWriteToStreamAsync(
            Type type,
            object value,
            Stream stream,
            HttpContentHeaders contentHeaders,
            FormatterContext formatterContext,
            TransportContext transportContext)
        {
            string callback;

            if (this.IsJsonpRequest(formatterContext.Response.RequestMessage, out callback))
            {
                return Task.Factory.StartNew(() =>
                {
                    var writer = new StreamWriter(stream);
                    writer.Write(callback + "(");
                    writer.Flush();
                    base.OnWriteToStreamAsync(type, value, stream, contentHeaders, formatterContext, transportContext).Wait();
                    writer.Write(")");
                    writer.Flush();
                });
            }
            else
            {
                return base.OnWriteToStreamAsync(type, value, stream, contentHeaders, formatterContext, transportContext);
            }
        }

        private bool IsJsonpRequest(HttpRequestMessage request, out string callback)
        {
            callback = null;

            if (request.Method != HttpMethod.Get)
            {
                return false;
            }

            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
            callback = query[this.CallbackQueryParameter];

            return !string.IsNullOrEmpty(callback);
        }
    }
}
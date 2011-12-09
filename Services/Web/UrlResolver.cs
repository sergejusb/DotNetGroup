using System;
using System.Net;

namespace Services.Web
{
    public interface IUrlResolver
    {
        string Resolve(string url);
    }

    public class UrlResolver : IUrlResolver
    {
        public string Resolve(string url)
        {
            var resolvedUrl = url;

            try
            {
                var request = WebRequest.Create(url);
                request.Method = WebRequestMethods.Http.Head;

                using (var response = request.GetResponse())
                {
                    resolvedUrl = response.ResponseUri.AbsoluteUri;
                }
            }
            catch (Exception)
            {
            }

            return resolvedUrl;
        }
    }
}

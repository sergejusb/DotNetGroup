namespace Services.Web
{
    using System.Net;

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
            catch
            {
                // ignore
            }

            return resolvedUrl;
        }
    }
}

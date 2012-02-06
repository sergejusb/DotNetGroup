/*
 * Credits: Stephen Walther, http://stephenwalther.com
 * The source code has been taken from his MVC Tip blog series http://stephenwalther.com/blog/archive/2008/07/02/asp-net-mvc-tip-13-unit-test-your-custom-routes.aspx
 */

namespace Tests.Fakes
{
    using System.Collections.Specialized;
    using System.Web;

    public class FakeHttpRequest : HttpRequestBase
    {
        private readonly string relativeUrl;
        private readonly NameValueCollection formParams;
        private readonly NameValueCollection queryStringParams;
        private readonly HttpCookieCollection cookies;

        public FakeHttpRequest(string relativeUrl, NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies)
        {
            this.relativeUrl = relativeUrl;
            this.formParams = formParams;
            this.queryStringParams = queryStringParams;
            this.cookies = cookies;
        }

        public override NameValueCollection Form
        {
            get
            {
                return this.formParams;
            }
        }

        public override NameValueCollection QueryString
        {
            get
            {
                return this.queryStringParams;
            }
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return this.cookies;
            }
        }

        public override string AppRelativeCurrentExecutionFilePath
        {
            get { return this.relativeUrl; }
        }

        public override string PathInfo
        {
            get { return string.Empty; }
        }
    }
}

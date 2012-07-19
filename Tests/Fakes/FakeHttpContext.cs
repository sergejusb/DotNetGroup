/*
 * Credits: Stephen Walther, http://stephenwalther.com
 * The source code has been taken from his MVC Tip blog series http://stephenwalther.com/blog/archive/2008/07/02/asp-net-mvc-tip-13-unit-test-your-custom-routes.aspx
 */

namespace DotNetGroup.Tests.Fakes
{
    using System.Collections.Specialized;
    using System.Security.Principal;
    using System.Web;
    using System.Web.SessionState;

    public class FakeHttpContext : HttpContextBase
    {
        private readonly string relativeUrl;
        private readonly FakePrincipal principal;
        private readonly NameValueCollection formParams;
        private readonly NameValueCollection queryStringParams;
        private readonly HttpCookieCollection cookies;
        private readonly SessionStateItemCollection sessionItems;

        public FakeHttpContext(string relativeUrl)
            : this(relativeUrl, null, null, null, null, null)
        {
        }

        public FakeHttpContext(string relativeUrl, FakePrincipal principal, NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies, SessionStateItemCollection sessionItems)
        {
            this.relativeUrl = relativeUrl;
            this.principal = principal;
            this.formParams = formParams;
            this.queryStringParams = queryStringParams;
            this.cookies = cookies;
            this.sessionItems = sessionItems;
        }

        public override HttpRequestBase Request
        {
            get
            {
                return new FakeHttpRequest(this.relativeUrl, this.formParams, this.queryStringParams, this.cookies);
            }
        }

        public override IPrincipal User
        {
            get
            {
                return this.principal;
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public override HttpSessionStateBase Session
        {
            get
            {
                return new FakeHttpSessionState(this.sessionItems);
            }
        }
    }
}

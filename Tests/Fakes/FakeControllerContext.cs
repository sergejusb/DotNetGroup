/*
 * Credits: Stephen Walther, http://stephenwalther.com
 * The source code has been taken from his MVC Tip blog series http://stephenwalther.com/blog/archive/2008/07/02/asp-net-mvc-tip-13-unit-test-your-custom-routes.aspx
 */

using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace Tests.Fakes
{
    public class FakeControllerContext : ControllerContext
    {
        public FakeControllerContext(IController controller)
            : this(controller, String.Empty, null, null, null, null, null, null)
        {
        }

        public FakeControllerContext(IController controller, HttpCookieCollection cookies)
            : this(controller, String.Empty, null, null, null, null, cookies, null)
        {
        }

        public FakeControllerContext(IController controller, SessionStateItemCollection sessionItems)
            : this(controller, String.Empty, null, null, null, null, null, sessionItems)
        {
        }

        public FakeControllerContext(IController controller, NameValueCollection formParams)
            : this(controller, String.Empty, null, null, formParams, null, null, null)
        {
        }

        public FakeControllerContext(IController controller, NameValueCollection formParams, NameValueCollection queryStringParams)
            : this(controller, String.Empty, null, null, formParams, queryStringParams, null, null)
        {
        }

        public FakeControllerContext(IController controller, string userName)
            : this(controller, String.Empty, userName, null, null, null, null, null)
        {
        }

        public FakeControllerContext(IController controller, string userName, string[] roles)
            : this(controller, String.Empty, userName, roles, null, null, null, null)
        {
        }

        public FakeControllerContext(
                IController controller,
                string relativeUrl,
                string userName,
                string[] roles,
                NameValueCollection formParams,
                NameValueCollection queryStringParams,
                HttpCookieCollection cookies,
                SessionStateItemCollection sessionItems
            )
            : base(new FakeHttpContext(relativeUrl, new FakePrincipal(new FakeIdentity(userName), roles), formParams, queryStringParams, cookies, sessionItems), new RouteData(), (ControllerBase)controller)
        { }
    }
}
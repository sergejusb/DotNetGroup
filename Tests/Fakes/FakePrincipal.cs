/*
 * Credits: Stephen Walther, http://stephenwalther.com
 * The source code has been taken from his MVC Tip blog series http://stephenwalther.com/blog/archive/2008/07/02/asp-net-mvc-tip-13-unit-test-your-custom-routes.aspx
 */

namespace DotNetGroup.Tests.Fakes
{
    using System.Linq;
    using System.Security.Principal;

    public class FakePrincipal : IPrincipal
    {
        private readonly IIdentity identity;
        private readonly string[] roles;

        public FakePrincipal(IIdentity identity, string[] roles)
        {
            this.identity = identity;
            this.roles = roles;
        }

        public IIdentity Identity
        {
            get { return this.identity; }
        }

        public bool IsInRole(string role)
        {
            return this.roles != null && this.roles.Contains(role);
        }
    }
}
/*
 * Credits: Stephen Walther, http://stephenwalther.com
 * The source code has been taken from his MVC Tip blog series http://stephenwalther.com/blog/archive/2008/07/02/asp-net-mvc-tip-13-unit-test-your-custom-routes.aspx
 */

namespace Tests.Fakes
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.SessionState;

    public class FakeHttpSessionState : HttpSessionStateBase
    {
        private readonly SessionStateItemCollection sessionItems;

        public FakeHttpSessionState(SessionStateItemCollection sessionItems)
        {
            this.sessionItems = sessionItems;
        }

        public override int Count
        {
            get
            {
                return this.sessionItems.Count;
            }
        }

        public override NameObjectCollectionBase.KeysCollection Keys
        {
            get
            {
                return this.sessionItems.Keys;
            }
        }

        public override object this[string name]
        {
            get
            {
                return this.sessionItems[name];
            }
            set
            {
                this.sessionItems[name] = value;
            }
        }

        public override object this[int index]
        {
            get
            {
                return this.sessionItems[index];
            }
            set
            {
                this.sessionItems[index] = value;
            }
        }

        public override void Add(string name, object value)
        {
            this.sessionItems[name] = value;
        }

        public override void Remove(string name)
        {
            this.sessionItems.Remove(name);
        }

        public override IEnumerator GetEnumerator()
        {
            return this.sessionItems.GetEnumerator();
        }
    }
}
namespace DotNetGroup.Api.Worker
{
    using System;
    using System.Web.Hosting;

    public class JobHost : IRegisteredObject
    {
        private readonly object _lock = new object();
        private bool shuttingDown;

        public JobHost()
        {
            HostingEnvironment.RegisterObject(this);
        }

        public void Stop(bool immediate)
        {
            lock (this._lock)
            {
                this.shuttingDown = true;
            }

            HostingEnvironment.UnregisterObject(this);
        }

        public void DoWork(Action work)
        {
            lock (this._lock)
            {
                if (this.shuttingDown)
                {
                    return;
                }

                work();
            }
        }
    }
}
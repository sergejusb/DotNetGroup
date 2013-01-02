namespace DotNetGroup.Api.Controllers
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using DotNetGroup.Services;

    public class SystemController : ApiController
    {
        private readonly IStreamPersister streamPersister;

        public SystemController()
            : this(new StreamPersister(ConfigurationManager.AppSettings["db.connection"], ConfigurationManager.AppSettings["db.database"]))
        {
        }

        public SystemController(IStreamPersister streamPersister)
        {
            if (streamPersister == null)
            {
                throw new ArgumentNullException("streamPersister");
            }

            this.streamPersister = streamPersister;
        }

        [HttpPost]
        public HttpResponseMessage UpdateStream()
        {
            this.streamPersister.PersistLatest();

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
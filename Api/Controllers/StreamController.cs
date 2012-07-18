namespace Api.Controllers
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Api.Models;

    using Services.Storage;

    public class StreamController : ApiController
    {
        private readonly IStreamStorage streamStorage;

        public StreamController()
            : this(new StreamStorage(ConfigurationManager.AppSettings["db.connection"], ConfigurationManager.AppSettings["db.database"]))
        {
        }

        public StreamController(IStreamStorage streamStorage)
        {
            if (streamStorage == null)
            {
                throw new ArgumentNullException("streamStorage");
            }

            this.streamStorage = streamStorage;
        }

        public HttpResponseMessage Get([FromUri] StreamFilter filter)
        {
            var response = Request.CreateResponse(HttpStatusCode.NotFound);

            if (!string.IsNullOrEmpty(filter.Id))
            {
                var item = this.streamStorage.Get(filter.Id);
                if (item != null)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, item);
                }
            }
            else
            {
                var stream = this.streamStorage.GetLatest(filter.Type, filter.From, filter.To, filter.Limit);
                response = Request.CreateResponse(HttpStatusCode.OK, stream);
            }

            return response;
        }
    }
}

namespace DotNetGroup.Api.Controllers
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using DotNetGroup.Api.Models;
    using DotNetGroup.Services.Storage;
    using DotNetGroup.Services.Utililty;

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
            var response = this.Request.CreateResponse(HttpStatusCode.NotFound);

            if (!string.IsNullOrEmpty(filter.Id))
            {
                var item = this.streamStorage.Get(filter.Id);
                if (item != null)
                {
                    response = this.Request.CreateResponse(HttpStatusCode.OK, item);
                }
            }
            else if (!string.IsNullOrEmpty(filter.Max_Id))
            {
                var item = this.streamStorage.Get(filter.Max_Id);
                if (item != null)
                {
                    var stream = this.streamStorage.GetOlder(item, filter.Limit);
                    response = this.Request.CreateResponse(HttpStatusCode.OK, stream);
                }
            }
            else if (!string.IsNullOrEmpty(filter.Min_Id))
            {
                var item = this.streamStorage.Get(filter.Min_Id);
                if (item != null)
                {
                    var stream = this.streamStorage.GetNewer(item, filter.Limit);
                    response = this.Request.CreateResponse(HttpStatusCode.OK, stream);
                }
            }
            else if (filter.Type.HasValue || filter.From != SystemDateTime.UtcNow().AddDays(-StreamFilter.PastDays).Date || filter.To.HasValue)
            {
                // this is temp branch for backwards compatibility, will be removed
                var stream = this.streamStorage.GetLatest(filter.Type, filter.From, filter.To, filter.Limit);
                response = this.Request.CreateResponse(HttpStatusCode.OK, stream);
            }
            else
            {
                var stream = this.streamStorage.GetLatest(filter.Limit);
                response = this.Request.CreateResponse(HttpStatusCode.OK, stream);
            }

            return response;
        }
    }
}

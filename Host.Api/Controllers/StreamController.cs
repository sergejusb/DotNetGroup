namespace Host.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Services.Model;
    using Services.Storage;

    public class StreamController : ApiController
    {
        private readonly IStreamStorage streamStorage;

        public StreamController()
            : this(ConfigurationManager.AppSettings["db.connection"], ConfigurationManager.AppSettings["db.database"])
        {
        }

        public StreamController(string connectionString, string database)
            : this(new StreamStorage(connectionString, database))
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

        public HttpResponseMessage Get(StreamFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Id))
            {
                var item = this.streamStorage.Get(filter.Id);
                return item == null ?
                        new HttpResponseMessage(HttpStatusCode.NotFound) :
                        new HttpResponseMessage<Item>(item);
            }

            var items = this.streamStorage.GetLatest(filter.Type, filter.From, filter.To, filter.Limit);
            return new HttpResponseMessage<IEnumerable<Item>>(items);
        }

        [AcceptVerbs("HEAD")]
        public HttpResponseMessage Head(StreamFilter filter)
        {
            var count = this.streamStorage.Count(filter.Type, filter.From, filter.To, filter.Limit);
            var response = new HttpResponseMessage();
            response.Headers.Add("X-Result-Count", count.ToString(CultureInfo.InvariantCulture));
            return response;
        }
    }
}

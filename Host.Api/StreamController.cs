namespace Host.Api
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
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
                return new HttpResponseMessage<Item>(item);
            }

            var items = this.streamStorage.GetLatest(filter.Type, filter.From, filter.To, filter.Limit);
            return new HttpResponseMessage<IEnumerable<Item>>(items);
        }

        public int Count(StreamFilter filter)
        {
            return this.streamStorage.Count(filter.Type, filter.From, filter.To, filter.Limit);
        }
    }
}

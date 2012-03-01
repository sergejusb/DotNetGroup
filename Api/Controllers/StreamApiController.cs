namespace Api.Controllers
{
    using System;
    using System.Configuration;
    using System.Web.Mvc;

    using Api.Models;

    using Services.Storage;

    public class StreamApiController : ApiController
    {
        private readonly IStreamStorage streamStorage;

        public StreamApiController()
            : this(ConfigurationManager.AppSettings["db.connection"], ConfigurationManager.AppSettings["db.database"])
        {
        }

        public StreamApiController(string connectionString, string database)
            : this(new StreamStorage(connectionString, database))
        {
        }

        public StreamApiController(IStreamStorage streamStorage)
        {
            if (streamStorage == null)
            {
                throw new ArgumentNullException("streamStorage");
            }

            this.streamStorage = streamStorage;
        }

        public ActionResult Get(GetFilter filter)
        {
            var item = this.streamStorage.Get(filter.Id);

            return this.JsonOrJsonp(item, filter.Callback);
        }

        public ActionResult Stream(StreamFilter filter)
        {
            var items = this.streamStorage.GetLatest(filter.Type, filter.From, filter.To, filter.Limit);

            return this.JsonOrJsonp(items, filter.Callback);
        }

        public ActionResult Count(StreamFilter filter)
        {
            var count = this.streamStorage.Count(filter.Type, filter.From, filter.To, filter.Limit);

            return this.JsonOrJsonp(count, filter.Callback);
        }
    }
}

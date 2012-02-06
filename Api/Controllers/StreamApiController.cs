namespace Api.Controllers
{
    using System;
    using System.Configuration;
    using System.Web.Mvc;

    using Services.Model;
    using Services.Storage;

    public class StreamApiController : Controller
    {
        private const int MaxItems = 100;
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

        public ActionResult Get(string id)
        {
            return Json(this.streamStorage.Get(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Stream(DateTime? from = null, ItemType? type = null, int limit = MaxItems)
        {
            from = from ?? DateTime.UtcNow.AddDays(-7).Date;
            var items = this.streamStorage.GetLatest(from, type, limit);

            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }
}

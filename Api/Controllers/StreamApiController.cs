using System;
using System.Configuration;
using System.Web.Mvc;
using Services;
using Services.Model;

namespace Api.Controllers
{
    public class StreamApiController : Controller
    {
        private const int MaxItems = 100;
        private readonly IStreamApi _streamApi;

        public StreamApiController()
            : this(new StreamApi(ConfigurationManager.AppSettings["db.connection"], ConfigurationManager.AppSettings["db.database"]))
        {
        }

        public StreamApiController(IStreamApi streamApi)
        {
            _streamApi = streamApi;
        }

        public ActionResult Get(string id)
        {
            return Json(_streamApi.Get(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Stream(DateTime? from = null, ItemType? type = null, int limit = MaxItems)
        {
            from = from ?? DateTime.UtcNow.AddDays(-7).Date;
            var items = type.HasValue ? _streamApi.Get(from.Value, type.Value, limit) : _streamApi.Get(from.Value, limit);
            
            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }
}

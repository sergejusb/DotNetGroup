using System;
using System.Web.Mvc;
using Services.Generic;
using Services.Rss;

namespace Api.Controllers
{
    public class RssController : Controller
    {
        private readonly IItemAggregator _aggregator;

        public RssController()
            : this(new RssAggregator())
        {
        }

        public RssController(IItemAggregator aggregator)
        {
            _aggregator = aggregator;
        }

        public ActionResult Json(DateTime? from)
        {
            var fromDate = from ?? DateTime.UtcNow.AddMonths(-2);
            return Json(_aggregator.GetLatest(fromDate), JsonRequestBehavior.AllowGet);
        }
    }
}

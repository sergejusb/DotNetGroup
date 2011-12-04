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
            : this(new CachedItemAggregator(new RssAggregator(), TimeSpan.FromMinutes(10)))
        {
        }

        public RssController(IItemAggregator aggregator)
        {
            _aggregator = aggregator;
        }

        public ActionResult Json(int count = 20)
        {
            return Json(_aggregator.GetLatest(count), JsonRequestBehavior.AllowGet);
        }
    }
}

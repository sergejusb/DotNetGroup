using System;
using System.Web.Mvc;
using Services.Generic;
using Services.Twitter;

namespace Api.Controllers
{
    public class TwitterController : Controller
    {
        private readonly IItemAggregator _aggregator;

        public TwitterController()
            : this(new TwitterAggregator())
        {
        }

        public TwitterController(IItemAggregator aggregator)
        {
            _aggregator = aggregator;
        }

        public ActionResult Json(DateTime? from)
        {
            var fromDate = from ?? DateTime.UtcNow.AddMonths(-1);
            return Json(_aggregator.GetLatest(fromDate), JsonRequestBehavior.AllowGet);
        }
    }
}

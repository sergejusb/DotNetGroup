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
            : this(new CachedItemAggregator(new TwitterAggregator(), TimeSpan.FromMinutes(1)))
        {
        }

        public TwitterController(IItemAggregator aggregator)
        {
            _aggregator = aggregator;
        }

        public ActionResult Json(int count = 50)
        {
            return Json(_aggregator.GetLatest(count), JsonRequestBehavior.AllowGet);
        }
    }
}

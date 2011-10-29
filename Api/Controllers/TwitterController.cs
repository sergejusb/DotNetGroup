using System;
using System.Web.Mvc;
using Services.Twitter;

namespace Api.Controllers
{
    public class TwitterController : Controller
    {
        private readonly ITwitterAggregator _aggregator;

        public TwitterController()
        {
            _aggregator = new CachedTwitterAggregator(TimeSpan.FromMinutes(1));
        }

        public TwitterController(ITwitterAggregator aggregator)
        {
            _aggregator = aggregator;
        }

        public ActionResult Json()
        {
            return Json(_aggregator.GetLatestTweets(), JsonRequestBehavior.AllowGet);
        }
    }
}

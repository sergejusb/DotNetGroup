using System.Web.Mvc;
using Services.Rss;

namespace Api.Controllers
{
    public class RssController : Controller
    {
        private readonly IRssAggregator _aggregator;

        public RssController()
        {
            _aggregator = new RssAggregator();
        }

        public RssController(IRssAggregator aggregator)
        {
            _aggregator = aggregator;
        }

        public ActionResult Json()
        {
            return Json(_aggregator.GetLatestFeeds(), JsonRequestBehavior.AllowGet);
        }
    }
}

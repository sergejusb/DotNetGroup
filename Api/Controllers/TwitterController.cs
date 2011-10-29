using System.Configuration;
using System.Web.Mvc;
using Services.Twitter;

namespace Api.Controllers
{
    public class TwitterController : Controller
    {
        private readonly string _hashtag;
        private readonly ITwitterService _service;

        public TwitterController()
        {
            _hashtag = ConfigurationManager.AppSettings["twitter"];
            _service = new TwitterService();
        }

        public ActionResult Index()
        {
            return Json(_service.GetTweets(_hashtag), JsonRequestBehavior.AllowGet);
        }
    }
}

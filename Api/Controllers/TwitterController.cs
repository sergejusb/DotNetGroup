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
            _service = new TwitterService();
            _hashtag = ConfigurationManager.AppSettings["twitter"];
        }

        public TwitterController(ITwitterService service, string hashtag)
        {
            _service = service;
            _hashtag = hashtag;
        }

        public ActionResult Json()
        {
            return Json(_service.GetTweets(_hashtag), JsonRequestBehavior.AllowGet);
        }
    }
}

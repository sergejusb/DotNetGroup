using System.Web.Mvc;
using Web.Services;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly StreamService _streamService;

        public HomeController()
            : this(new StreamService())
        {
        }

        public HomeController(StreamService streamService)
        {
            _streamService = streamService;
        }

        public ActionResult Index()
        {
            var items = _streamService.GetItems();
            return View(items);
        }
    }
}

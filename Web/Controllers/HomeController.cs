namespace Web.Controllers
{
    using System.Configuration;
    using System.Web.Mvc;

    using Web.Models;
    using Web.Services;

    public class HomeController : Controller
    {
        private readonly IStreamService streamService;

        public HomeController()
            : this(new StreamService(ConfigurationManager.AppSettings["api.url"]))
        {
        }

        public HomeController(IStreamService streamService)
        {
            this.streamService = streamService;
        }

        public virtual ActionResult Index(StreamFilter filter)
        {
            var items = this.streamService.GetItems(filter);
            return View("Index", items);
        }
    }
}

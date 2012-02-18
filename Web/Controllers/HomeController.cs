namespace Web.Controllers
{
    using System.Configuration;
    using System.Dynamic;
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

        public ActionResult Index(StreamFilter filter)
        {
            dynamic model = new ExpandoObject();
            model.Items = this.streamService.GetItems(filter);

            return View("Index", model);
        }
    }
}

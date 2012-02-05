using System;
using System.Web.Mvc;
using Services.Model;
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

        public virtual ActionResult Index(ItemType? type, DateTime? from, int? limit)
        {
            var items = _streamService.GetItems(type, from, limit);
            return View("Index", items);
        }
    }
}

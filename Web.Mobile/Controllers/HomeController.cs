using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Web.Mobile.Models.ViewModels;
using Web.Mobile.Services;

namespace Web.Mobile.Controllers
{
    using Web.Mobile.Models;

    public partial class HomeController : Controller
    {
        private readonly IStreamService _streamService;

        public HomeController()
            : this(new StreamService())
        {
        }

        public HomeController(IStreamService streamService)
        {
            this._streamService = streamService;
        }

        public virtual ActionResult Index(StreamFilter filter)
        {
            var enumerable = Mapper.Map<IEnumerable<ItemCompactView>>(_streamService.GetItems(filter));
            return View(enumerable);
        }

        public virtual ActionResult Item(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            var item = _streamService.GetItem(id);

            if (item == null)
            {
                return HttpNotFound();
            }

            var itemView = Mapper.Map<ItemView>(item);
            return View(itemView);
        }
    }
}

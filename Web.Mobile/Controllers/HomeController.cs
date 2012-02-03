using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Web.Mobile.Models.ViewModels;
using Web.Mobile.Services;

namespace Web.Mobile.Controllers
{
    using System;

    using MongoDB.Bson;

    using Web.Mobile.Models;

    using global::Services.Model;

    public partial class HomeController : Controller
    {
        private readonly IStreamService streamService;

        public HomeController() : this(new StreamService())
        {            
        }

        public HomeController(IStreamService streamService)
        {
            this.streamService = streamService;
        }

        public virtual ActionResult Index(StreamFilter filter)
        {
            var enumerable = Mapper.Map<IEnumerable<ItemCompactView>>(streamService.GetItems(filter));
            return View(enumerable);
        }

        public virtual ActionResult Item(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            ObjectId objectId;
            try
            {
                objectId = new ObjectId(id);
            }
            catch 
            {
                return HttpNotFound();
            }

            var item = streamService.GetItem(objectId);

            if (item == null)
            {
                return HttpNotFound();
            }

            var itemView = Mapper.Map<ItemView>(item);
            return View(itemView);
        }
    }
}

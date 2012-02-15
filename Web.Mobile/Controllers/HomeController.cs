namespace Web.Mobile.Controllers
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.Mvc;

    using AutoMapper;

    using Web.Mobile.Models;
    using Web.Mobile.Models.ViewModels;
    using Web.Mobile.Services;

    public partial class HomeController : Controller
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
            return View(filter);
        } 
       
        public virtual ActionResult Items(StreamFilter filter)
        {
            var items = Mapper.Map<IEnumerable<ItemCompactView>>(this.streamService.GetItems(filter));
            return View(new ItemsView
                {
                    Items = items, 
                    Filter = filter
                });
        }

        public virtual ActionResult Item(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            var item = this.streamService.GetItem(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            var itemView = Mapper.Map<ItemView>(item);
            return View(itemView);
        }
    }    
}

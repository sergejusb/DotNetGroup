namespace DotNetGroup.Web.Controllers
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web.Mvc;

    using DotNetGroup.Services.Model;
    using DotNetGroup.Services.Web;
    using DotNetGroup.Web.ViewModels;

    public class StreamController : Controller
    {
        private readonly string apiUrl;
        private readonly IJsonClient jsonClient;

        public StreamController()
            : this(ConfigurationManager.AppSettings["api.url"], new JsonClient())
        {
        }

        public StreamController(string apiUrl, IJsonClient jsonClient)
        {
            this.apiUrl = apiUrl;
            this.jsonClient = jsonClient;
        }

        public ActionResult Index()
        {
            var stream = this.Get(this.apiUrl);

            return this.View("Index", stream);
        }

        public ActionResult Newer(string id)
        {
            var url = string.Format("{0}/?min_id={1}", this.apiUrl, id);
            var stream = this.Get(url);

            return this.PartialView("Stream", stream);
        }

        public ActionResult Older(string id)
        {
            var url = string.Format("{0}/?max_id={1}", this.apiUrl, id);
            var stream = this.Get(url);

            return this.PartialView("Stream", stream);
        } 

        private IEnumerable<StreamItem> Get(string url)
        {
            return this.jsonClient.Get<IEnumerable<Item>>(url).Select(i => new StreamItem(i));
        }
    }
}
namespace Api.Controllers
{
    using System.Web.Mvc;

    using Api.Extensions;

    public abstract class ApiController : Controller
    {
        public JsonpResult Jsonp(object data, JsonRequestBehavior behavior)
        {
            return new JsonpResult
            {
                Data = data,
                JsonRequestBehavior = behavior
            };
        }

        public ActionResult JsonOrJsonp(object data, string callback = null)
        {
            return string.IsNullOrEmpty(callback) ?
                Json(data, JsonRequestBehavior.AllowGet) :
                this.Jsonp(data, JsonRequestBehavior.AllowGet);
        }
    }
}

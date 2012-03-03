namespace Api.Controllers
{
    using System.Web.Mvc;

    using Mvc.Jsonp;

    public abstract class ApiController : JsonpControllerBase
    {
        public ActionResult JsonOrJsonp(object data, string callback = null)
        {
            return string.IsNullOrEmpty(callback) ?
                Json(data, JsonRequestBehavior.AllowGet) :
                Jsonp(data, callback, JsonRequestBehavior.AllowGet);
        }
    }
}

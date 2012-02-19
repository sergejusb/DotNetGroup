namespace Api.Extensions
{
    using System.Web.Mvc;

    public static class ContollerExtensions
    {
        public static JsonpResult Jsonp(this Controller controller, object data)
        {
            return new JsonpResult { Data = data };
        }
    }
}
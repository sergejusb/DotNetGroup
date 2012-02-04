using System;
using System.Web.Mvc;

namespace Tests.Helpers
{
    public static class ActionResultExtensions
    {
        public static ViewResult ReturnsViewResult(this ActionResult result)
        {
            var viewResult = result as ViewResult;
            if (viewResult == null)
            {
                throw new Exception("result is not a ViewResult");
            }

            return viewResult;
        }
    }
}
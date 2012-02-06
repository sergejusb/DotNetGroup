namespace Tests.Helpers
{
    using System;
    using System.Web.Mvc;

    public static class ActionResultExtensions
    {
        public static ViewResult ReturnsViewResult(this ActionResult result)
        {
            var viewResult = result as ViewResult;
            if (viewResult == null)
            {
                throw new ArgumentException("Result is not a ViewResult", "result");
            }

            return viewResult;
        }
    }
}
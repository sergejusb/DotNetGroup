/*
 * Credits: Nerdworks Blogorama, http://blogorama.nerdworks.in
 * The source code has been taken from http://blogorama.nerdworks.in/entry-EnablingJSONPcallsonASPNETMVC.aspx
 */

namespace Api.Extensions
{
    using System;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    /// <summary>
    /// Renders result as JSON and also wraps the JSON in a call
    /// to the callback function specified in "JsonpResult.Callback".
    /// </summary>
    public class JsonpResult : JsonResult
    {
        /// <summary>
        /// Gets or sets the JavaScript callback function that is
        /// to be invoked in the resulting script output.
        /// </summary>
        /// <value>The callback function name.</value>
        public string Callback { get; set; }

        /// <summary>
        /// Enables processing of the result of an action method by a
        /// custom type that inherits from <see cref="T:System.Web.Mvc.ActionResult"/>.
        /// </summary>
        /// <param name="context">The context within which the
        /// result is executed.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;
            if (string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = "application/javascript";
            }
            else
            {
                response.ContentType = this.ContentType;
            }

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if (string.IsNullOrEmpty(this.Callback))
            {
                this.Callback = context.HttpContext.Request.QueryString["callback"];
            }

            if (this.Data != null)
            {
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(this.Data);
                response.Write(this.Callback + "(" + json + ");");
            }
        }
    }
}
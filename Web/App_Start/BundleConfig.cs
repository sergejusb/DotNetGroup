namespace DotNetGroup.Web
{
    using System.Web.Optimization;

    using DotNetGroup.Web.Bundles;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var css = new Bundle("~/Public/css", new LessTransform(), new CssMinify());
            css.Include("~/Public/css/bootstrap.css")
               .Include("~/Public/css/bootstrap-responsive.css")
               .Include("~/Public/css/fonts.css")
               .Include("~/Public/css/styles.less");
            bundles.Add(css);

            var js = new Bundle("~/Public/js", new JsMinify());
            js.Include("~/Public/js/lib/jquery-1*")
              .Include("~/Public/js/lib/bootstrap.js")
              .Include("~/Public/js/scripts.js");
            bundles.Add(js);
        }
    }
}
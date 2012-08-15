namespace DotNetGroup.Web
{
    using System.Web.Optimization;

    using DotNetGroup.Web.Bundles;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var css = new Bundle("~/public/static/css", new CssMinify());
            css.Include("~/Public/css/bootstrap.css")
                .Include("~/Public/css/bootstrap-responsive.css")
                .Include("~/Public/css/fonts.css");
            bundles.Add(css);

            var less = new Bundle("~/public/static/less", new LessTransform(), new CssMinify());
            less.Include("~/Public/css/styles.less");
            bundles.Add(less);

            var js = new Bundle("~/public/static/js", new JsMinify());
            js.Include("~/Public/js/lib/jquery-1*")
              .Include("~/Public/js/lib/bootstrap.js")
              .Include("~/Public/js/lib/spin.js")
              .Include("~/Public/js/lib/jquery.spin.js")
              .Include("~/Public/js/scripts.js");
            bundles.Add(js);
        }
    }
}
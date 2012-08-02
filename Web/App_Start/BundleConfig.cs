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
        }
    }
}
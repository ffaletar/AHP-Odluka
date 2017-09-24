using System.Web;
using System.Web.Optimization;

namespace AHPDecision
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //scripts
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/js/jquery/jquery.validate*"));
            
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/js/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/js/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/AdminLTE").Include(
                        "~/Content/js/adminlte.js"));

            bundles.Add(new ScriptBundle("~/bundles/App").Include(
                        "~/Content/js/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/Site").Include(
                        "~/Content/js/site.js"));

            bundles.Add(new ScriptBundle("~/bundles/Evaluation").Include(
                        "~/Content/js/evaluation.js"));

            bundles.Add(new ScriptBundle("~/bundles/Slider").Include(
                        "~/Content/js/bootstrap-slider.js"));

            bundles.Add(new ScriptBundle("~/bundles/SlimScroll").Include(
                        "~/Content/js/jquery/jquery.slimscroll.js"));

            //styles
            bundles.Add(new StyleBundle("~/Content/css/Bootstrap").Include(
                      "~/Content/css/bootstrap-theme.css",
                      "~/Content/css/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/css/AdminLTE").Include(
                      "~/Content/css/AdminLTE.css"));

            bundles.Add(new StyleBundle("~/Content/css/Site").Include(
                      "~/Content/css/site.css"));

            bundles.Add(new StyleBundle("~/Content/css/FontCSS").Include(
                      "~/Content/css/font-awesome.css",
                      "~/Content/css/ionicons.css"));

            bundles.Add(new StyleBundle("~/Content/css/Skin").Include(
                      "~/Content/skins/skin-green.css"));

            bundles.Add(new StyleBundle("~/Content/css/Slider").Include(
                      "~/Content/css/slider.css"));

        }
    }
}

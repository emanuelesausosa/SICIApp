using System.Web;
using System.Web.Optimization;

namespace SICIApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/lib/jquery-pjax/jquery.pjax.js",
                        "~/lib/bootstrap-sass-official/assets/javascripts/bootstrap.js",
                        "~/lib/widgster/widgster.js",
                        "~/lib/underscore/underscore.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));
            // general scripts
            // app and chat
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                        "~/Scripts/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/chat").Include(
                        "~/Scripts/chat.js"));
            //component
            bundles.Add(new ScriptBundle("~/bundles/component").Include(
                        "~/Scripts/component-*"));

            // forms
            bundles.Add(new ScriptBundle("~/bundles/forms").Include(
                        "~/Scripts/forms.js",
                        "~/Scripts/forms-*"));
            // grid
            bundles.Add(new ScriptBundle("~/bundles/grid").Include(
                        "~/Scripts/grid-live.js"));
            //inbox
            bundles.Add(new ScriptBundle("~/bundles/inbox").Include(
                        "~/Scripts/inbox.js"));
            // index
            bundles.Add(new ScriptBundle("~/bundles/index").Include(
                        "~/Scripts/index.js"));
            // landing
            bundles.Add(new ScriptBundle("~/bundles/landing").Include(
                        "~/Scripts/landing.js"));

            //print
            bundles.Add(new ScriptBundle("~/bundles/print").Include(
                        "~/Scripts/print.js"));
            // setings
            bundles.Add(new ScriptBundle("~/bundles/settings").Include(
                        "~/Scripts/settings.js"));
            //stats
            bundles.Add(new ScriptBundle("~/bundles/stat").Include(
                        "~/Scripts/stat-*"));

            //tables
            bundles.Add(new ScriptBundle("~/bundles/tables").Include(
                        "~/Scripts/tables-*"));
            // iu-s
            bundles.Add(new ScriptBundle("~/bundles/ui").Include(
                        "~/Scripts/ui-*"));

            //nvd3
            bundles.Add(new ScriptBundle("~/bundles/nvd3").Include(
                        "~/Scripts/nvd3-custom-lb1.0/nv.d3.custom.js",
                        "~/Scripts/nvd3-custom-lb1.0/stream_layers.js",
                        "~/Scripts/nvd3-custom-lb1.0/src/models/axis.js",
                        "~/Scripts/nvd3-custom-lb1.0/src/models/legend.js",
                        "~/Scripts/nvd3-custom-lb1.0/src/models/line.js",
                        "~/Scripts/nvd3-custom-lb1.0/src/models/lineChart.js",
                        "~/Scripts/nvd3-custom-lb1.0/src/models/multiBar.js",
                        "~/Scripts/nvd3-custom-lb1.0/src/models/multiBarChart.js",
                        "~/Scripts/nvd3-custom-lb1.0/src/models/pie.js",
                        "~/Scripts/nvd3-custom-lb1.0/src/models/pieChartTotal.js",
                        "~/Scripts/nvd3-custom-lb1.0/src/models/scatter.js",
                        "~/Scripts/nvd3-custom-lb1.0/src/models/stackedArea.js",
                        "~/Scripts/nvd3-custom-lb1.0/src/models/stackedAreaChart.js"));

            //page specific scripts
            bundles.Add(new ScriptBundle("~/bundles/specific").Include(
                       "~/lib/slimScroll/jquery.slimscroll.min.js",
                       "~/lib/jquery.sparkline/index.js",
                       "~/lib/backbone/backbone.js",
                       "~/lib/d3/d3.min.js",
                       "~/lib/nvd3/nv.d3.min.js"));

            // css
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/application.css"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/application-theme.css",
            //          "~/Content/site.css"));
        }
    }
}

using System.Web.Optimization;

namespace PremiumDiesel.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootbox.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/DataTables/jquery.datatables.js",
                        "~/Scripts/DataTables/datatables.bootstrap.js"
                      ));

            // Original settings
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"
                      ));

            // Use the development version of Modernizr to develop with and learn from. Then, when
            // you're ready for production, use the build tool at https://modernizr.com to pick only
            // the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"
                      ));

            // Original settings
            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/bootbox.js",
            //          "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        $"~/Content/Themes/{System.Configuration.ConfigurationManager.AppSettings["BootstrapTheme"]}/bootstrap.css",
                        $"~/Content/Themes/{System.Configuration.ConfigurationManager.AppSettings["BootstrapTheme"]}/bootswatch.less",
                        $"~/Content/Themes/{System.Configuration.ConfigurationManager.AppSettings["BootstrapTheme"]}/variables.less",
                        "~/Content/DataTables/css/datatables.bootstrap.css",
                        "~/Content/site.css"
                      ));

            // Original settings
            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //           "~/Content/bootstrap.css",
            //           "~/Content/site.css"
            //          ));
        }
    }
}
using System.Web.Optimization;

namespace Cybertech_v1._1
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/bootstrap.css",
                       "~/adminlte/css/adminlte.min.css",
                       "~/adminlte/plugins/fontawesome-free/css/all.min.css",
                       "~/adminlte/plugins/select2/select2.min.css",
                       "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/jquery.unobtrusive-ajax.js")
                .Include("~/Scripts/jquery.validate*"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval"));//);            

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .Include("~/Scripts/modernizr-*"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Scripts/bootstrap.bundle.min.js"));

            bundles.Add(new ScriptBundle("~/adminlte/js")
                .Include("~/adminlte/js/adminlte.min.js")
                .Include("~/adminlte/plugins/select2/js/select2.full.min.js"));


            BundleTable.EnableOptimizations = false;

        }
    }
}

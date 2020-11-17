using System.Web;
using System.Web.Optimization;

namespace Realtek.IntraLogin
{
    public class BundleConfig
    {
        // 如需「搭配」的詳細資訊，請瀏覽 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好實際執行時，請使用 http://modernizr.com 上的建置工具，只選擇您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/toastmessage").Include(
                     "~/Scripts/jquery.toastmessage.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/font-awesome.min.css",
                "~/Content/jquery.toastmessage.css",
                "~/Content/metisMenu.min.css",
                "~/Content/kendo/styles/kendo.common.min.css",
                "~/Content/kendo/styles/kendo.material.min.css",
                "~/Content/kendo.custom.css",
                "~/Content/fullcalendar.min.css",
                "~/Content/bootstrap-tagsinput.css",
                "~/Content/Site.css",
                "~/Content/Site.mobile.css",
                 "~/Content/loading.css"
                ));

            /*kendo ui */
            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                     "~/Scripts/kendo/kendo.all.min.js",
                     "~/Scripts/kendo/kendo.custom.js",
                     "~/Scripts/kendo/jszip.min.js"));

            /*bootstrap-tagsinput */
            bundles.Add(new ScriptBundle("~/bundles/tagsinput").Include(
                     "~/Scripts/bootstrap-tagsinput.min.js"));

            /* metisMenu */
            bundles.Add(new ScriptBundle("~/bundles/metisMenu").Include(
                     "~/Scripts/metisMenu.min.js"));

            /* moment */
            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                     "~/Scripts/moment-with-locales.min.js"));

            /* Full Calander */
            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include(
                     "~/Scripts/fullcalendar/fullcalendar.min.js"));

            // custom
            bundles.Add(new ScriptBundle("~/Scripts/custom").Include(
                   "~/Scripts/custom/*.js"));

        }
    }
}

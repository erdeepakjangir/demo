using System.Web.Optimization;

namespace Survey.Web
{
    internal static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/json3*",
                "~/Scripts/jquery-3.1.1.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.validate.unobtrusive*",
                "~/Scripts/jquery.validate.unobtrusive.bootstrap*",
                "~/Scripts/jquery.unobtrusive-ajax*",
                "~/Scripts/bootstrap*",
                "~/Scripts/bootstrap-multiselect.js",
                "~/Scripts/DataTables/jquery.dataTables.*",
                "~/Scripts/DataTables/dataTables.responsive.*",
                "~/Scripts/chosen.*",
                "~/Scripts/d3.v3.js",
                "~/Scripts/d3.v3.min.js",
                "~/Scripts/custom/common.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/validationjs").Include(
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.validate.unobtrusive*"

           ));



            bundles.Add(new ScriptBundle("~/bundles/studentjs").Include(
                "~/Scripts/custom/dashboard.js",
             "~/Scripts/custom/qualification.js"
             ));


            bundles.Add(new ScriptBundle("~/bundles/facultyjs").Include(
                "~/Scripts/custom/dashboard.js",
            "~/Scripts/custom/qualification.js",
             "~/Scripts/custom/studentDetail.js",
              "~/Scripts/moment.js",
              "~/Scripts/datetime-moment.js",
             "~/Scripts/DataTables/dataTables.buttons.js",
              "~/Scripts/jszip.js",
              "~/Scripts/pdfmake/pdfmake.js",
               "~/Scripts/pdfmake/vfs_fonts.js",
           "~/Scripts/DataTables/buttons.html5.js",
           "~/Scripts/DataTables/buttons.flash.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/reportjs").Include(
              "~/Scripts/custom/report.js"
           ));




            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css",
                "~/Content/bootstrap-theme.css",
                "~/Content/bootstrap-datepicker3.css",
                "~/Content/DataTables/css/jquery.dataTables.css",
                "~/Content/DataTables/css/responsive.dataTables.css",
                "~/Content/site.css",
                "~/Content/font-awesome.min.css",
                 "~/Content/chosen.css"));
        }
    }
}

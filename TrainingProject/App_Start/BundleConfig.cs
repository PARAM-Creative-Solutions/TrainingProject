using System.Web;
using System.Web.Optimization;

namespace TrainingProject
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/validateDate").Include(
                       "~/Scripts/validateDate.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're

            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapJS").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrapCSS").Include(
                      "~/Content/bootstrap.css"));

            #region Bundal For Login

            #region CSS
            bundles.Add(new StyleBundle("~/Content/LoginCSS").Include(
                    "~/Content/bootstrap.css",
                     "~/Content/Login/form-elements.css",
                     "~/Content/Login/style.css"));
            #endregion

            #region JS
            bundles.Add(new ScriptBundle("~/bundles/LoginJS").Include(
                 "~/Scripts/bootstrap.js",
                   "~/Scripts/Login/jquery.backstretch.min.js",
                   "~/Scripts/Login/scripts.js"));
            #endregion

            #endregion

            

            #region Bundal For Layout


            #region Font awesome
            bundles.Add(new StyleBundle("~/Content/fontsCSS").Include(
                    "~/Content/fontawesome-all.min.css",
                    "~/Content/font-awesome.min.css",
                    "~/Content/font-awesome-animation.min.css"));
            #endregion

            #region FastClick
            bundles.Add(new ScriptBundle("~/bundles/fastclickJS").Include(
               "~/Scripts/fastclick.js"));
            #endregion

            #region NProgress
            bundles.Add(new StyleBundle("~/Content/nprogressCSS").Include(
                   "~/Content/nprogress.css"));
            bundles.Add(new ScriptBundle("~/bundles/nprogressJS").Include(
                   "~/Scripts/nprogress.min.js"));
            #endregion

            #region Custom Template
            bundles.Add(new StyleBundle("~/Content/customThemeCSS").Include(
               "~/Libs/Gentelella/css/custom.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/customThemeJS").Include(
                    "~/Libs/Gentelella/js/custom.min.js"));
            #endregion

            #region mCustomScrollbar
            bundles.Add(new StyleBundle("~/Content/mCustomScrollbarCSS").Include(
               "~/Content/jquery.mCustomScrollbar.css"));
            bundles.Add(new ScriptBundle("~/bundles/mCustomScrollbarJS").Include(
                     "~/Scripts/jquery.mCustomScrollbar.js"));
            #endregion

            #region Datatable
            bundles.Add(new StyleBundle("~/Content/datatableCSS").Include(
            "~/Content/DataTables/css/dataTables.bootstrap.min.css",
            "~/Content/DataTables/css/buttons.bootstrap.min.css",
            "~/Content/DataTables/css/fixedHeader.bootstrap.min.css",
            "~/Content/DataTables/css/responsive.bootstrap.min.css",
            "~/Content/DataTables/css/scroller.bootstrap.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/datatableJS").Include(
                    "~/Scripts/DataTables/jquery.dataTables.min.js",
    "~/Scripts/DataTables/dataTables.bootstrap.min.js",
    "~/Scripts/DataTables/dataTables.buttons.min.js",
    "~/Scripts/DataTables/buttons.bootstrap.min.js",
    "~/Scripts/DataTables/buttons.flash.min.js",
    "~/Scripts/DataTables/buttons.html5.min.js",
    "~/Scripts/DataTables/buttons.print.min.js",
    "~/Scripts/DataTables/dataTables.fixedHeader.min.js",
    "~/Scripts/DataTables/dataTables.keyTable.min.js",
    "~/Scripts/DataTables/dataTables.responsive.min.js",
    "~/Scripts/DataTables/responsive.bootstrap.min.js",
    "~/Scripts/DataTables/dataTables.scroller.min.js",
    "~/Scripts/jszip.min.js",
    "~/Scripts/pdfmake/pdfmake.min.js",
    "~/Scripts/pdfmake/vfs_fonts.js"));
            #endregion

            #endregion

            #region Option Buttons
            bundles.Add(new StyleBundle("~/Content/cssOptionButtons").Include(
                      "~/Content/OptionButton.css"));
            #endregion

            #region CSS & JS Bundle For PNOTIFY MESSAGES BOX

               bundles.Add(new StyleBundle("~/Content/cssPNotify").Include(
                       "~/Content/Pnotify/pnotify.custom.css"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryPNotify").Include(
                      "~/Scripts/PNotify/pnotify.custom.js",
                      "~/Scripts/AppScripts/PNotifyMessages.js"));
            #endregion

            #region CSS and JS Bundle Used For File Upload Panel for Common Document Upload


            bundles.Add(new StyleBundle("~/Content/jQueryFileUpload").Include(
                    "~/Content/jQuery.FileUpload/css/jquery.fileupload.css",
                   "~/Content/jQuery.FileUpload/css/jquery.fileupload-ui.css"
                   ));

            bundles.Add(new ScriptBundle("~/bundles/jQueryFileUpload").Include(
                    //<!-- The Templates plugin is included to render the upload/download listings -->
                    // "~/Scripts/jquery-ui-1.9.0.min",
                    "~/Scripts/jQuery.FileUpload/JqueryWidge/jquery.ui.widget.js",
                       "~/Scripts/jQuery.FileUpload/tmpl.min.js",
//<!-- The Load Image plugin is included for the preview images and image resizing functionality -->
"~/Scripts/jQuery.FileUpload/load-image.all.min.js",
//<!-- The Canvas to Blob plugin is included for image resizing functionality -->
"~/Scripts/jQuery.FileUpload/canvas-to-blob.min.js",
//"~/Scripts/file-upload/jquery.blueimp-gallery.min.js",
//<!-- The Iframe Transport is required for browsers without support for XHR file uploads -->
"~/Scripts/jQuery.FileUpload/jquery.iframe-transport.js",
//<!-- The basic File Upload plugin -->
"~/Scripts/jQuery.FileUpload/jquery.fileupload.js",
//<!-- The File Upload processing plugin -->
"~/Scripts/jQuery.FileUpload/jquery.fileupload-process.js",
//<!-- The File Upload image preview & resize plugin -->
"~/Scripts/jQuery.FileUpload/jquery.fileupload-image.js",
//<!-- The File Upload audio preview plugin -->
"~/Scripts/jQuery.FileUpload/jquery.fileupload-audio.js",
//<!-- The File Upload video preview plugin -->
"~/Scripts/jQuery.FileUpload/jquery.fileupload-video.js",
//<!-- The File Upload validation plugin -->
"~/Scripts/jQuery.FileUpload/jquery.fileupload-validate.js",
//!-- The File Upload user interface plugin -->
"~/Scripts/jQuery.FileUpload/jquery.fileupload-ui.js"));
            #endregion

            #region Bunndal For Chart
            bundles.Add(new ScriptBundle("~/bundles/ChartJS").Include(
                 "~/Scripts/Charts/Chart.min.js",
                 "~/Scripts/Charts/utils.js"));
            #endregion

            #region jqueryDataTable Bundle Used To Load The Scripts of Jquery Datatable

            bundles.Add(new ScriptBundle("~/bundles/DataTableJS").Include(
                        "~/Scripts/DataTables/jquery.dataTables.min.js",
                      "~/Scripts/DataTables/dataTables.bootstrap.min.js",
                       "~/Scripts/DataTables/dataTables.buttons.min.js",
                       "~/Scripts/DataTables/buttons.bootstrap.min.js",
                       "~/Scripts/DataTables/jszip.min.js",
                       "~/Scripts/DataTables/pdfmake.min.js",
                       "~/Scripts/DataTables/vfs_fonts.js",
                       "~/Scripts/DataTables/buttons.flash.min.js",
                       "~/Scripts/DataTables/buttons.html5.min.js",
                       "~/Scripts/DataTables/buttons.print.min.js",
                       "~/Scripts/DataTables/buttons.colVis.min.js",
                       "~/Scripts/DataTables/dataTables.select.min.js",
                       "~/Scripts/DataTables/dataTables.fixedColumns.min.js",
                        "~/Scripts/DataTables/dataTables.fixedHeader.min.js",
                        "~/Scripts/DataTables/dataTables.keyTable.min.js",
                        "~/Scripts/DataTables/dataTables.responsive.min.js",
                        "~/Scripts/DataTables/dataTables.scroller.min.js",
                        "~/Scripts/DataTables/dataTables.rowReorder.min.js",
                        "~/Scripts/DataTables/responsive.bootstrap.js",
                        "~/Scripts/DataTables/customDatatable.js"
                       ));

          
            #endregion

            #region cssDataTable Bundle For Datatable CSS

            bundles.Add(new StyleBundle("~/Content/DataTableCSS").Include(
                        //"~/Content/DataTables/css/buttons.dataTables.min.css",
                        "~/Content/DataTables/css/dataTables.bootstrap.min.css",
                        "~/Content/DataTables/css/buttons.bootstrap.min.css",
                        "~/Content/DataTables/css/fixedHeader.bootstrap.min.css",
                         "~/Content/DataTables/css/fixedColumns.bootstrap.min.css",
                          "~/Content/DataTables/css/rowReorder.dataTables.min.css",
                        "~/Content/DataTables/css/responsive.bootstrap.min.css",
                        "~/Content/DataTables/css/scroller.bootstrap.min.css"));

            #endregion

            #region Datepicker
            bundles.Add(new StyleBundle("~/Content/DatePickerCSS").Include(
                "~/Content/Datepicker/bootstrap-datepicker.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/DatePickerJS").Include(
                "~/Scripts/Datepicker/bootstrap-datepicker.js"));

            #endregion

            #region FullCalendar
            bundles.Add(new StyleBundle("~/Content/fullcalendarCSS").Include(
                "~/Content/fullcalendar.min.css"
               // "~/Content/fullcalendar.print.css"
                ));
            bundles.Add(new ScriptBundle("~/bundles/fullcalendarJS").Include(
                "~/Scripts/moment.min.js",
                "~/Scripts/fullcalendar/fullcalendar.min.js"));

            #endregion

            #region jQuery Smart Wizard
            bundles.Add(new StyleBundle("~/Content/jquerySmartWizardCSS").Include(
               "~/Content/jQuery.smartWizard/smart_wizard.css",
               "~/Content/jQuery.smartWizard/smart_wizard_theme_arrows.min.css",
               "~/Content/jQuery.smartWizard/smart_wizard_theme_circles.min.css",
               "~/Content/jQuery.smartWizard/smart_wizard_theme_dots.min.css",
               "~/Content/jQuery.smartWizard/smart_wizard_vertical.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquerySmartWizardJS").Include(
                "~/Scripts/jQuery.smartWizard/jquery.smartWizard.js"));
            #endregion

            #region FoolProof Validation for validation depanedancey

            bundles.Add(new ScriptBundle("~/bundles/mvcFoolProof")
       .Include("~/Scripts/MicrosoftAjax*",
                "~/Scripts/MicrosoftMvcAjax*",
                "~/Scripts/MicrosoftMvcValidation*",
                "~/Client Scripts/mvcfoolproof*",
                "~/Client Scripts/MvcFoolproofJQueryValidation*",
                "~/Client Scripts/MvcFoolproofValidation*"));

            #endregion


            //the following creates bundles in debug mode;
            BundleTable.EnableOptimizations = false;
        }
    }
}

using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InspectionCall
{
    public partial class ReportViewerWebForm : ReportViewerForMvc.ReportViewerWebForm
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ReportViewer1.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            //Entities entities = new Entities();
            //var ID = Convert.ToInt32(e.Parameters["DeptId"].Values[0]);
            //var useregroup = entities.Users.Where(x => x.DeptId == ID);
            //var employeeDetails = new ReportDataSource() { Name = "DataSet1", Value = useregroup };
            //e.DataSources.Add(employeeDetails);
            //if (e.ReportPath == "SubReport")
            //{
            //    var employeeDetails = new ReportDataSource() { Name = "DataSet1", Value = useregroup };
            //    e.DataSources.Add(employeeDetails);
            //}
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingProjectDataLayer.Reports
{
    public class Reports
    {
        public int NumberOfCallRaised { get; set; }
        public int NumberOfCallWaived { get; set; }
        public int NumberOfCallattended { get; set; }

        public string SupplierName { get; set; }
        public string PlantCode { get; set; }

        public DateTime InspectionCallRaisedOn { get; set; }
        public DateTime InspectionCallDate { get; set; }
        public DateTime InspectionCallAttendedOn { get; set; }

        public string  Remark { get; set; }

        [Required(ErrorMessage = "Please Select Start Date")]
        public DateTime? StartDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1);
       
        [Required(ErrorMessage = "Please Select End Date")]
        public DateTime? EndDate { get; set; } = DateTime.Now;

    }

  
}

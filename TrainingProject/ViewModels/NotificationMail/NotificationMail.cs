using TrainingProjectDataLayer.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainingProject.ViewModels.NotificationMail
{
    public class PendingDocViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string SalesOrderNumber { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Unique_So_No { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Property for SalesOrder name
        /// </summary>        
        [Display(Name = "SalesOrder Name ")]
        public string SalesOrderName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DocName { get; set; }

        /// <summary>
        /// Property for Version Number
        /// </summary>
        [Display(Name = "Latest Version")]
        public string LatestVersion { get; set; }

        /// <summary>
        /// Property for Version Number
        /// </summary>
        [Display(Name = "Document Submission Status")]
        public string DocSubStatus { get; set; }

        /// <summary>
        /// Property for Version Number
        /// </summary>
        [Display(Name = "Due Date")]
        [DisplayFormat(DataFormatString = Constants.DateFormat, ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }

        ///// <summary>
        ///// Property for TOPEngg name initials
        ///// </summary>
        //[Display(Name = "Engg")]
        //public string Engg { get; set; }
    }
}
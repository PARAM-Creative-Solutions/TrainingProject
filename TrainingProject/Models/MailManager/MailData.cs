using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingProject.Models.MailManager
{
    /// <summary>
    /// Class for get and set the values for mail Sending
    /// addd by priyanka gaharwal
    /// </summary>
    public class MailData
    {

        /// <summary>
        /// Property for From field of mail sending
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Property for To field of mail sending
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Property for Body field of mail sending
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the File path with file name to be attched to Mail
        /// </summary>
        public List<string> Filepath { get; set; }

        /// <summary>
        /// Property for Subject field of mail sending
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Property for Subject field of mail sending
        /// </summary>
        public string MailContentNote { get; set; }

        /// <summary>
        /// Property for Cc field of mail sending
        /// </summary>
        public string Cc { get; set; }

        /// <summary>
        /// Gets or sets BCc property for sending Email
        /// </summary>
        public string BCc { get; set; }

        /// <summary>
        /// Gets or sets the mailid of TOP engg which is kept in the Cc and reply to this mail will get send to TOP engg
        /// </summary>
        public string ReplyBackTo { get; set; }

        /// <summary>
        /// Object of User
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Property for displaying user name 
        /// </summary>
        public string UserName { get; set; }

       

        /// <summary>
        /// Maximum Size of attachtment file
        /// </summary>
        public int MaxFileSize { get; set; } = 25 * 1024 * 1024;
       
        /// <summary>
        /// Gets or sets the mailid of TOP engg which is kept in the Cc and reply to this mail will get send to TOP engg
        /// </summary>
        public HttpPostedFileBase Fileattachment { get; set; }

        public Department CurrentUserDepartment { get; set; }

        public byte[] bytes { get; set; }

        public string Extrainfo { get; set; }

        public string FileName { get; set; }

        //public InspectionCallRequest objInspectionCallRequest;
    }
}
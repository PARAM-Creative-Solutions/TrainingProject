using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace TrainingProject.Models.MailManager
{
    /// <summary>
    /// Class for Sending mail at background
    /// added by priyanka gaharwal
    /// </summary>
    public class Mail
    {
        /// <summary>
        /// Method for Sending mail in background which takes the credetials for sending mail in background
        /// </summary>
        /// <param name="mailData">object of MailData class</param>
        /// <returns></returns>
        public static async Task<bool> SendMail(MailData mailData)
        {

            #region GetMailId From web config
            bool IsTestingMails =Convert.ToBoolean(WebConfigurationManager.AppSettings["IsTestingMails"]);
            string DeafultMailTo = WebConfigurationManager.AppSettings["DeafultMailTo"] ;
            string DeafultMailCC = WebConfigurationManager.AppSettings["DeafultMailCC"] ;
            string DeafultMailBCC = WebConfigurationManager.AppSettings["DeafultMailBCC"];

            if (IsTestingMails)
                DeafultMailBCC = string.Empty;
            else
            {
                if (string.IsNullOrEmpty(mailData.To))
                    DeafultMailTo = DeafultMailTo + "," + mailData.To;
                else
                    DeafultMailTo = mailData.To;
                DeafultMailCC = mailData.Cc;
                DeafultMailBCC = DeafultMailBCC + "," + mailData.BCc;
            }
            #endregion

            UniqueMailIds finalMails = new UniqueMailIds(DeafultMailTo, DeafultMailCC, DeafultMailBCC);


            #region Mail Content
            var message = new MailMessage();
            string FromMail = WebConfigurationManager.AppSettings["DeafultMailFrom"];
            message.From = new MailAddress(FromMail, "KSB Training Project");  // replace with valid value
            message.Subject = mailData.Subject;
            message.Body = mailData.Body;
            message.IsBodyHtml = true;

            if (!string.IsNullOrEmpty(finalMails.To))
                message.To.Add(finalMails.To);
            if (!string.IsNullOrEmpty(finalMails.CC))
                message.CC.Add(finalMails.CC);
            if (!string.IsNullOrEmpty(finalMails.BCC))
                message.Bcc.Add(finalMails.BCC);

            if (!string.IsNullOrEmpty(mailData.ReplyBackTo))
            {
                //message.ReplyToList.Add(new MailAddress(mailData.ReplyBackTo));
            }
            
            if (mailData.Filepath != null)
            {
                long Size = 0;
                for (int i = 0; i < mailData.Filepath.Count; i++)
                {
                    message.Attachments.Add(new Attachment(mailData.Filepath[i]));
                    Size = Size + message.Attachments.Last().ContentStream.Length;
                }
                if (mailData.Fileattachment != null)
                {
                    message.Attachments.Add(new Attachment(mailData.Fileattachment.InputStream, Path.GetFileName(mailData.Fileattachment.FileName)));
                    Size = Size + message.Attachments.Last().ContentStream.Length;
                }
                long mybyt = BitConverter.ToInt64(BitConverter.GetBytes(Size), 0);
                if (mybyt > mailData.MaxFileSize)
                {
                    message.Attachments.Clear();
                    message.Subject = message.Subject + " (Attactment could not be attached as its exceeded Maximum File Size)";
                }
            }

            if (mailData.bytes != null)
                message.Attachments.Add(new Attachment(new MemoryStream(mailData.bytes), mailData.FileName));

            #endregion
        
            using (var smtp = new SmtpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };
                    smtp.Send(message);
                message.Attachments.Dispose();
                return true;
            }
        }
    }

    public class UniqueMailIds
    {
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }

        private string MailTo { get; set; }
        private string MailCC { get; set; }
        private string MailBCC { get; set; }

        public UniqueMailIds()
        {

        }

        public UniqueMailIds(string MailTo, string MailCC, string MailBCC)
        {
            this.MailTo = MailTo;
            this.MailCC = MailCC;
            this.MailBCC = MailBCC;
            GetUniqueMailIds();
        }

        public void GetUniqueMailIds()
        {
            HashSet<string> HashSetTo = new HashSet<string>();
            HashSet<string> HashSetCC = new HashSet<string>();
            HashSet<string> HashSetBCC = new HashSet<string>();
            if (!string.IsNullOrEmpty(MailTo))
                HashSetTo = new HashSet<string>(MailTo.Split(',').ToList().Where(w => !string.IsNullOrEmpty(w)));
            if (!string.IsNullOrEmpty(MailCC))
                HashSetCC = new HashSet<string>(MailCC.Split(',').ToList().Where(w => !string.IsNullOrEmpty(w)));
            if (!string.IsNullOrEmpty(MailBCC))
                HashSetBCC = new HashSet<string>(MailBCC.Split(',').ToList().Where(w => !string.IsNullOrEmpty(w)));

            HashSetTo.All(t => HashSetCC.Remove(t));
            HashSetTo.All(t => HashSetBCC.Remove(t));

            HashSetCC.All(t => HashSetTo.Remove(t));
            HashSetCC.All(t => HashSetBCC.Remove(t));

            HashSetBCC.All(t => HashSetTo.Remove(t));
            HashSetBCC.All(t => HashSetCC.Remove(t));

            To = string.Join(",", HashSetTo);
            CC = string.Join(",", HashSetCC);
            BCC = string.Join(",", HashSetBCC);
        }

    }
}
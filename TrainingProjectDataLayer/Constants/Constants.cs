using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using static TrainingProjectDataLayer.Constants.SystemEnums;





namespace TrainingProjectDataLayer.Constants
{
    /// DO NOT CHANGE THE ENUM TYPE INDEXES BECAUSE THESE INDEXES ARE GIVEN IN THE DATABASE AND USED IT WHEREVER REQUIRED
    /// 

    #region SystemEnums
    /// <summary>
    /// SystemEnums
    /// </summary>
    public class SystemEnums
    {

        #region Rights
        /// <summary>
        /// Rights This Enum is Define as per database rights...
        /// For Proper Working of system both should be same
        /// </summary>
        public enum Rights
        {
            #region Master Data
            /// <summary>
            ///Manage MasterDocumentList Master Data
            /// </summary>
            [Description("Manage Master Data")]
            [Right("Manage System Master Data", Status = (int)RightsStatus.IA)]
            ManageSystemMasterData = 0, 
            #endregion

            #region Manage Master Data

            /// <summary>
            ///Manage MasterDocumentList Master Data
            /// </summary>
            [Description("Manage Master Data")]
            [Right("Manage Plant Master Data")]
            ManagePlantLevelMasterData, 
            #endregion

            #region Department Head

            /// <summary>
            ///Manage MasterDocumentList Master Data
            /// </summary>
            [Description("Manage Department Data")]
            [Right("Manage Department Data")]
            ManageDepartmentData,
            #endregion

            #region Report

            /// <summary>
            ///Manage MasterDocumentList Master Data
            /// </summary>
            [Description("View Report Type")]
            [Right("View Report")]
            ViewReports,
            #endregion

            #region View All Users Data
            /// <summary>
            ///view data of other users also(for head)
            /// </summary>
            [Description("View All Users Data")]
            [Right("View All Users Data")]
            ViewAllUsersData,
            #endregion

            [Description("Create Inspection Call")]
            [Right("Create Inspection Call")]
            CreateInspectionCall,

            [Description("Update Inspection Call")]
            [Right("Update Inspection Call KSB")]
            UpdateInspectionCallByKSB,

            [Description("Update Inspection Call")]
            [Right("Update Inspection Call Supplier")]
            UpdateInspectionCallBySupplier,

            [Description("View All Inspection Calls Of Plant")]
            [Right("View All Inspection Calls Of Plant")]
            ViewAllInspectionCallsOfPlant,

            [Description("View All Plants Inspection Calls")]
            [Right("View All Plants Inspection Calls")]
            ViewAllPlantsInspectionCalls,

            [Description("View Document")]
            [Right("View Document")]
            ViewDocument,

            [Description("Download Document")]
            [Right("Download Documents")]
            DownloadDocument,

            [Description("ChangeDocument")]
            [Right("Change Document")]
            ChangeDocument,

            [Description("Only View")]
            [Right("Only View")]
            OnlyView

        }

       

        /// <summary>
        /// Use To define RightStatus
        /// </summary>
        public enum RightsStatus
        {
            A = 1,
            IA = 2,

        }
        #endregion

        #region Departments

        /// <summary>
        /// Gets Roles
        /// </summary>
        public enum Departments
        {
            /// <summary>
            /// ADMIN Department
            /// </summary>
            [Description("ADMIN")]
            ADMIN = 1,

            /// <summary>
            /// TOP
            /// </summary>
            [Description("QA")]
            QA = 2,

            [Description("SUPPLIER")]
            SUPPLIER = 14,

            [Description("Other")]
            OTHER = 17,

        }

        #endregion

        #region Status
        /// <summary>
        /// enum for Status
        /// </summary>
        public enum UserStatus
        {
            [Description("Active")]
            Active = 1,

            [Description("In Active")]
            InActive = 2,

            [Description("Suspended")]
            Suspended = 3,
        }

        public enum PlantStatus
        {
            [Description("Active")]
            Active = 1,

            [Description("In Active")]
            InActive = 2,

        }
        #endregion

        #region Colors
        public enum Colors
        {
            [Description("#FCB5B0")]
            Red,
            [Description("#DEF5D2")]
            Green,
            [Description("#FFFF99")]
            Yellow
        }
        #endregion

        #region UserManual Types

        /// <summary>
        ///  Gets the User maual Types for download
        /// </summary>
        public enum UserManualTypes
        {
            ///// <summary>
            ///// Used to get the Admin user manual
            ///// </summary>
            //[Description("Smart Barrier User Manual for Admin.pdf")]
            //AdminManual = 0,

            ///// <summary>
            ///// Used to get the "User" user manual
            ///// </summary>
            //[Description("Smart Barrier User Manual for User.pdf")]
            //UserManual = 1,

            /// <summary>
            /// Used to get the Admin user manual
            /// </summary>
            [Description("TSSHeadManual.pdf")]
            TSSHeadManual = 0,

            /// <summary>
            /// Used to get the Admin user manual
            /// </summary>
            [Description("TSSUserManual.pdf")]
            TSSUserManual = 1,
        }

        #endregion

        #region Report
        /// <summary>
        /// Report Design types
        /// </summary>
        public enum ReportDesignType
        {
            /// <summary>
            /// Dynamic Report No RDLC File Required
            /// Report Which Includes Only Table
            /// </summary>
            TableReport = 1,

            /// <summary>
            /// Dynamic Report No RDLC File Required
            /// Report Which Includes  Table as well as details
            /// </summary>
            TableDetailReport = 2,

            /// <summary>
            /// Rdlc file should be present
            /// </summary>
            Other = 3

        }

        /// <summary>
        /// Report Design types
        /// </summary>
        public enum OtherReports
        {

            [Description("Supplier wise call report")]
            SupplierWiseCallreport = 1,

            [Description("Age analysis report for call raised and attended by KSB ")]
            AgeAnalysis = 2,

            [Description("Waived call report")]
            WaivedCallReport = 3,

           
            [Description("Rejected/Cancelled call report ")]
            RejectedCallReport = 4,

            [Description("Pending call report")]
            PendingCallReport =5,

           [Description("Inspection Stage Wise Data")]
            InspectionStageWiseData = 6
               
        }



        #endregion

    }

    #endregion

    #region Constants

    /// <summary>
    /// Get all the constants required for application
    /// </summary>
    public class Constants
    {
        #region Static Properties

        #region DropdownListOptions
        public static List<string> DropdownCheckList { get { return new List<string> { "Yes", "No" }; } }
        public static List<string> DropdownImportance { get { return new List<string> { "High", "Medium", "Low" }; } }
        public const string SelectOption = "--Select--";
        #endregion

        #region EncryptionDecryptionenable 

        /// <summary>
        /// Gets the true/false for encryption and decryption
        /// </summary>
        public static bool IsEncryptionDecryptionEnable { get { return false; } }

        #endregion

        #region Date Format
        /// <summary>
        /// Gets the format of date to be shown on ui and saved in database
        /// </summary>
         public static string DateFormat1 { get { return "dd-mm-yyyy"; } }
        #endregion

        #endregion

        #region Const
        public const string DateFormat = "{0:dd-MM-yyyy}"; //"dd-mm-yyyy";  //

        public const string SAPDestinationFolder = "SAP Files";

    

        #region Report data Table name
        /// Follwing Names are of datatables..
        /// These Are used in Unit Of Work GetReportData Method
        /// To identify table from database
        public const string dsTable = "Table";
        public const string dsDetail = "Detail";

        /// <summary>
        /// Shows the option of Select in dropdown bydefault
        /// </summary>
        public const string Option = "--Select--";

        /// <summary>
        /// Key for encryption and decryption
        /// </summary>
        public const string Key = "KsbPa$4&";

        /// <summary>
        /// Gets the path of SalesOrder
        /// </summary>
        public const string SalesOrder_Path = "SalesOrder_Path";

        /// <summary>
        /// Gets the path of SAP Folder
        /// </summary>
        public const string SAP_SourceFolderPath = "SAP_SourceFolderPath";

        #endregion        
        #endregion
    }

    #endregion

    #region HelperMethods
    /// <summary>
    /// Helper Methods Defin in this class
    /// </summary>
    public static class HelperMethods
    {
        #region GetAttribute
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name) // I prefer to get attributes this way
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }
        #endregion

        #region GetDescription
        /// <summary>
        /// Gets the description of the enum object
        /// </summary>
        /// <param name="Type"></param>
        /// <returns>description of the enum type</returns>
        public static string GetDescription(object Type)
        {
            FieldInfo fi = Type.GetType().GetField(Type.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : Type.ToString();
        }
        //public static string GetMailHeading(SystemEnums.InspectionCallStatus Type)
        //{
        //    FieldInfo fi = Type.GetType().GetField(Type.ToString());
        //    DescriptorAttribute[] attributes =
        //        (DescriptorAttribute[])fi.GetCustomAttributes(typeof(DescriptorAttribute), false);
        //    return (attributes.Length > 0) ? attributes[0].MailHeading : Type.ToString();
        //}

        #endregion

        #region GetEnumValueFromDescription
        /// <summary>
        /// Gets the enum value from description of enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(a => ((DescriptionAttribute)a.Att)
                                .Description == description).SingleOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }

        #endregion

        #region GetEnumRightIntoList
        public static List<Right> GetEnumRightIntoList()
        {
            List<Right> AllRights = new List<Right>();
            List<Rights> EnumValues = Enum.GetValues(typeof(Rights)).Cast<Rights>().ToList();
            foreach (Rights item in EnumValues)
            {
                Right AttributeRight = HelperMethods.GetAttribute<Right>(item);
                if (AttributeRight != null)
                {
                    AttributeRight.RightId = (int)item;
                    AttributeRight.Description = HelperMethods.GetAttribute<DescriptionAttribute>(item).Description;
                    if (AttributeRight.Status != (int)RightsStatus.IA)
                        AllRights.Add(AttributeRight);
                }
                else
                {
                    AllRights.Add(new Right()
                    {
                        //Action = "",
                        //Controller = "",
                        Description = "",
                        RightId = (int)item,
                        RightName = "Enum_" + item.ToString()
                    });
                }
            }
            return AllRights;
        }
        #endregion

        #region GetHostDetails

        /// <summary>
        /// Gets the Computer Name of the Client Machine
        /// </summary>
        /// <param name="pContext">Current HttpContext</param>
        /// <returns>Computer Name of the client machine</returns>
        public static string GetHostDetails(HttpContextBase pContext)
        {
            string Name = "";
            try
            {
                // Dns.GetHostEntry(Context.Request.GetHttpContext().Request.ServerVariables["remote_addr"]).HostName;
                IPAddress myIP = IPAddress.Parse(pContext.Request.UserHostAddress);
                string ipList = pContext.Request.ServerVariables["LOCAL_ADDR"];
                IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
                //******computer Name
                List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
                Name = compName.Count() != 0 ? compName.First() : "";
                //**********ip address
                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip))
                {
                    ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                IPHostEntry GetIPHost1 = Dns.GetHostEntry(ip);
            }
            catch (Exception ex)
            {
                return Name;
            }
            return Name;
        }
        public static string GetIp(System.Web.HttpContext context)
        {
            string ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }

        /// <summary>
        /// Gets the Ip Address of the Client Machine
        /// </summary>
        /// <param name="pContext">Current HttpContext</param>
        /// <returns>Computer Name of the client machine</returns>
        public static string GetIPAddress(HttpContextBase pContext)
        {

            // return ip;
            //string Ip = string.Empty;
            //IPHostEntry Host = default(IPHostEntry);
            //string Hostname = null;
            //Hostname = System.Environment.MachineName;
            //Host = Dns.GetHostEntry(Hostname);
            //foreach (IPAddress IP in Host.AddressList)
            //{
            //    if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //    {
            //        Ip = Convert.ToString(IP);
            //    }
            //}
            //return Ip;
            string ipAddress = string.Empty;
            ipAddress = pContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = pContext.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ipAddress;
        }



        #endregion

        #region GetYesNoSelectList
        /// <summary>
        /// Method to return select list Yes/No for Dropdownlist (True/False)
        /// </summary>
        /// <returns></returns>
        //public static SelectList YesNoSelectList(bool Selected)
        //{
        //    var Data = from IsApplicable s in Enum.GetValues(typeof(IsApplicable)) select new { Text = s.ToString(), Value = Convert.ToBoolean((int)s) };
        //    return new SelectList(Data, "Value", "Text", Selected);
        //}
        public static SelectList GetYesNoSelectList(bool Selected = false)
        {
            SelectListItem[] selectListItems = new SelectListItem[2];

            var itemTrue = new SelectListItem();
            itemTrue.Value = "false";
            itemTrue.Text = "No";
            selectListItems[0] = itemTrue;

            var itemFalse = new SelectListItem();
            itemFalse.Value = "true";
            itemFalse.Text = "Yes";
            selectListItems[1] = itemFalse;

            var selectList = new SelectList(selectListItems, "Value", "Text", Selected);

            return selectList;

        }

        #endregion

        public static SelectList SelectList<T>()
        {
            EnumHelper.GetSelectList(typeof(T));
            var Data = from T s in Enum.GetValues(typeof(T)) select new { Text = s.ToString(), Value = Convert.ToBoolean(s.ToString()) };
            return new SelectList(Data, "Value", "Text");
        }

        public static string EncryptURLId(string ToEncrypt)
        {
         string s=   Convert.ToBase64String(Encoding.ASCII.GetBytes(ToEncrypt));
            return s;
        }
        public static string DecryptURLId(string cypherString)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(cypherString));
        }

        #region INSPECTION CALL

        #region CheckString
        public static string CheckString(string input)
        {
            return input!=null ? input : string.Empty;
        }
        #endregion



        /// <summary>
        /// Creates the DropDown List (HTML Select Element) from LINQ 
        /// Expression where the expression returns an Enum type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
            where TModel : class
        {
            TProperty value = htmlHelper.ViewData.Model == null
                ? default(TProperty)
                : expression.Compile()(htmlHelper.ViewData.Model);
            string selected = value == null ? String.Empty : value.ToString();
            return htmlHelper.DropDownListFor(expression, createSelectList(expression.ReturnType, selected));
        }

        /// <summary>
        /// Creates the select list.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="selectedItem">The selected item.</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> createSelectList(Type enumType, string selectedItem)
        {
            return (from object item in Enum.GetValues(enumType)
                    let fi = enumType.GetField(item.ToString())
                    let attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault()
                    let title = attribute == null ? item.ToString() : ((DescriptionAttribute)attribute).Description
                    select new SelectListItem
                    {
                        Value = item.ToString(),
                        Text = title,
                        //Selected = selectedItem == item.ToString()
                    }).ToList();
        }

        /// <summary>
        /// Creates the select list.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="selectedItem">The selected item.</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> createSelectList<T>()
        {
            Type enumType = typeof(T);

            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var item in Enum.GetValues(enumType))
            {
                var fi = enumType.GetField(item.ToString());
                var attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
                var title = attribute == null ? item.ToString() : ((DescriptionAttribute)attribute).Description;
                DescriptorAttribute descriptorAttribute = fi.GetCustomAttribute<DescriptorAttribute>();
                if (descriptorAttribute==null ||(descriptorAttribute != null&&descriptorAttribute.DisplayInDropDown ))
                {
                    list.Add(new SelectListItem
                    {
                        Value = Convert.ToInt32(item).ToString(),
                        Text = title,
                        //Selected = selectedItem == item.ToString()
                    });
                }

            }


            //return (from object item in Enum.GetValues(enumType)
            //        let fi = enumType.GetField(item.ToString())
            //        let attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault()
            //        let title = attribute == null ? item.ToString() : ((DescriptionAttribute)attribute).Description
            //        let descriptorAttribute = fi.GetCustomAttribute<DescriptorAttribute>()

            //        select new SelectListItem
            //        {
            //            Value = Convert.ToInt32(item).ToString(),
            //            Text = title,
            //            //Selected = selectedItem == item.ToString()
            //        }).ToList();

            return list;
        }
        public static IEnumerable<SelectListItem> DocumentStatusList(IEnumerable<SelectListItem> List , string DocumentType   )
        {
            return List;
        }

            //public static string GetHolidays()

            //{
            //    Holiday[] HolidayList;
            //    using (UnitofWork uow = new UnitofWork(new InspectionCallEntities()))
            //    {
            //       HolidayList = uow.HolidayRepository.GetAll().ToArray();
            //    }



            //    return JsonConvert.SerializeObject(  HolidayList;

            //}



            #endregion
        }
    #endregion

    #region HELPER CLASSES
    public class DescriptorAttribute : Attribute
    {
        public string MailHeading { get; }
        public bool DisplayInDropDown { get; }

        public DescriptorAttribute(string mailHeading, bool displayInDropDown =true)
        {
            MailHeading = mailHeading;
            DisplayInDropDown = displayInDropDown;
        }
    }
    #endregion
}

using TrainingProjectDataLayer.Constants;
using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections;

namespace TrainingProjectDataLayer.DataLayer.Entities.DAL
{

    #region CUSTOM ATTRIBUTE
    public sealed class AtLeastOneDocumentAttribute : ValidationAttribute ,IClientValidatable
    {
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("chars", "/.,!@#$%");
            rule.ValidationType = "exclude";
            yield return rule;
        }

        public override bool IsValid(object InspectionCallWiseReferenceDocuments)
        {
            //var list = InspectionCallWiseReferenceDocuments as IList;
            //if (list != null)
            //{
            //    return list.Count > 0;
            //}



            //int cnt = 0;
            //if (InspectionCallWiseReferenceDocuments != null)
            //{
            //    foreach (InspectionCallWiseReferenceDocument item in InspectionCallWiseReferenceDocuments)
            //    {   //CHECK IF ITS IN EDIT MODE , MEANS IF DB ALREADY HAS FILE
            //        //CHECK IF NO FILE BROWSED
            //        if (item.InspectionCallWiseRefernceDocumentId == 0 && item.file == null)
            //            cnt++;
            //    }
            //}
            //if (cnt > 0) _IsAnyRefDocument

            return true;
        }
    }

    //public sealed class RemarkRequiredAttribute : ValidationAttribute, IClientValidatable
    //{
    //    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    //    {
    //        var rule = new ModelClientValidationRule()
    //        {
    //            ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
    //            ValidationType = "requiredif",
    //        };
    //        string depProp = BuildDependentPropertyId(metadata, context as ViewContext);
    //        // find the value on the control we depend on;
    //        // if it's a bool, format it javascript style 
    //        // (the default is True or False!)
    //        StringBuilder sb = new StringBuilder();
    //        foreach (var obj in this._targetValue)
    //        {
    //            string targetValue = (obj ?? "").ToString();
    //            if (obj.GetType() == typeof(bool))
    //                targetValue = targetValue.ToLower();
    //            sb.AppendFormat("|{0}", targetValue);
    //        }
    //        rule.ValidationParameters.Add("dependentproperty", depProp);
    //        rule.ValidationParameters.Add("targetvalue", sb.ToString().TrimStart('|'));
    //        yield return rule;
    //    }
    //    public override bool IsValid(object InspectionCallWiseReferenceDocuments)
    //    {
    //        return true;
    //    }
    //}


    public class remarkifAttribute : ValidationAttribute, IClientValidatable
    {
        private RequiredAttribute _innerAttribute = new RequiredAttribute();
        private string _dependentProperty;
        private object[] _targetValue;
        public remarkifAttribute(string dependentProperty, params object[] targetValue)
        {
            this._dependentProperty = dependentProperty;
            this._targetValue = targetValue;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // get a reference to the property this validation depends upon
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(this._dependentProperty);
            if (field != null)
            {
                // get the value of the dependent property
                var dependentvalue = field.GetValue(validationContext.ObjectInstance, null);
                foreach (var obj in _targetValue)
                {
                    // compare the value against the target value
                    if ((dependentvalue == null && this._targetValue == null) ||
                        (dependentvalue != null && dependentvalue.Equals(obj)))
                    {
                        // match => means we should try validating this field
                        if (!_innerAttribute.IsValid(value))
                            // validation failed - return an error
                            return new ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
                    }
                }
            }
            return ValidationResult.Success;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "remarkif",
            };
            string depProp = BuildDependentPropertyId(metadata, context as ViewContext);
            // find the value on the control we depend on;
            // if it's a bool, format it javascript style 
            // (the default is True or False!)
            StringBuilder sb = new StringBuilder();
            foreach (var obj in this._targetValue)
            {
                string targetValue = (obj ?? "").ToString();
                if (obj.GetType() == typeof(bool))
                    targetValue = targetValue.ToLower();
                sb.AppendFormat("|{0}", targetValue);
            }
            rule.ValidationParameters.Add("dependentproperty", depProp);
            rule.ValidationParameters.Add("targetvalue", sb.ToString().TrimStart('|'));
            yield return rule;
        }
        private string BuildDependentPropertyId(ModelMetadata metadata, ViewContext viewContext)
        {
            // build the ID of the property
            string depProp = viewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(this._dependentProperty);
            // unfortunately this will have the name of the current field appended to the beginning,
            // because the TemplateInfo's context has had this fieldname appended to it. Instead, we
            // want to get the context as though it was one level higher (i.e. outside the current property,
            // which is the containing object (our Person), and hence the same level as the dependent property.
            var thisField = metadata.PropertyName + "_";
            if (depProp.StartsWith(thisField))
                // strip it off again
                depProp = depProp.Substring(thisField.Length);
            return depProp;
        }
    }


#endregion


#region Users MetaData

/// <summary>
/// Metadata class for Users
/// </summary>
public class UsersMetaData
    {
        /// <summary>
        /// Property for UserId Field
        /// </summary>
        //[Required(ErrorMessage = "User ID Is Required")]
        //[Display(Name = "User ID")]
        public int UserId { get; set; }

        /// <summary>
        /// Property for UserName Field
        /// </summary>
        [Required(ErrorMessage = "User Name Is Required")]
        [Display(Name = "User Name")]
        [Remote("doesUserNameExist", "Users", HttpMethod = "POST", ErrorMessage = "User name already exists. Please enter a different user name.")]
        public string UserName { get; set; }

        /// <summary>
        /// Property for Password Field
        /// </summary>
        [Required(ErrorMessage = "Password Is Required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Password Must Be Between 1-10 Characters")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Property for First_Name Field
        /// </summary>
        [Required(ErrorMessage = "First Name Is Required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Property for First_Name Field
        /// </summary>
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Property for Last_Name Field
        /// </summary>
        [Required(ErrorMessage = "Last Name Is Required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        /// Property for Initials Field
        /// </summary>
        [Display(Name = "Initials")]
        public string Init { get; set; }

        /// <summary>
        /// Property for DeptID Field
        /// </summary>
        [Required(ErrorMessage = "Department Name Is Required")]
        [Display(Name = "Deptartment")]
        public int DeptId { get; set; }


        /// <summary>
        /// Property for EMailID Field
        /// </summary>
        
        //[RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", ErrorMessage = "Please Enter A Valid Email ID")]
        [Display(Name = "Email")]
        [Required(ErrorMessage ="Enter at least one Email Id")]
        public string EmailId { get; set; }

        /// <summary>
        /// Property for CreatedOn Field
        /// </summary>
        [Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = Constants.Constants.DateFormat, ApplyFormatInEditMode = true)]
        public System.DateTime CreatedOn { get; set; }

        /// <summary>
        /// Property for CreatedBy Field
        /// </summary>
        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// Property for Email Notifications
        /// </summary>
        [Display(Name = "Email Notifications")]
        public bool EmailNotifications { get; set; }

        /// <summary>
        /// Property for Notifications In System
        /// </summary>
        [Display(Name = "Notifications In System")]
        public bool NotificationsInSystem { get; set; }

        /// <summary>
        /// Property for LastModifiedOn Field
        /// </summary>
        [Display(Name = "LastModified On")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = Constants.Constants.DateFormat, ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> LastModifiedOn { get; set; }

        /// <summary>
        /// Property for LastModifiedBy Field
        /// </summary>
        [Display(Name = "LastModified By")]
        public Nullable<int> LastModifiedBy { get; set; }
        
    }
    #endregion

    #region Users MetaData

    /// <summary>
    /// Metadata class for Users
    /// </summary>
    public class UserLogMetaData
    {
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "User")]
        public virtual User User { get; set; }

        /// <summary>
        /// Property for Login Time
        /// </summary>
        [Display(Name = "Logged In Time")]
        ////[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = Constants.Constants.DateFormat, ApplyFormatInEditMode = true)]
        public System.DateTime LoggedInTime { get; set; }

        /// <summary>
        /// Log out time
        /// </summary>
        [Display(Name = "Logged Out Time")]
        ////[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = Constants.Constants.DateFormat, ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> LoggedOutTime { get; set; }

        /// <summary>
        /// system info
        /// </summary>
        [Display(Name = "System Information")]
        public string SystemInfo { get; set; }

        /// <summary>
        /// connection Id
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// Online Status
        /// </summary>
        public bool OnlineStatus { get; set; }

        [Display(Name = "Last Acessed Time")]
        ////[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = Constants.Constants.DateFormat, ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> LastAcessedOn { get; set; }

        /// <summary>
        /// Offline by admin
        /// </summary>
        [Display(Name = "Is Offline By ADMIN")]
        public bool IsOfflineByAdmin { get; set; }

        /// <summary>
        /// Ip address of user system
        /// </summary>
        [Display(Name = "System IP Address")]
        public string IPAddress { get; set; }
    }
    #endregion

    #region Plant Metadata

    /// <summary>
    ///  Metadata class for Plant
    /// </summary>
    public class PlantMetaData
    {
        
        [Required(ErrorMessage = "Plant Id Is Required")]
        [Display(Name = "Plant Id")]
        public int PlantId { get; set; }

        [Required(ErrorMessage = "Plant Code Is Required")]
        [Display(Name = "Plant Code")]
        public string PlantCode { get; set; }

        [Required(ErrorMessage = "Plant Name Is Required")]
        [Display(Name = "Plant Name")]
        public string PlantName { get; set; }

        [Required(ErrorMessage = "Description Is Required")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = Constants.Constants.DateFormat, ApplyFormatInEditMode = true)]
        public System.DateTime CreatedOn { get; set; }

        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }

        [Display(Name = "LastModified On")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = Constants.Constants.DateFormat, ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> LastModifiedOn { get; set; }

        [Display(Name = "LastModified By")]
        public Nullable<int> LastModifiedBy { get; set; }
    }

    #endregion

    #region Department

    /// <summary>
    ///  Metadata class for Department
    /// </summary>
    public class DepartmentMetaData
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Dept Id Is Required")]
        [Display(Name = "Dept Id")]
        public int DeptId { get; set; }

        [Required(ErrorMessage = "Dept Name Is Required")]
        [Display(Name = "Department Name")]
        public string Name { get; set; }


        public string Description { get; set; }

        [Required(ErrorMessage = "Plant Id Is Required")]
        [Display(Name = "Plant Id")]
        public int PlantId { get; set; }

        [Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = Constants.Constants.DateFormat, ApplyFormatInEditMode = true)]
        public System.DateTime CreatedOn { get; set; }

        [Required(ErrorMessage = "Created By Is Required")]
        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }

        [Display(Name = "LastModified On")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = Constants.Constants.DateFormat, ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> LastModifiedOn { get; set; }

        public Nullable<int> LastModifiedBy { get; set; }

        public virtual Plant Plant { get; set; }

    }

    #endregion

    #region Vendor List MetaData

    /// <summary>
    /// Metadata class for Vendor List
    /// </summary>
    public class VendorMeteData
    {
        /// <summary>
        /// Property for VendorListId Field
        /// </summary>
        [Display(Name = "Vendor List ID")]
        public int VendorId { get; set; }

    
        /// <summary>
        /// Property for VendorName Field
        /// </summary>
        [Required(ErrorMessage = "Vendor Name Is Required")]
        [Display(Name = "Vendor Name")]
        public string VendorName { get; set; }


        /// <summary>
        /// Property for Location Field
        /// </summary>
        [Required(ErrorMessage = "Location Is Required")]
        [Display(Name = "Location")]
        public string Location { get; set; }


        /// <summary>
        /// Property for VendorScope Field
        /// </summary>
        [Required(ErrorMessage = "Vendor Scope Is Required")]
        [Display(Name = "Vendor Scope")]
        public string VendorScope { get; set; }

        /// <summary>
        /// Property for CreatedOn Field
        /// </summary>
        [Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = Constants.Constants.DateFormat, ApplyFormatInEditMode = true)]
        public System.DateTime CreatedOn { get; set; }

        /// <summary>
        /// Property for CreatedBy Field
        /// </summary>
        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// Property for LastModifiedOn Field
        /// </summary>
        [Display(Name = "LastModified On")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = Constants.Constants.DateFormat, ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> LastModifiedOn { get; set; }

        /// <summary>
        /// Property for LastModifiedBy Field
        /// </summary>
        [Display(Name = "LastModified By")]
        public Nullable<int> LastModifiedBy { get; set; }
    }
    #endregion

    #region Right MetaData

    /// <summary>
    /// Metadata class for Rights
    /// </summary>
    public class RightMeteData
    {
        /// <summary>
        /// Property for Right ID Field
        /// </summary>
        [Display(Name = "Right ID")]
        public int RightId { get; set; }

        /// <summary>
        /// Property for Right Name Field
        /// </summary>
        //[Required(ErrorMessage ="Right Name Is Required")]
        [Display(Name = "Right Name")]
        public string RightName { get; set; }

        /// <summary>
        /// Property for Description Field
        /// </summary>
        public string Description { get; set; }
    }
    #endregion

    #region Role MetaData

    /// <summary>
    /// Metadata class for Role
    /// </summary>
    public class RoleMeteData
    {
        /// <summary>
        /// Property for Role ID Field
        /// </summary>
        [Display(Name = "Role ID")]
        public int RoleId { get; set; }

        /// <summary>
        /// Property for Role Name Field
        /// </summary>
        [Required(ErrorMessage = "Role Name Is Required")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        /// <summary>
        /// Property for Role Description Field
        /// </summary>
        [Display(Name = "Role Description")]
        public string RoleDescription { get; set; }

        /// <summary>
        /// Property for CreatedOn Field
        /// </summary>
        [Display(Name = "Created On")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = Constants.Constants.DateFormat, ApplyFormatInEditMode = true)]
        public System.DateTime CreatedOn { get; set; }

        /// <summary>
        /// Property for CreatedBy Field
        /// </summary>
        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }

        
    }
    #endregion

    #region Report Type Meta Data
    /// <summary>
    /// Metadata class for Report Type
    /// </summary>
    public class ReportsTypeMetaData
    {
        [Required(ErrorMessage = "Display Name Is Required")]
        [Display(Name = "Display Name")]
        public string ReportName { get; set; }
       
        [Display(Name = "Report Tittle Name")]
        public string ReportTittleName { get; set; }

        [Required(ErrorMessage = "Please Select Report Type")]
        [Display(Name = "Report Type")]
        public int ReportDesignType { get; set; }

        //[RequiredIf("ReportDesignType",1, ErrorMessage = "Please Enter Report Header")]
        [Display(Name = "Report Header")]
        public string ReportHeader { get; set; }

        /// <summary>
        /// View name for data Table in Report
        /// </summary>
        [Required(ErrorMessage = "Report Table Name Is Required")]
        [Display(Name = "Report Table Name")]
        public string ReportTableView { get; set; }


        /// <summary>
        /// View Name for detail data above table in Report
        /// </summary>
        [Display(Name = "Report Detail Name")]
        public string ReportDetailView { get; set; }


    }
    #endregion

}

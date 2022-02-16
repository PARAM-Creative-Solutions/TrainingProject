using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TrainingProjectDataLayer.Constants;
using TrainingProjectDataLayer.DataLayer.Unit_of_Work.Implementation;
using System.Web.Mvc;

namespace TrainingProjectDataLayer.DataLayer.Entities.DAL
{
    #region Users

    /// <summary>
    /// Partial class for Users
    /// </summary>
    [MetadataType(typeof(UsersMetaData))]
    public partial class User
    {
        public List<int> SelectedRoles { get; set; }

        /// <summary>
        /// gets or sets the vendor list id if department is selected as Vendor
        /// </summary>
        [Display(Name = "Vendor List")]
        [NotMapped]
        //[RequiredIf("DeptId", 14,  ErrorMessage =("Select Vendor"))]
        public int VendorListId { get; set; }

        /// <summary>
        /// Property for RoleId Field
        /// </summary>
        [Required(ErrorMessage = "Role Is Required")]
        [Display(Name = "Role")]
        [NotMapped]
        public int RoleID { get; set; }

        /// <summary>
        /// Property for HttppostedBased File Field
        /// </summary>
        [Display(Name = "Picture File")]
        [NotMapped]
        public HttpPostedFileBase httpFile { get; set; }

        /// <summary>
        /// Property for HttppostedBased File Field
        /// </summary>
        [Display(Name = "Cropped File")]
        [NotMapped]
        public string CroppedFile { get; set; }
    }

    [MetadataType(typeof(UserLogMetaData))]
    public partial class UserLog
    {

    }

    #endregion

    #region Plant

    /// <summary>
    /// Partial class for Material
    /// </summary>
    [MetadataType(typeof(PlantMetaData))]
    public partial class Plant
    {

    }

    #endregion

    #region Department

    /// <summary>
    /// Partial class for Material
    /// </summary>
    [MetadataType(typeof(DepartmentMetaData))]
    public partial class Department
    {

    }

    #endregion.


    #region Vendor List

    /// <summary>
    /// Partial class for VendorList
    /// </summary>
    [MetadataType(typeof(VendorMeteData))]
    public partial class Vendor
    {

    }

    #endregion

    #region Right

    /// <summary>
    /// Partial class for Right
    /// </summary>
    [MetadataType(typeof(RightMeteData))]
    public partial class Right : Attribute
    {
        [NotMapped]
        public int Status { get; set; }

        public Right()
        {

        }
        public Right(string RightName)
        {
            this.RightName = RightName;
            this.Description = Description;
            //this.Status = Status;
        }
    }

    #endregion

    #region Role

    /// <summary>
    /// Partial class for Role
    /// </summary>
    [MetadataType(typeof(RoleMeteData))]
    public partial class Role
    {
        /// <summary>
        /// Property for Right ID Field
        /// </summary>
        [Display(Name = "Right ID")]
        public int RightId { get; set; }

        [Display(Name = "User Rights")]
        public List<int> SelectedRights { get; set; }

    }

    #endregion

    #region Report Type
    /// <summary>
    /// Partial class for Report Type
    /// </summary>
    [MetadataType(typeof(ReportsTypeMetaData))]
    public partial class ReportsType
    {


    }
    #endregion



}

using TrainingProjectDataLayer.Constants;
using TrainingProjectDataLayer.DataLayer.Entities.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;


namespace TrainingProject.Security
{
    #region Class CustomPrincipal

    interface ICustomPrincipal : IPrincipal
    {
        #region Property
        /// <summary>
        /// Property for UserId Field
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Property for PlantId Field
        /// </summary>
        int PlantId { get; set; }

        /// <summary>
        /// Property for PlantId Field
        /// </summary>
        int DepartmentId { get; set; }

        /// <summary>
        /// Property for FullUsername field
        /// </summary>
        string FullName { get; set; }

        /// <summary>
        /// Property for roles field
        /// </summary>
        List<int> Rights { get; set; }

        string SessionId { get; set; }

        #endregion
    }

    /// <summary>
    /// Class for implementing all methods of IPrncipal interface
    /// </summary>
    public class CustomPrincipal : ICustomPrincipal
    {
        #region constructors
        /// <summary>
        /// Deafult constructor
        /// </summary>
        public CustomPrincipal()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        public CustomPrincipal(GenericIdentity identity)
        {
            Identity = identity;
        }

        /// <summary>
        /// Initializes Identity
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Id"></param>
        /// <param name="PlantId"></param>
        /// <param name="DepartmentId"></param>
        /// <param name="FullName"></param>
        /// <param name="Rights"></param>
        public CustomPrincipal(string Username,int Id,int PlantId,int DepartmentId,string FullName, List<int> Rights,string SessionId)
        {
            this.Identity = new GenericIdentity(Username);
            this.Id = Id;
            this.PlantId = PlantId;
            this.DepartmentId = DepartmentId;
            this.FullName = FullName;
            this.Rights = Rights;
            this.SessionId = SessionId;
        }

        #endregion

        #region Property
        /// <summary>
        /// Identity
        /// </summary>
        public IIdentity Identity
        {
            get; private set;
        }
        /// <summary>
        /// Property for UserId Field
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Property for PlantId Field
        /// </summary>
        public int PlantId { get; set; }

        /// <summary>
        /// Property for PlantId Field
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Property for FullUsername field
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Property for roles field
        /// </summary>
        public List<int> Rights { get; set; }

        public string SessionId { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Method of type bool,checking roles
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public bool IsInRole(string right)
        {
            return Rights.Contains(Convert.ToInt32(right));
        }

        #region IsWithRight
        /// <summary>
        /// To Validate user with given right
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public bool IsWithRight(SystemEnums.Rights right)
        {
            int EnumNumber = (int)right;
            var user = HttpContext.Current.User as Security.CustomPrincipal;
            if (user != null)
                return user.Rights.Contains(EnumNumber);
            else
                return false;
        }
        #endregion


        #endregion


    }

    #endregion
}
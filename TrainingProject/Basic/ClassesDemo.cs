using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingProject.Basic
{

    public class Organization
    {
        public List<User> Users { get; set; }
        public List<Department> Departments { get; set; }
        public List<Role> Roles { get; set; }

        public  List<User> GetUsers()
        {
            return Users;
        }
        public List<Department> GetDepartments()
        {
            return Departments;
        }

        public List<Role> GetRoles()
        {
            return Roles;
        }
    }

    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string  LastName { get; set; }
        public Address Permanent { get; set; }
        public Address Office { get; set; }
        public Department department { get; set; }
        public List<Role> Roles { get; set; }
        public bool AddIntoDB()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }  
            finally
            {

            }
        }
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    }

    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }

    public class Address
    {
        public string City { get; set; }
        public int PinCode { get; set; }
    }
}
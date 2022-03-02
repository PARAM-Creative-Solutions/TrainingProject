using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingProject.Basic
{

    class Organaization
    {
        public List<Employee> Employees { get; set; }
        public Organaization()
        {
            Employees = new List<Employee>();
        }
        public Organaization(List<Employee> Employees)
        {
            Employee emp1 = new Employee();
            Employee emp2 = new Employee(10);
            Employee emp3 = new Employee("Pooja");
            Employee emp4 = new Employee(10,"Rishi");
        }
    }

    //protected //private //public 

    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public Employee()
        {

        }

        public Employee(int EmployeeId, string EmployeeName)
        {
            this.EmployeeId = EmployeeId;
            this.EmployeeName = EmployeeName;
        }

        public Employee(int EmployeeId)
        {
            this.EmployeeId = EmployeeId;
        }

        public Employee(string EmployeeName)
        {
            this.EmployeeName = EmployeeName;
        }
    }
}
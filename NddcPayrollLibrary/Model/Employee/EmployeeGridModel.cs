using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Employee
{
    public class EmployeeGridModel
    {
        public int SrNo { get; set; }
        public int Id { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        private string myFullName;

        public string FullName
        {
            get 
            {
                myFullName = $"{FirstName} {LastName} {OtherNames}";
                return myFullName; 
            }
           
        }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string GradeLevel { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool Archived { get; set; }
        public string ExitCondition { get; set; }
        public DateTime DateEngaged { get; set; }
        public DateTime ContExpiringDate { get; set; }
        public DateTime ExitDate { get; set; }
    }
}

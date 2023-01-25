using NddcPayrollLibrary.Model.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Employee
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string MaidenName { get; set; }
        public string SpouseName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int SID { get; set; }
        public MyStatesModel State { get; set; }
        public string Passport { get; set; }
        public string EmploymentStatus { get; set; }
        public DateTime DateEngaged { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }

    }
}

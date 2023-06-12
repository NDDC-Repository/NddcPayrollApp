using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Report
{
    public class PayrollListingModel2
    {
        [DisplayNameAttribute("S/N")]
        public int SrNo { get; set; }
        [DisplayNameAttribute("Department")]
        public string DepartmentName { get; set; }
        public string Category { get; set; }
        [DisplayNameAttribute("Job Grade")]
        public string GradeLevel { get; set; }
        [DisplayNameAttribute("Employee Code")]
        public string EmployeeCode { get; set; }
        [DisplayNameAttribute("Surname         ")]
        public string LastName { get; set; }
        [DisplayNameAttribute("Full Names            ")]
        public string FirstName { get; set; }

        [DisplayNameAttribute("ED-Basic Salary  ")]
        public decimal BasicSalary { get; set; }
        
        [DisplayNameAttribute("Total Earnings")]
        public decimal TotalEarnings { get; set; }
        [DisplayNameAttribute("Total Deductions")]
        public decimal TotalDeductions { get; set; }
        [DisplayNameAttribute("Net Pay          ")]
        public decimal NetPay { get; set; }
        [DisplayNameAttribute("Code")]
        public string BankCode { get; set; }
        [DisplayNameAttribute("Account Number")]
        public string AccountNumber { get; set; }
        [DisplayNameAttribute("Bank Name                   ")]
        public string BankName { get; set; }
    }
}

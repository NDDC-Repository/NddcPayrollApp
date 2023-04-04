using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.DataManagement.DataMigration
{
    public class MyEmployeeMigrationModel
    {
        public int Id { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeCode { get; set; }
        public string Surname { get; set; }
        public string OtherNames { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string JobTitle { get; set; }
        public string JobGrade { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateEngaged { get; set; }
        public string Category { get; set; }
        public string Department { get; set; }
        public string DepartmentId { get; set; }
        public string JobTitleCode { get; set; }
        public string JobTitleId { get; set; }
        public string TaxOffice { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string State { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string BankAccountNo { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string PensionCode { get; set; }
        public string PensionFundNumber { get; set; }
        public int PensionFundId { get; set; }
        public DateTime TaxYearStart { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public string TaxStateProvince { get; set; }
        public int GradeLevelId { get; set; }
        public decimal CoopAmt { get; set; }
    }
}

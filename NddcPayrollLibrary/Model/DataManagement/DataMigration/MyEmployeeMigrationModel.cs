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
        public string JobTitle { get; set; }
        public string JobGrade { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateOnly DateEngaged { get; set; }
        public string Category { get; set; }
        public string Department { get; set; }
        public string JobTitleCode { get; set; }
        public string TaxOffice { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string State { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string BankAccountNo { get; set; }
        public string AccountNumber { get; set; }
        public string PensionCode { get; set; }
        public string PensionNumber { get; set; }
        public DateOnly TaxYearStart { get; set; }
    }
}

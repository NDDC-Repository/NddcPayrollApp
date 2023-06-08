using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Report
{
    public class MyPayRollListModel
    {
        [DisplayNameAttribute("S/N")]
        public int SrNo { get; set; }
        [Bindable(false)]
        public int Id { get; set; }
        [DisplayNameAttribute("Department")]
        public string DepartmentName { get; set; }
        public string Category { get; set; }
        [DisplayNameAttribute("Job Grade")]
        public string GradeLevel { get; set; }
        [DisplayNameAttribute("Employee Code")]
        public string EmployeeCode { get; set; }
        [DisplayNameAttribute("Last Name")]
        public string LastName { get; set; }
        [DisplayNameAttribute("First Name")]
        public string FirstName { get; set; }
        [DisplayNameAttribute("Basic Salary")]
        public decimal BasicSalary { get; set; }
        public decimal TransportAllowance { get; set; }
        public decimal HousingAllowance { get; set; }
        public decimal FurnitureAllowance { get; set; }
        public decimal MealAllowance { get; set; }
        public decimal UtilityAllowance { get; set; }
        public decimal EducationAllowance { get; set; }
        public decimal SecurityAllowance { get; set; }
        public decimal DomesticServantAllowance { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal DriversAllowance { get; set; }
        public decimal VehicleMaintenanceAllowance { get; set; }
        public decimal HazardAllowance { get; set; }
        public decimal TaxDeduction { get; set; }
        public decimal NHFDeduction { get; set; }
        public decimal PensionDeduction { get; set; }
        public decimal JSADeduction { get; set; }
        public decimal SSADeduction { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetPay { get; set; }
        public string BankCode { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public decimal SecretarialAllow { get; set; }
        public decimal CooperativeDed { get; set; }
        public decimal VoluntaryPension { get; set; }
        public decimal EntertainmentAllow { get; set; }
        public decimal NewspaperAllow { get; set; }
        public decimal Arreas { get; set; }
        public decimal LeaveAllow { get; set; } 

    }
}

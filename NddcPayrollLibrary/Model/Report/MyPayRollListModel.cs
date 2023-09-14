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
        [DisplayName("Gender")]
        public string Gender { get; set; }
        [DisplayNameAttribute("ED-Basic Salary  ")]
        public decimal BasicSalary { get; set; }
        [DisplayNameAttribute("ED-Salary Arreas  ")]
        public decimal Arreas { get; set; }
        [DisplayNameAttribute("ED-Leave Allowance  ")]
        public decimal LeaveAllow { get; set; }
        [Bindable(false)]
        public int Id { get; set; }
        [DisplayNameAttribute("ED-Transport Allow")]
        public decimal TransportAllowance { get; set; }
        [DisplayNameAttribute("ED-Housing Allow")]
        public decimal HousingAllowance { get; set; }
        [DisplayNameAttribute("ED-Furniture Allow")]
        public decimal FurnitureAllowance { get; set; }
        [DisplayNameAttribute("ED-Meal Allow")]
        public decimal MealAllowance { get; set; }
        [DisplayNameAttribute("ED-Utility Allow")]
        public decimal UtilityAllowance { get; set; }
        [DisplayNameAttribute("ED-Education Allow")]
        public decimal EducationAllowance { get; set; }
        [DisplayNameAttribute("ED-Security Allow")]
        public decimal SecurityAllowance { get; set; }
        [DisplayNameAttribute("ED-Domestic Servan")]
        public decimal DomesticServantAllowance { get; set; }
        [DisplayNameAttribute("ED-Medical Allow")]
        public decimal MedicalAllowance { get; set; }
        [DisplayNameAttribute("ED-Driver's Allow")]
        public decimal DriversAllowance { get; set; }
        [DisplayNameAttribute("ED-Vehicle Mainten")]
        public decimal VehicleMaintenanceAllowance { get; set; }
        [DisplayNameAttribute("ED-Hazard Allow")]
        public decimal HazardAllowance { get; set; }
        [DisplayNameAttribute("ED-Entertainment")]
        public decimal EntertainmentAllow { get; set; }
        [DisplayNameAttribute("ED-Newspaper")]
        public decimal NewspaperAllow { get; set; }
        [DisplayNameAttribute("ED-Secretarial Alw")]
        public decimal SecretarialAllow { get; set; }
        [DisplayNameAttribute("DD-Tax           ")]
        public decimal TaxDeduction { get; set; }
        [DisplayNameAttribute("DD-Nat. HousingFund")]
        public decimal NHFDeduction { get; set; }
        [DisplayNameAttribute("DD-Pension      ")]
        public decimal PensionDeduction { get; set; }
        [DisplayNameAttribute("DD-Cooperative")]
        public decimal CooperativeDed { get; set; }
        [DisplayNameAttribute("DD-Voluntary Pens")]
        public decimal VoluntaryPension { get; set; }
        [DisplayNameAttribute("DD-NDDC AUPTRE JSA")]
        public decimal JSADeduction { get; set; }
        [DisplayNameAttribute("DD-NDDC AUPTRE SSA")]
        public decimal SSADeduction { get; set; }
        [DisplayNameAttribute("Total Earnings")]
        public decimal TotalEarnings { get; set; }
        [DisplayNameAttribute("Total Deductions")]
        public decimal TotalDeductions { get; set; }
        [DisplayNameAttribute("Net Pay          ")]
        public decimal NetPay { get; set; }
        [Bindable(false)]
        public string BankCode { get; set; }
        [Bindable(false)]
        public string AccountNumber { get; set; }
        [Bindable(false)]
        public string BankName { get; set; }
        
       
      
        
       

    }
}

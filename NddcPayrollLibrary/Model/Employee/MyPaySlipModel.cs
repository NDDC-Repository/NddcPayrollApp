using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Employee
{
    public class MyPaySlipModel
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PayPoint { get; set; }
        public string DepartmentName { get; set; }
        public string JobTitle { get; set; }
        public string JobGrade { get; set; }
        public string DateOfPayment { get; set; }
        public DateTime DateEngaged { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal TransportAllow { get; set; }
        public decimal MealAllow { get; set; }
        public decimal UtilityAllow { get; set; }
        public decimal EducationAllow { get; set; }
        public decimal MedicalAllow { get; set; }
        public decimal VehicleMaintenanceAllow { get; set; }
        public decimal HazardAllow { get; set; }
        public decimal HousingAllow { get; set; }
        public decimal FurnitureAllow { get; set; }
        public decimal SecurityAllow { get; set; }
        public decimal DomesticServantAllow { get; set; }
        public decimal DriverAllow { get; set; }

        public decimal Tax { get; set; }
        public decimal NHF { get; set; }
        public decimal Pension { get; set; }
        public decimal CooperativeDed { get; set; }
        public decimal SSA { get; set; }
        public decimal JSA { get; set; }
        public decimal VoluntaryPension { get; set; }
        public decimal Insurance { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetPay { get; set; }
        public decimal EntertainmentAllow { get; set; }
        public decimal NewspaperAllow { get; set; }
    }
}

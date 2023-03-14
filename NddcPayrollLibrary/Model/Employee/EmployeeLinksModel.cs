using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Employee
{
    public class EmployeeLinksModel
    {
        public int Id { get; set; }
        public int GradeLevelId { get; set; }
        public int JobTitleId { get; set; }
        public string Category { get; set; }
        public int DepartmentId { get; set; }
        public string PayPoint { get; set; }
        public decimal Insurance { get; set; }
        public decimal SecretarialAllow { get; set; }
        public decimal CooperativeDed { get; set; }
        public decimal VoluntaryPension { get; set; }
        public decimal TransportAllow { get; set; }
        public decimal HousingAllow { get; set; }
        public decimal FurnitureAllow { get; set; }
        public decimal MealAllow { get; set; }
        public decimal UtilityAllow { get; set; }
        public decimal EducationAllow { get; set; }
        public decimal SecurityAllow { get; set; }
        public decimal DomesticServantAllow { get; set; }
        public decimal MedicalAllow { get; set; }
        public decimal DriverAllow { get; set; }
        public decimal VehicleAllow { get; set; }
        public decimal HazardAllow { get; set; }
        public decimal Tax { get; set; }
        public decimal NHF { get; set; }
        public decimal Pension { get; set; }
        public decimal JSA { get; set; }
        public decimal SSA { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetPay { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal MonthlyGross { get; set; }
        public decimal LeaveAllow { get; set; }
        public decimal ActingAllow { get; set; }
        public decimal ShiftAllow { get; set; }
        public decimal UniformAllow { get; set; }
        public string TaxCalc { get; set; }
        public decimal Arreas { get; set; }
        public decimal EmployerPension { get; set; }
        public decimal TaxAdjustment { get; set; }
        public decimal EntertainmentAllow { get; set; }
        public decimal NewspaperAllow { get; set; }
    }
}

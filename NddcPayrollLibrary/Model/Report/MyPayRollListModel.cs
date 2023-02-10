using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Model.Report
{
    public class MyPayRollListModel
    {
        public int SrNo { get; set; }
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string Category { get; set; }
        public string GradeLevel { get; set; }
        public string EmployeeCode { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
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
    }
}

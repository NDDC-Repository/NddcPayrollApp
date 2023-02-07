using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Calculations.Allowance;
using NddcPayrollLibrary.Data.Calculations.Deductions;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.Employee;
using System.ComponentModel.DataAnnotations;

namespace NddcPayroll.Web.Pages.Employee
{
    public class EmployeeDetailsModel : PageModel
    {
        private readonly ICompanyData db;
        private readonly IEmployeeData empDb;
        private readonly IPayrollData payDb;
        private readonly IAllowanceData allowDb;
        private readonly IDeductionData dedDb;

        public decimal BasicSalary { get; set; }
    
        public decimal MonthlyGross { get; set; }

        public EmployeeModel Employee { get; set; }
        public List<MyStatesModel> States { get; set; }
    
        public decimal MySalaryBenefits { get; set; }
        public decimal HousingAllowance { get; set; }
        public decimal FurnitureAllowance { get; set; }
        public decimal TransportAllowance { get; set; }
        public decimal MealAllowance { get; set; }
        public decimal UtilityAllowance { get; set; }
        public decimal EducationAllowance { get; set; }
        public decimal SecurityAllowance { get; set; }
        public decimal DomesticServantAllowance { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal DriverAllowance { get; set; }
        public decimal VehicleMaintenance { get; set; }
        public decimal Hazard { get; set; }
        public decimal PayeTax { get; set; }

        public EmployeeDetailsModel(ICompanyData db, IEmployeeData EmpDb, IPayrollData PayDb, IAllowanceData allowDb, IDeductionData dedDb)
        {
            this.db = db;
            empDb = EmpDb;
            payDb = PayDb;
            this.allowDb = allowDb;
            this.dedDb = dedDb;
        }
        public void OnGet(int? EmpId)
        {
            States = db.GetAllStates();
            Employee = empDb.GetEmployeeDetails(EmpId.Value);
            BasicSalary = payDb.GetBasicSalary(EmpId.Value);
            MySalaryBenefits = payDb.GetBenefits(EmpId.Value);
            MonthlyGross = payDb.GetMonthlyGross(EmpId.Value);
            HousingAllowance = allowDb.GetHousingAllowance(EmpId.Value);
            PayeTax = dedDb.GetPAYEAmount(EmpId.Value);
            FurnitureAllowance = allowDb.GetFurnitureAllowance(EmpId.Value);
            TransportAllowance = allowDb.GetTransportAllowance(EmpId.Value);
            MealAllowance = allowDb.GetMealAllowance(EmpId.Value);
            UtilityAllowance = allowDb.GetUtilityAllowance(EmpId.Value);
            EducationAllowance = allowDb.GetEducationAllowance(EmpId.Value);
            SecurityAllowance = allowDb.GetSecurityAllowance(EmpId.Value);
            DomesticServantAllowance = allowDb.GetDomesticServantAllowance(EmpId.Value);    
            MedicalAllowance = allowDb.GetMedicalAllowance(EmpId.Value);
            DriverAllowance = allowDb.GetDriverAllowance(EmpId.Value);
            VehicleMaintenance = allowDb.GetVehicleMaintenanceAllowance(EmpId.Value);
            Hazard = allowDb.GetHazardAllowance(EmpId.Value);
        }
    }
}

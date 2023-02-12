using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Calculations.Allowance;
using NddcPayrollLibrary.Data.Calculations.Deductions;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Data.Helper;
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
        private readonly IHelperData helperDb;

        public decimal BasicSalary { get; set; }
    
        public decimal MonthlyGross { get; set; }

        public EmployeeModel Employee { get; set; }
        public MyStatutoryDetailsModel StatutoryDetails { get; set; }
        public List<MyStatesModel> States { get; set; }
        public List<MyPayPointModel> PayPoints { get; set; }
        public List<MyPensionFundListModel> PensionAdmins { get; set; }
        [BindProperty]
        public int Age { get; set; }
        public List<BankModel> Banks { get; set; }
        public MyPaymentDetailsModel PaymentDetails { get; set; }

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
        public decimal Pension { get; set; }
        public decimal NHF { get; set; }
        public decimal JSA { get; set; }
        public decimal SSA { get; set; }
        public decimal TotalDeductions { get; set; }

        public EmployeeDetailsModel(ICompanyData db, IEmployeeData EmpDb, IPayrollData PayDb, IAllowanceData allowDb, IDeductionData dedDb, IHelperData helperDb)
        {
            this.db = db;
            empDb = EmpDb;
            payDb = PayDb;
            this.allowDb = allowDb;
            this.dedDb = dedDb;
            this.helperDb = helperDb;
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

            Pension = dedDb.GetPensionAmount(EmpId.Value);
            NHF = dedDb.GetNHFAmount(EmpId.Value);
            JSA = dedDb.GetJSA(EmpId.Value);
            SSA = dedDb.GetSSA(EmpId.Value);
            TotalDeductions = dedDb.GetTotalDeductions(EmpId.Value);

            PayPoints = db.GetAllPayPoints();
            PensionAdmins = db.GetAllPensionAdminsList();

            DateTime dob = helperDb.GetAnyRecord<DateTime, int>("Employees", "DateOfBirth", "Id", EmpId.Value);

            Age = DateTime.Now.Year - dob.Year;
            StatutoryDetails = empDb.GetStatutoryDetails(EmpId.Value);

            Banks = db.GetAllBanks();
            PaymentDetails = db.GetPaymentDetails(EmpId.Value);
        }
    }
}

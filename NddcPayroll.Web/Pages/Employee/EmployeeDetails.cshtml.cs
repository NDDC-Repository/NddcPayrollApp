using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

   
        public double BasicSalary { get; set; }
    
        public double MonthlyGross { get; set; }

        public EmployeeModel Employee { get; set; }
        public List<MyStatesModel> States { get; set; }
    
        public double MySalaryBenefits { get; set; }

        public EmployeeDetailsModel(ICompanyData db, IEmployeeData EmpDb, IPayrollData PayDb)
        {
            this.db = db;
            empDb = EmpDb;
            payDb = PayDb;
        }
        public void OnGet(int? EmpId)
        {
            States = db.GetAllStates();
            Employee = empDb.GetEmployeeDetails(EmpId.Value);
            BasicSalary = payDb.GetBasicSalary(EmpId.Value);
            MySalaryBenefits = payDb.GetBenefits(EmpId.Value);
            MonthlyGross = payDb.GetMonthlyGross(EmpId.Value);
        }
    }
}

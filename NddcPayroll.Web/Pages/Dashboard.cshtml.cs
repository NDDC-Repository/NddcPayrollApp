using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Model.Employee;

namespace NddcPayroll.Web.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IEmployeeData db;
        private readonly IPayrollData payDb;

        public List<EmployeeGridModel> Employees { get; set; }
        public List<EmployeeGridModel> ContractEmployees { get; set; }
        public int EmpCount { get; set; }
        public string HeaderDate { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal NetPay { get; set; }
        public decimal Tax { get; set; }
        public decimal Cooperative { get; set; }
        public decimal BasicSalary { get; set; }
        public int EmpAddedThisMonth { get; set; }
        public int EmpExitedThisMonth { get; set; }

        public DashboardModel(IEmployeeData db, IPayrollData payDb)
        {
            this.db = db;
            this.payDb = payDb;
        }

        DateTime currDate = DateTime.Now;
        public void OnGet(string searchItem)
        {
            Employees = db.GetAllEmployeesAddedThisMonth();
            ContractEmployees = db.GetExpiringContract();
            EmpCount = db.GetEmployeeCount();
            HeaderDate = currDate.ToString("Y");
            TotalEarnings = payDb.GetSumOfTotalEarnings();
            NetPay = payDb.GetSumOfNetPay();
            Tax = payDb.GetSumOfTax();
            Cooperative = payDb.GetSumOfCooperative();
            BasicSalary = payDb.GetSumOfBasicSalary();
            EmpAddedThisMonth = db.CountEmployeesAddedThisMonth();
            EmpExitedThisMonth = db.CountEmployeesExitedThisMonth();
        }
    }
}

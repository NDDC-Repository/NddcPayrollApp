using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Model.Employee;

namespace NddcPayroll.Web.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IEmployeeData db;
        public List<EmployeeGridModel> Employees { get; set; }

        public DashboardModel(IEmployeeData db)
        {
            this.db = db;
        }
        public void OnGet(string searchItem)
        {
            Employees = db.GetAllEmployees(searchItem);
        }
    }
}

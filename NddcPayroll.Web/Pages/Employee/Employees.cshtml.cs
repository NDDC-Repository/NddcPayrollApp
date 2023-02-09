using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Model.Employee;

namespace NddcPayroll.Web.Pages.Employee
{
    public class EmployeesModel : PageModel
    {
        private readonly IEmployeeData db;
        public List<EmployeeGridModel> Employees { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public EmployeesModel(IEmployeeData db)
        {
            this.db = db;
        }
        public void OnGet()
        {
           Employees = db.GetAllEmployees(SearchTerm);
        }
    }
}

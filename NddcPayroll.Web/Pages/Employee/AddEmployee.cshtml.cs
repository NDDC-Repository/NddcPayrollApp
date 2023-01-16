using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.Employee;

namespace NddcPayroll.Web.Pages.Employee
{
    public class AddEmployeeModel : PageModel
    {
        private readonly ICompanyData db;
        private readonly IEmployeeData dbEmp;

        public IEnumerable<MyStatesModel> States { get; set; }
        [BindProperty]
        public EmployeeModel Employee { get; set; }
       

        public AddEmployeeModel(ICompanyData db, IEmployeeData dbEmp)
        {
            this.db = db;
            this.dbEmp = dbEmp;
        }
        public void OnGet()
        {
            States = db.GetAllStates();
        }

        public IActionResult OnPost()
        {
            dbEmp.AddEmployee(Employee);
            return RedirectToPage("/Dashboard");
        }
    }
}

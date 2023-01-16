using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Model.Company;

namespace NddcPayroll.Web.Pages.Company
{
    public class AddDepartmentModel : PageModel
    {
        private readonly ICompanyData db;
        [BindProperty]
        public DepartmentModel Department { get; set; }

        public AddDepartmentModel(ICompanyData db)
        {
            this.db = db;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            db.AddDepartment(Department);
            return RedirectToPage("Departments");
        }
    }
}

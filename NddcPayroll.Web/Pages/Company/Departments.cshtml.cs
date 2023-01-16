using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Model.Company;

namespace NddcPayroll.Web.Pages.Company
{
    public class DepartmentsModel : PageModel
    {
        private readonly ICompanyData db;
        public DepartmentModel Department { get; set; }
        public List<DepartmentModel> Departments { get; set; }

        public DepartmentsModel(ICompanyData db)
        {
            this.db = db;
        }
        public void OnGet()
        {
            Departments = db.GetAllDepartments();
        }
    }
}

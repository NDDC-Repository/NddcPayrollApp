using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class BenefitsModel : PageModel
    {
        private readonly ICompanyData db;

        public List<MyGradeLevelGridModel> GradeLevels { get; set; }
        public MyGradeLevelGridModel GradeLevel { get; set; }


        public BenefitsModel(ICompanyData db)
        {
            this.db = db;
        }
        public void OnGet()
        {
            GradeLevels = db.GetAllGradeLevels();

        }
    }
}

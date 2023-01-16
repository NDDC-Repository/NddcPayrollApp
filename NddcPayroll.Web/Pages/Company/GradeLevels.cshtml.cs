using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Model.Company;

namespace NddcPayroll.Web.Pages.Company
{
    public class GradeLevelsModel : PageModel
    {
        private readonly ICompanyData db;

        
        public List<MyGradeLevelGridModel> GradeLevels { get; set; }
        public MyGradeLevelGridModel GradeLevel { get; set; }

        public GradeLevelsModel(ICompanyData db)
        {
            this.db = db;
        }
        public void OnGet()
        {
            GradeLevels = db.GetAllGradeLevels();
        }

        
    }
}

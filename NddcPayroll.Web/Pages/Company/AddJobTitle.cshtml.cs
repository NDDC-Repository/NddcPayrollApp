using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Model.Company;

namespace NddcPayroll.Web.Pages.Company
{
    public class AddJobTitleModel : PageModel
    {
        private readonly ICompanyData db;
       
        [BindProperty]
        public JobTitleModel JobTitle { get; set; }

        public AddJobTitleModel(ICompanyData db)
        {
            this.db = db;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            db.AddJobTitle(JobTitle);
            return RedirectToPage("JobTitles");
        }
    }
}

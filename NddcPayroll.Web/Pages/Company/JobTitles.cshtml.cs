using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Model.Company;

namespace NddcPayroll.Web.Pages.Company
{
    public class JobTitlesModel : PageModel
    {
        private readonly ICompanyData db;
        public List<JobTitleModel> JobTitles { get; set; }
        public JobTitlesModel JobTitle { get; set; }

        public JobTitlesModel(ICompanyData db)
        {
            this.db = db;
        }
        public void OnGet()
        {
           JobTitles = db.GetAllJobTitles();
        }
    }
}

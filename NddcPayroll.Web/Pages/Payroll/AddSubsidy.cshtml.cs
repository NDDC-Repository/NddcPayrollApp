using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class AddSubsidyModel : PageModel
    {
        private readonly IPayrollData db;

        [BindProperty]
        public MySubsidiesModel Subsidy { get; set; }

        public AddSubsidyModel(IPayrollData db)
        {
            this.db = db;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost(int? gradeLevelID)
        {
            Subsidy.GradeLevelId = gradeLevelID.Value;
            db.AddSubsidies(Subsidy);

            return RedirectToPage("ViewSubsidies", new { gradeLevelID = gradeLevelID.Value });
        }
    }
}

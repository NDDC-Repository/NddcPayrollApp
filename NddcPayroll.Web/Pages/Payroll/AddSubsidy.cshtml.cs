using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class AddSubsidyModel : PageModel
    {
        private readonly IPayrollData db;
        private readonly IReportsData repDb;

        [BindProperty]
        public MySubsidiesModel Subsidy { get; set; }

        public AddSubsidyModel(IPayrollData db, IReportsData repDb)
        {
            this.db = db;
            this.repDb = repDb;
        }
        public void OnGet(int? gradeLevelId)
        {
            if (gradeLevelId.HasValue == true)
            {
                Subsidy = db.GetSubsidyBySubsId(gradeLevelId.Value);
            }
            else
            {
                Subsidy = new MySubsidiesModel();
            }
        }

        public IActionResult OnPost(int? gradeLevelID)
        {
            if (gradeLevelID > 0)
            {
                db.UpdateSubsidy(Subsidy);
                repDb.UpdateEmployeesPayrollByGradeLevelAsync(Subsidy.GradeLevelId);
            }
            else
            {
                Subsidy.GradeLevelId = gradeLevelID.Value;
                db.AddSubsidies(Subsidy);
                repDb.UpdateEmployeesPayrollByGradeLevelAsync(gradeLevelID.Value);
            }
           
            return RedirectToPage("ViewSubsidies", new { gradeLevelID = gradeLevelID.Value });
        }
    }
}

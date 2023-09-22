using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.Enums;

namespace NddcPayroll.Web.Pages.Company
{
    public class AddGradeLevelModel : PageModel
    {
        private readonly ICompanyData db;
        private readonly IHtmlHelper htmlHelper;
        private readonly IReportsData repDb;

        [BindProperty]
        public MyGradeLevelModel GradeLevel { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        public AddGradeLevelModel(ICompanyData db, IHtmlHelper htmlHelper, IReportsData repDb)
        {
            this.db = db;
            this.htmlHelper = htmlHelper;
            this.repDb = repDb;
        }
        public void OnGet(int? gradeLevelId)
        {
            Categories = htmlHelper.GetEnumSelectList<Categories>();

            if (gradeLevelId.HasValue)
            {
                GradeLevel = db.GetGradeLevelById(gradeLevelId.Value);
            }
            else
            {
                GradeLevel = new MyGradeLevelModel();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (GradeLevel.Id > 0)
            {
                db.UpdateGradeLevel(GradeLevel);
                await Task.Run(() => repDb.UpdateEmployeesPayrollByGradeLevelAsync(GradeLevel.Id));
            }
            else
            {
                db.AddGradeLevel(GradeLevel);
            }
            
            return RedirectToPage("GradeLevels");
        }
    }
}

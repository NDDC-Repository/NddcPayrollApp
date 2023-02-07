using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.Enums;

namespace NddcPayroll.Web.Pages.Company
{
    public class AddGradeLevelModel : PageModel
    {
        private readonly ICompanyData db;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty]
        public MyGradeLevelModel GradeLevel { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        public AddGradeLevelModel(ICompanyData db, IHtmlHelper htmlHelper)
        {
            this.db = db;
            this.htmlHelper = htmlHelper;
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

        public IActionResult OnPost()
        {
            if (GradeLevel.Id > 0)
            {
                db.UpdateGradeLevel(GradeLevel);
            }
            else
            {
                db.AddGradeLevel(GradeLevel);
            }
            
            return RedirectToPage("GradeLevels");
        }
    }
}

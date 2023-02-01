using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Model;
using NddcPayrollLibrary.Model.Enums;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class AddBenefitsModel : PageModel
    {
        private readonly IPayrollData db;
        [BindProperty]
        public MyBenefitsModel MyBenefit { get; set; }
        public string Benefit { get; set; }
        public double Percentage { get; set; }
        //public IEnumerable<SelectListItem> BenefitsTypes { get; set; }
        public List<MyBenefitsTypeModel> BenefitTypes { get; set; }
        public IHtmlHelper HtmlHelper { get; }

        public AddBenefitsModel(IPayrollData db, IHtmlHelper htmlHelper)
        {
            this.db = db;
            HtmlHelper = htmlHelper;
        }
        public void OnGet()
        {
            //BenefitsTypes = HtmlHelper.GetEnumSelectList<BenefitTypes>();
            BenefitTypes = db.GetBenefitTypes();
        }

        public IActionResult OnPost(int? gradeLevelID)
        {
            MyBenefit.GradeLevelID = gradeLevelID.Value;
            db.AddBenefit(MyBenefit);
            return RedirectToPage("ViewBenefits", new { gradeLevelID = gradeLevelID.Value});
        }
    }
}

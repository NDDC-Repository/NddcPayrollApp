using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Model;
using NddcPayrollLibrary.Model.Enums;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class AddBenefitsModel : PageModel
    {
        private readonly IPayrollData db;
        private readonly IReportsData repDb;

        [BindProperty]
        public MyBenefitsModel MyBenefit { get; set; }
        public string Benefit { get; set; }
        public double Percentage { get; set; }
        //public IEnumerable<SelectListItem> BenefitsTypes { get; set; }
        public List<MyBenefitsTypeModel> BenefitTypes { get; set; }
        public IHtmlHelper HtmlHelper { get; }

        public AddBenefitsModel(IPayrollData db, IHtmlHelper htmlHelper, IReportsData repDb)
        {
            this.db = db;
            HtmlHelper = htmlHelper;
            this.repDb = repDb;
        }
        public void OnGet(int? gradeLevelID)
        {
            //BenefitsTypes = HtmlHelper.GetEnumSelectList<BenefitTypes>();
            BenefitTypes = db.GetBenefitTypes();
           
        }

        public IActionResult OnPost(int? gradeLevelID)
        {
            
            MyBenefit.GradeLevelID = gradeLevelID.Value;
            db.AddBenefit(MyBenefit);
            repDb.UpdateEmployeesPayrollByGradeLevelAsync(gradeLevelID.Value);
            return RedirectToPage("ViewBenefits", new { gradeLevelID = gradeLevelID.Value});
        }
    }
}

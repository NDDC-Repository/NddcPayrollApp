using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Model;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Employee
{
    public class AnalysisDetailsModel : PageModel
    {
        private readonly IPayrollData db;
        private readonly IHtmlHelper htmlHelper;

        public SalaryScale MySalaryScale { get; set; }
        public IEnumerable<SelectListItem> SalaryScales { get; set; }
        public IEnumerable<SalaryScale> MyGradeLevels { get; set; }

        public AnalysisDetailsModel(IPayrollData db, IHtmlHelper htmlHelper)
        {
            this.db = db;
            this.htmlHelper = htmlHelper;
        }
        public void OnGet()
        {
            MyGradeLevels = db.GetGradeLevels();
            //SalaryScales = htmlHelper.GetEnumSelectList<GradeLevels>();
            //GradeLevels = db.GetGradeLevels();
           
        }
    }
}

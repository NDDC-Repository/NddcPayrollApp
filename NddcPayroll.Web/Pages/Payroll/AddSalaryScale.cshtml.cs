using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NddcPayrollLibrary.Data;
using NddcPayrollLibrary.Model.Payroll;
using NddcPayrollLibrary.Model.Enums;
using NddcPayrollLibrary.Data.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class AddSalaryScaleModel : PageModel
    {
        private readonly IPayrollData _db;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty]
        public SalaryScale SalaryScale { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> MyGradeLevels { get; set; }

        public AddSalaryScaleModel(IPayrollData db, IHtmlHelper htmlHelper)
        {
            _db = db;
            this.htmlHelper = htmlHelper;
        }
        public void OnGet()
        {
            Categories = htmlHelper.GetEnumSelectList<Categories>();
            MyGradeLevels = htmlHelper.GetEnumSelectList<GradeLevels>();
        }

        public IActionResult OnPost()
        {
            _db.AddSalaryScale(SalaryScale);
            return RedirectToPage("SalaryScale");
        }
    }
}

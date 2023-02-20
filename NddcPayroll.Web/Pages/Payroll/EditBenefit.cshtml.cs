using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class EditBenefitModel : PageModel
    {
        private readonly IPayrollData payDb;
        [BindProperty]
        public MyBenefitsModel Benefit { get; set; }
        public List<MyBenefitsTypeModel> BenefitTypes { get; set; }

        public EditBenefitModel(IPayrollData payDb)
        {
            this.payDb = payDb;
        }
        public void OnGet(int? benefitId)
        {
            BenefitTypes = payDb.GetBenefitTypes();
            Benefit = payDb.GetBenefitsByBenefitId(benefitId.Value);
        }

        public IActionResult OnPost()
        {
            payDb.UpdateBenefit(Benefit);
            return RedirectToPage("Benefits");
        }
       
    }
}

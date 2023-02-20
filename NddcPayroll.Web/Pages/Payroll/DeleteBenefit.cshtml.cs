using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class DeleteBenefitModel : PageModel
    {
        private readonly IPayrollData payDb;

        public DeleteBenefitModel(IPayrollData payDb)
        {
            this.payDb = payDb;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost(int? benefitId)
        {
            payDb.DeleteBenefit(benefitId.Value);

            return RedirectToPage("Benefits");
        }
    }
}

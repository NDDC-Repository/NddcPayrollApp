using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class DeleteLinkedBenefitModel : PageModel
    {
        private readonly IPayrollData payDb;

        public DeleteLinkedBenefitModel(IPayrollData payDb)
        {
            this.payDb = payDb;
        }
        public void OnGet()
        {

        }

        public IActionResult OnPost(int? benefitId)
        {
            payDb.DeleteLinkedBenefit(benefitId.Value);
            
            return RedirectToPage("Benefits");
        }
    }
}

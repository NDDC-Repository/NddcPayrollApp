using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Helper;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Data.Reports;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class DeleteLinkedBenefitModel : PageModel
    {
        private readonly IPayrollData payDb;
        private readonly IHelperData helpDb;
        private readonly IReportsData repDb;

        public DeleteLinkedBenefitModel(IPayrollData payDb, IHelperData helpDb, IReportsData repDb)
        {
            this.payDb = payDb;
            this.helpDb = helpDb;
            this.repDb = repDb;
        }
        public void OnGet()
        {

        }

        public IActionResult OnPost(int? benefitId)
        {
            int gradeLevelId = helpDb.GetAnyRecord<int, int>("Benefits", "GradeLevelId", "Id", benefitId.Value);
            payDb.DeleteLinkedBenefit(benefitId.Value);

            repDb.UpdateEmployeesPayrollByGradeLevelAsync(gradeLevelId);

            return RedirectToPage("Benefits");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Helper;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Data.Reports;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class DeleteBenefitModel : PageModel
    {
        private readonly IPayrollData payDb;
        private readonly IReportsData repDb;
        private readonly IHelperData helpDb;

        public DeleteBenefitModel(IPayrollData payDb, IReportsData repDb, IHelperData helpDb)
        {
            this.payDb = payDb;
            this.repDb = repDb;
            this.helpDb = helpDb;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost(int? benefitId)
        {
            int gradeLevelId = helpDb.GetAnyRecord<int, int>("Benefits", "GradeLevelId", "Id", benefitId.Value);
            payDb.DeleteBenefit(benefitId.Value);
            
            repDb.UpdateEmployeesPayrollByGradeLevelAsync(gradeLevelId);

            return RedirectToPage("Benefits");
        }
    }
}

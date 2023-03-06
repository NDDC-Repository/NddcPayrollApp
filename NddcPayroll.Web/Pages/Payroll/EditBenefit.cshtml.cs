using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class EditBenefitModel : PageModel
    {
        private readonly IPayrollData payDb;
        private readonly IReportsData repDb;

        [BindProperty]
        public MyBenefitsModel Benefit { get; set; }
        public List<MyBenefitsTypeModel> BenefitTypes { get; set; }

        public EditBenefitModel(IPayrollData payDb, IReportsData repDb)
        {
            this.payDb = payDb;
            this.repDb = repDb;
        }
        public void OnGet(int? benefitId)
        {
            BenefitTypes = payDb.GetBenefitTypes();
            Benefit = payDb.GetBenefitsByBenefitId(benefitId.Value);
        }

        public IActionResult OnPostAsync()
        {
            payDb.UpdateBenefit(Benefit);
            repDb.UpdateEmployeesPayrollByGradeLevelAsync(Benefit.GradeLevelID);
            return RedirectToPage("Benefits");
        }
       
    }
}

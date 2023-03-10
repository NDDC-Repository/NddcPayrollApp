using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.Enums;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class EditLinkedBenefitModel : PageModel
    {
        private readonly ICompanyData compDb;
        private readonly IPayrollData payDb;
        private readonly IReportsData repDb;

        public List<MyGradeLevelGridModel> GradeLevels { get; set; }
        [BindProperty]
        public MyLinkedBenefitsModel LinkedBenefit { get; set; }
        public List<MyBenefitsTypeModel> BenefitTypes { get; set; }
        public string GradeLevelName { get; set; }

        public EditLinkedBenefitModel(ICompanyData compDb, IPayrollData payDb, IReportsData repDb)
        {
            this.compDb = compDb;
            this.payDb = payDb;
            this.repDb = repDb;
        }
        public void OnGet(int? benefitId)
        {
            GradeLevels = compDb.GetAllGradeLevels();
            BenefitTypes = payDb.GetBenefitTypes();
            LinkedBenefit = payDb.GetLinkedBenefitByLinkBenefitId(benefitId.Value);
            GradeLevelName = compDb.GetGradeLevelNameById(LinkedBenefit.GradeLevelId);
        }

        public IActionResult OnPost()
        {
            payDb.UpdateLinkedBenefit(LinkedBenefit);
            repDb.UpdateEmployeesPayrollByGradeLevelAsync(LinkedBenefit.GradeLevelId);
            //return RedirectToPage($"/Payroll/ViewLinkedBenefits?gradeLevelID={LinkedBenefit.GradeLevelId}");
            return RedirectToPage("Benefits");
        }
    }
}

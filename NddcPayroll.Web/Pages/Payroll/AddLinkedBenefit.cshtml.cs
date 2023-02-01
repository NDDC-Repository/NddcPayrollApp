using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.Enums;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class AddLinkedBenefitModel : PageModel
    {
        private readonly ICompanyData db;
        private readonly IPayrollData payDb;

        public List<MyGradeLevelGridModel> GradeLevels { get; set; }
        public List<MyBenefitsTypeModel> BenefitTypes { get; set; }
        [BindProperty]
        public MyLinkedBenefitsModel LinkedBenefit { get; set; }

        public AddLinkedBenefitModel(ICompanyData db, IPayrollData payDb)
        {
            this.db = db;
            this.payDb = payDb;
        }
        public void OnGet()
        {
            GradeLevels = db.GetAllGradeLevels();
            BenefitTypes = payDb.GetBenefitTypes();
        }


        public IActionResult OnPost(int? gradeLevelID)
        {
            LinkedBenefit.GradeLevelId = gradeLevelID.Value;
            payDb.AddLinkedBenefits(LinkedBenefit);

            return RedirectToPage("ViewLinkedBenefits", new { gradeLevelID = gradeLevelID.Value });
        }
    }
}

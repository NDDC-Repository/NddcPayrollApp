using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Model.Enums;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class ViewLinkedBenefitsModel : PageModel
    {
        private readonly IPayrollData db;
        private readonly ICompanyData compDb;

        public List<MyLinkedBenefitsModel> LinkedBenefits { get; set; }
        public int Id { get; set; }
        public string GradeLevelName { get; set; }

        public ViewLinkedBenefitsModel(IPayrollData db, ICompanyData compDb)
        {
            this.db = db;
            this.compDb = compDb;
        }
        public void OnGet(int? GradeLevelid)
        {
            Id = GradeLevelid.Value;
            LinkedBenefits = db.GetLinkedBenefits(Id);
            GradeLevelName = compDb.GetGradeLevelNameById(GradeLevelid.Value);
        }
    }
}

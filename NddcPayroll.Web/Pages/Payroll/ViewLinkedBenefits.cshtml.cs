using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class ViewLinkedBenefitsModel : PageModel
    {
        private readonly IPayrollData db;
        public List<MyLinkedBenefitsModel> LinkedBenefits { get; set; }
        public int Id { get; set; }

        public ViewLinkedBenefitsModel(IPayrollData db)
        {
            this.db = db;
        }
        public void OnGet(int? GradeLevelid)
        {
            Id = GradeLevelid.Value;
            LinkedBenefits = db.GetLinkedBenefits(Id);
        }
    }
}

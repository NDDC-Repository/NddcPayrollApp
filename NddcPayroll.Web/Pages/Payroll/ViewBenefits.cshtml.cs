using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class ViewBenefitsModel : PageModel
    {
        private readonly IPayrollData db;
        public List<MyBenefitsModel> Benefits { get; set; }
        //public MyBenefitsModel Item { get; set; }
        //public MyBenefitsModel Benefit { get; set; }
        public int Id { get; set; }

        public ViewBenefitsModel(IPayrollData db)
        {
            this.db = db;
        }
        public void OnGet(int? gradeLevelID)
        {
            Id = gradeLevelID.Value;
            Benefits = db.GetBenefitsById(gradeLevelID.Value);
           
        }
    }
}

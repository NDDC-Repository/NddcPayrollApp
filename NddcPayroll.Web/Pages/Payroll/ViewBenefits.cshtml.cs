using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class ViewBenefitsModel : PageModel
    {
        private readonly IPayrollData db;
        private readonly ICompanyData compDb;

        public List<MyBenefitsModel> Benefits { get; set; }
        //public MyBenefitsModel Item { get; set; }
        //public MyBenefitsModel Benefit { get; set; }
        public int Id { get; set; }
        public string GradeLevel { get; set; }

        public ViewBenefitsModel(IPayrollData db, ICompanyData compDb)
        {
            this.db = db;
            this.compDb = compDb;
        }
        public void OnGet(int? gradeLevelID)
        {
            Id = gradeLevelID.Value;
            Benefits = db.GetBenefitsById(gradeLevelID.Value);
            GradeLevel = compDb.GetGradeLevelNameById(gradeLevelID.Value);
        }
    }
}

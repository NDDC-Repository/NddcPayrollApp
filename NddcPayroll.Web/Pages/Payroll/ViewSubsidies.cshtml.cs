using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class ViewSubsidiesModel : PageModel
    {
        private readonly IPayrollData db;

        public List<MySubsidiesModel> Subsidies { get; set; }
        public int Id { get; set; }

        public ViewSubsidiesModel(IPayrollData db)
        {
            this.db = db;
        }
        public void OnGet(int? gradeLevelID)
        {

            Id = gradeLevelID.Value;
            Subsidies = db.GetSubsidies(Id);
        }
    }
}

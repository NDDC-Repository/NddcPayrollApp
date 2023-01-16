using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class SalaryScaleModel : PageModel
    {
        private readonly IPayrollData db;

        public List<SalaryScale> SalaryScales { get; set; }
        public SalaryScale MySalaryScale { get; set; }


        public SalaryScaleModel(IPayrollData db)
        {
            this.db = db;
        }
        public void OnGet()
        {
            SalaryScales = db.GetAllSalaryScale();
           
        }
    }
}

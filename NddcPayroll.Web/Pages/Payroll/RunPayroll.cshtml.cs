using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class RunPayrollModel : PageModel
    {
        private readonly IPayrollData payDb;
        public List<MyPayrollJournalTitleModel> PayrollListTitles { get; set; }

        public RunPayrollModel(IPayrollData payDb)
        {
            this.payDb = payDb;
        }
        public void OnGet()
        {
            PayrollListTitles = payDb.GetPayrollJournalTitles();
        }
    }
}

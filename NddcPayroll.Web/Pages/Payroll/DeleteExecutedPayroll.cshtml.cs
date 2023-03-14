using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Helper;
using NddcPayrollLibrary.Data.Payroll;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class DeleteExecutedPayrollModel : PageModel
    {
        private readonly IHelperData helpDb;
        private readonly IPayrollData payDb;

        public string MonthDate { get; set; }
        public string JournalTitle { get; set; }

        public DeleteExecutedPayrollModel(IHelperData helpDb, IPayrollData payDb)
        {
            this.helpDb = helpDb;
            this.payDb = payDb;
        }

        public void OnGet(int? payrollJournalTitleId)
        {
            MonthDate = helpDb.GetAnyRecord<string, int>("PayrollJournalTitles", "MonthYear", "Id", payrollJournalTitleId.Value);
            JournalTitle = helpDb.GetAnyRecord<string, int>("PayrollJournalTitles", "JournalName", "Id", payrollJournalTitleId.Value);
        }
        public IActionResult OnPost(int? payrollJournalTitleId)
        {
            
            payDb.DeleteExecutedPayroll(payrollJournalTitleId.Value);

            return RedirectToPage("RunPayroll");
        }
    }
}

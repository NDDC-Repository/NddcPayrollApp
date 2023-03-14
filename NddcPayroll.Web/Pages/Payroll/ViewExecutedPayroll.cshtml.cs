using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NddcPayroll.Web.Pages.Payroll
{
    public class ViewExecutedPayrollModel : PageModel
    {
        public int JournalTitleId { get; set; }

        public void OnGet(int? payrollJournalTitleId)
        {
            JournalTitleId = payrollJournalTitleId.Value;
        }
    }
}

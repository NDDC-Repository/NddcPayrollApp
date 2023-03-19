using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NddcPayroll.Web.Pages.Employee
{
    public class ManualEntryModel : PageModel
    {
        public int MyEmpId { get; set; }
        public void OnGet(int? EmpId)
        {
            MyEmpId = EmpId.Value;
        }
    }
}

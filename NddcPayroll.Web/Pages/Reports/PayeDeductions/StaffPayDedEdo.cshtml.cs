using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NddcPayroll.Web.Pages.Reports.PayeDeductions
{
    public class StaffPayDedEdoModel : PageModel
    {
        public string TaxStateProvince { get; set; }
        public void OnGet()
        {
            TaxStateProvince = "ESO";
        }
    }
}

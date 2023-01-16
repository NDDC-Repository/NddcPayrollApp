using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Model.Company;

namespace NddcPayroll.Web.Pages.Company
{
    public class AddPensionAdminModel : PageModel
    {
        private readonly ICompanyData db;

        [BindProperty]
        public PensionAdminModel PensionAdmin { get; set; }

        public AddPensionAdminModel(ICompanyData db)
        {
            this.db = db;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            db.AddPensionAdmin(PensionAdmin);
            return RedirectToPage("PensionAdmins");
        }
    }
}

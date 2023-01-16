using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Model.Company;

namespace NddcPayroll.Web.Pages.Company
{
    public class AddBankModel : PageModel
    {
        private readonly ICompanyData db;

        [BindProperty]
        public BankModel Bank { get; set; }

        public AddBankModel(ICompanyData db)
        {
            this.db = db;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            db.AddBank(Bank);
            return RedirectToPage("Banks");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Model.Company;

namespace NddcPayroll.Web.Pages.Company
{
    public class BanksModel : PageModel
    {
        private readonly ICompanyData db;

        public List<BankModel> Banks { get; set; }
        public BanksModel(ICompanyData db)
        {
            this.db = db;
        }
        public void OnGet()
        {
            Banks = db.GetAllBanks();
        }
    }
}

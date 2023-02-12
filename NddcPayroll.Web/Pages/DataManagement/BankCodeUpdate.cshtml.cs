using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.DataManagement;

namespace NddcPayroll.Web.Pages.DataManagement
{
    public class BankCodeUpdateModel : PageModel
    {
        private readonly IDataMigration db;

        public BankCodeUpdateModel(IDataMigration db)
        {
            this.db = db;
        }
        public void OnGet()
        {
            
        }

        public void OnPost()
        {
            db.UpdateBankCode();    
        }
    }
}

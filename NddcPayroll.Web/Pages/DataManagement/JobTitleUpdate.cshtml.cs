using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.DataManagement;

namespace NddcPayroll.Web.Pages.DataManagement
{
    public class JobTitleUpdateModel : PageModel
    {
        private readonly IDataMigration db;

        public JobTitleUpdateModel(IDataMigration db)
        {
            this.db = db;
        }
        public void OnGet()
        {
        }

        public void OnPost()
        {
            db.UpdateJobTitle();
        }
    }
}

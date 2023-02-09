using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.DataManagement;

namespace NddcPayroll.Web.Pages.DataManagement
{
    public class GradeLevelUpdateModel : PageModel
    {
        private readonly IDataMigration db;

        public GradeLevelUpdateModel(IDataMigration db)
        {
            this.db = db;
        }
        public void OnGet()
        {
        }
        public void OnPost() 
        {
            db.UpdateGradeLevel();
        }

    }
}

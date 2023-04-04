using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.DataManagement;

namespace NddcPayroll.Web.Pages.DataManagement
{
    public class PensionUpdateModel : PageModel
    {
        private readonly IDataMigration db;

        public PensionUpdateModel(IDataMigration db)
        {
            this.db = db;
        }
        public void OnGet()
        {
        }

        public void OnPost() 
        {
            db.UpdatePensionAdmin();
        }

        public void OnPostUpdatePaypoint()
        {
            db.UpdatePaypoint();
        }

        public void OnPostUpdateEmployerPension()
        {
            db.UpdateEmployerPension();
        }

        public void OnPostMigrateEmployees()
        {
            db.MigrateEmployees();
        }

        public void OnPostUpdateCooporative()
        {
            db.UpdateCooporative();
        }

    }
}

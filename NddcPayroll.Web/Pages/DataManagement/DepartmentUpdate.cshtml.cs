using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.DataManagement;

namespace NddcPayroll.Web.Pages.DataManagement
{
    public class DepartmentUpdateModel : PageModel
    {
        private readonly IDataMigration db;

        public DepartmentUpdateModel(IDataMigration db)
        {
            this.db = db;
        }
        public void OnGet()
        {
        }

        public void OnPost()
        {
            db.UpdateDepartment();
        }

    }
}

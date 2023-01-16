using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Model.Company;

namespace NddcPayroll.Web.Pages.Company
{
    public class PensionAdminsModel : PageModel
    {
        private readonly ICompanyData db;
       
        public List<PensionAdminModel> PensionAdmins { get; set; }

        public PensionAdminsModel(ICompanyData db)
        {
            this.db = db;
        }
        public void OnGet()
        {
            PensionAdmins = db.GetAllPensionAdmins();
        }


    }
}

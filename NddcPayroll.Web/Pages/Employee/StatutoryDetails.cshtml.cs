using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Model.Company;

namespace NddcPayroll.Web.Pages.Employee
{
    public class StatutoryDetailsModel : PageModel
    {
        private readonly ICompanyData db;
        public IEnumerable<MyStatesModel> States { get; set; }
        public IEnumerable<MyPensionFundListModel> PensionAdmins { get; set; }

        public StatutoryDetailsModel(ICompanyData db)
        {
            this.db = db;
        }
        public void OnGet()
        {
            States = db.GetAllStates();
            PensionAdmins = db.GetAllPensionAdminsList();
        }
    }
}

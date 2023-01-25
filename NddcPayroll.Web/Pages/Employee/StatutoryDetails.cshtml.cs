using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Data.Helper;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.Employee;

namespace NddcPayroll.Web.Pages.Employee
{
    public class StatutoryDetailsModel : PageModel
    {
        private readonly ICompanyData db;
        private readonly IHelperData helperDb;
        private readonly IEmployeeData empDb;

        public IEnumerable<MyStatesModel> States { get; set; }
        public IEnumerable<MyPensionFundListModel> PensionAdmins { get; set; }
        [BindProperty]
        public int Age { get; set; }
        [BindProperty]
        public MyStatutoryDetailsModel StatutoryDetails { get; set; }



        public StatutoryDetailsModel(ICompanyData db, IHelperData helperDb, IEmployeeData empDb)
        {
            this.db = db;
            this.helperDb = helperDb;
            this.empDb = empDb;
        }
        public void OnGet(int EmpId)
        {
            States = db.GetAllStates();
            PensionAdmins = db.GetAllPensionAdminsList();

            DateTime dob = helperDb.GetAnyRecord<DateTime, int>("Employees", "DateOfBirth", "Id", EmpId);

            Age = DateTime.Now.Year - dob.Year;

            
        }

        public IActionResult OnPost(int? EmpId)
        {
            StatutoryDetails.EmployeeId = EmpId.Value;
            empDb.AddStatutoryDetails(StatutoryDetails);
            return RedirectToPage("PaymentDetails", new { EmpId = EmpId });
        }
    }
}

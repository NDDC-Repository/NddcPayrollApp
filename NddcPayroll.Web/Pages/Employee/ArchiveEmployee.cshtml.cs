using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Data.Helper;

namespace NddcPayroll.Web.Pages.Employee
{
    public class ArchiveEmployeeModel : PageModel
    {
        private readonly IHelperData helpDb;
        private readonly IEmployeeData empDb;

        public string EmpName { get; set; }
        [BindProperty]
        public string ExitCondition { get; set; }
        [BindProperty]
        public DateTime ExitDate { get; set; }
        public ArchiveEmployeeModel(IHelperData helpDb, IEmployeeData empDb)
        {
            this.helpDb = helpDb;
            this.empDb = empDb;
        }
        public void OnGet(int? EmpId)
        {
            EmpName = helpDb.GetAnyRecord<string, int>("Employees", "(FirstName + ' ' + LastName) As Name", "ID", EmpId.Value);
            ExitDate = DateTime.Now;
        }

        public IActionResult OnPost(int? EmpId)
        {
            empDb.ArchiveEmployee(EmpId.Value, ExitCondition, ExitDate);
            return RedirectToPage("Employees");
        }
    }
}

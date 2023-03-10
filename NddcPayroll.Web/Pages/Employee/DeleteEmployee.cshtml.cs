using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Data.Helper;

namespace NddcPayroll.Web.Pages.Employee
{
    public class DeleteEmployeeModel : PageModel
    {
        private readonly IEmployeeData empDb;
        private readonly IHelperData helpDb;

        public string EmpName { get; set; }

        public DeleteEmployeeModel(IEmployeeData empDb, IHelperData helpDb)
        {
            this.empDb = empDb;
            this.helpDb = helpDb;
        }
        public void OnGet(int? EmpId)
        {
            EmpName = helpDb.GetAnyRecord<string, int>("Employees", "(FirstName + ' ' + LastName) As Name", "ID", EmpId.Value);
        }

        public IActionResult OnPost(int? EmpId)
        {
            empDb.DeleteEmployee(EmpId.Value);

            return RedirectToPage("Employees");

        }
    }
}

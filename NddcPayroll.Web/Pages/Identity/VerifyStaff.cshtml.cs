using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Model.Employee;

namespace NddcPayroll.Web.Pages.Identity
{
    public class VerifyStaffModel : PageModel
    {
        private readonly IEmployeeData empDb;

        public EmployeeModel Employee { get; set; }
        public VerifyStaffModel(IEmployeeData empDb)
        {
            this.empDb = empDb;
        }
        public void OnGet(int? EmpId)
        {
            Employee = empDb.GetEmployeeDetails(EmpId.Value);
        }
    }
}

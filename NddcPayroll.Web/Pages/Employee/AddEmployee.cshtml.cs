using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.Employee;
using NddcPayrollLibrary.Model.Enums;

namespace NddcPayroll.Web.Pages.Employee
{
    public class AddEmployeeModel : PageModel
    {
        private readonly ICompanyData db;
        private readonly IEmployeeData dbEmp;
        private readonly IHtmlHelper htmlHelper;

        public IEnumerable<MyStatesModel> States { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        [BindProperty (SupportsGet = true)]
        public EmployeeModel Employee { get; set; }
       

        public AddEmployeeModel(ICompanyData db, IEmployeeData dbEmp, IHtmlHelper htmlHelper)
        {
            this.db = db;
            this.dbEmp = dbEmp;
            this.htmlHelper = htmlHelper;
        }
        public void OnGet()
        {
            Employee.DateOfBirth = DateTime.Now;
            Employee.DateEngaged = DateTime.Now;
            States = db.GetAllStates();
            Categories = htmlHelper.GetEnumSelectList<Categories>();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                if (dbEmp.EmployeeExists(Employee.EmployeeCode))
                {
                    return RedirectToPage("EmpExists");
                }
                else
                {
                    int Id = dbEmp.AddEmployee(Employee);

                    return RedirectToPage("StatutoryDetails", new { EmpId = Id });
                }
            }
            else
            {
                States = db.GetAllStates();
                Categories = htmlHelper.GetEnumSelectList<Categories>();
                return Page();
            }
           
            
        }
    }
}

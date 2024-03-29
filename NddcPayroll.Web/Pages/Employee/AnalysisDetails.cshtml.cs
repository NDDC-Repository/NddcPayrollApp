using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NddcPayrollLibrary.Data.Company;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Model;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.Employee;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayroll.Web.Pages.Employee
{
    public class AnalysisDetailsModel : PageModel
    {
        private readonly ICompanyData db;
        private readonly IEmployeeData empDb;
        private readonly IReportsData repDb;
        private readonly IPayrollData payDb;

        public List<MyGradeLevelGridModel> MyGradeLevels { get; set; }
        public List<JobTitleModel> JobTitles { get; set; }
        public List<DepartmentModel> Departments { get; set; }
        public List<MyPayPointModel> PayPoints { get; set; }
        [BindProperty]
        public MyAnalysisDetailsModel AnalysisDetail { get; set; }
        [BindProperty]
        public bool ApplyArrears { get; set; }

        public AnalysisDetailsModel(ICompanyData db, IEmployeeData EmpDb, IReportsData repDb, IPayrollData payDb)
        {
            this.db = db;
            empDb = EmpDb;
            this.repDb = repDb;
            this.payDb = payDb;
        }
        public void OnGet()
        {
            MyGradeLevels = db.GetAllGradeLevels();
            JobTitles = db.GetAllJobTitles();
            Departments = db.GetAllDepartments();
            PayPoints = db.GetAllPayPoints();
        }

        public async Task<IActionResult> OnPostAsync(int? EmpId)
        {
            AnalysisDetail.EmployeeId = EmpId.Value;
            empDb.AddAnalysisDetails(AnalysisDetail);
            if (ApplyArrears)
            {
                payDb.AddArrears(EmpId.Value);
            }

            await (Task.Run(() => repDb.UpdateEmployeesPayrollByEmpIdAsync(EmpId.Value)));

            return RedirectToPage("Employees");
        }
    }
}

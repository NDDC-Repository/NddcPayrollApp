using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Model.Report;

namespace NddcPayroll.Web.Pages.Reports
{
    public class PayrollListingsModel : PageModel
    {
        private readonly IReportsData db;

        public List<MyPayRollListModel> PayrollList { get; set; }
        public PayrollListingsModel(IReportsData db)
        {
            this.db = db;
        }
        public void OnGet()
        {
            PayrollList = db.GetPayrollListReport();

        }
    }
}

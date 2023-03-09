using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Model.Report;

namespace NddcPayroll.Web.Pages.Reports
{
    public class PayrollSummaryByDeptModel : PageModel
    {
        private readonly IReportsData repDb;

        public List<MyPayrollSummaryByDepartmentModel> PayrollSumList { get; set; }
        public List<MyPayrollSummaryByDepartmentModel> PayrollTotals { get; set; }
        public PayrollSummaryByDeptModel(IReportsData repDb)
        {
            this.repDb = repDb;
        }
        public async Task OnGetAsync()
        {
            PayrollSumList = await Task.Run(() => repDb.GetPayrollSummaryByDeptReportAsync());
            PayrollTotals = await Task.Run(() => repDb.GetPayrollTotalsReportAsync());
        }
    }
}

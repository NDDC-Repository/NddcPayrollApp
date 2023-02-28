using NddcPayrollLibrary.Model.Report;

namespace NddcPayrollLibrary.Data.Reports
{
    public interface IReportsData
    {
        Task<List<MyNHFReportModel>> GetNHFReportAsync();
        Task<List<MyPayeReportSummaryModel>> GetPayeSummaryReportAsync();
        List<MyPayRollListModel> GetPayrollListReport();
        Task<List<MyPayRollListModel>> GetPayrollListReport2Async();
        Task<List<MyPayRollListModel>> GetPayrollListReportAsync();
        Task<List<MyPayrollSummaryByDepartmentModel>> GetPayrollSummaryByDeptReportAsync();
        Task<List<MyRemitaUploadModel>> GetRemitaReportAsync();
        Task<List<MyStaffPayeDeductionsModel>> GetStaffPayeDeductionByLocationAsync(string staffStateProvince);
        Task UpdateEmployeesPayrollAsync();
    }
}
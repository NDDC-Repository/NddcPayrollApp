using NddcPayrollLibrary.Model.Report;

namespace NddcPayrollLibrary.Data.Reports
{
    public interface IReportsData
    {
        Task<List<MyNHFReportModel>> GetNHFReportAsync();
        Task<List<MyPayeReportSummaryModel>> GetPayeSummaryReportAsync();
        List<MyPayRollListModel> GetPayrollListReport();
        Task<List<MyPayRollListModel>> GetPayrollListReport2Async();
        //Task<List<MyPayRollListModel>> GetPayrollListReport2Async(int payrollJournalTitleId);
        Task<List<MyPayRollListModel>> GetPayrollListReportAsync();
        List<MyPayRollListModel> GetPayrollListReportById(int payrollJournalTitleId);
        Task<List<MyPayRollListModel>> GetPayrollListSummaryReportAsync(int payrollJournalTitleId);
        Task<List<MyPayrollSummaryByDepartmentModel>> GetPayrollSummaryByDeptReportAsync();
        Task<List<MyPayrollSummaryByDepartmentModel>> GetPayrollTotalsReportAsync();
        Task<List<MyRemitaUploadModel>> GetRemitaReportAsync();
        Task<List<MyStaffPayeDeductionsModel>> GetStaffPayeDeductionByLocationAsync(string staffStateProvince);
        Task UpdateEmployeesPayrollAsync();
        Task UpdateEmployeesPayrollByEmpIdAsync(int empId);
        Task UpdateEmployeesPayrollByGradeLevelAsync(int gradeLevelId);
    }
}
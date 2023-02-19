using NddcPayrollLibrary.Model.Report;

namespace NddcPayrollLibrary.Data.Reports
{
    public interface IReportsData
    {
        Task<List<MyNHFReportModel>> GetNHFReportAsync();
        List<MyPayRollListModel> GetPayrollListReport();
        Task<List<MyPayRollListModel>> GetPayrollListReportAsync();
        Task<List<MyRemitaUploadModel>> GetRemitaReportAsync();
    }
}
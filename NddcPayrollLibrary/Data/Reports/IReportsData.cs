using NddcPayrollLibrary.Model.Report;

namespace NddcPayrollLibrary.Data.Reports
{
    public interface IReportsData
    {
        List<MyPayRollListModel> GetPayrollListReport();
        Task<List<MyPayRollListModel>> GetPayrollListReportAsync();
    }
}
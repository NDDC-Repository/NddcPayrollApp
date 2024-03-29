﻿using NddcPayrollLibrary.Model.Report;

namespace NddcPayrollLibrary.Data.Reports
{
    public interface IReportsData
    {
        List<MyEarningsByDeptChartModel> GetEarningsByDeptData();
        List<MyPayRollListModel> GetEmployeeAdhocAsync(int gradeLevelId, string category, int departmentId, string gender);
        Task<List<MyNHFReportModel>> GetNHFReportAsync();
        List<MyNHFReportModel> GetNHFReportByPaypoint(string payPoint);
        List<MyNHFReportModel> GetNHFReportSummary();
        Task<List<MyPayeReportSummaryModel>> GetPayeSummaryReportAsync();
        List<MyPayPointChartModel> GetPayPointData();
        List<MyPayRollListModel> GetPayrollListReport();
        Task<List<MyPayRollListModel>> GetPayrollListReport2Async();
        //Task<List<MyPayRollListModel>> GetPayrollListReport2Async(int payrollJournalTitleId);
        Task<List<MyPayRollListModel>> GetPayrollListReportAsync();
        List<MyPayRollListModel> GetPayrollListReportById(int payrollJournalTitleId);
        Task<List<MyPayRollListModel>> GetPayrollListSummaryReportAsync(int payrollJournalTitleId);
        Task<List<MyPayrollSummaryByDepartmentModel>> GetPayrollSummaryByDeptReportAsync();
        Task<List<MyPayrollSummaryByDepartmentModel>> GetPayrollTotalsReportAsync();
        List<PensionSummaryModel> GetPFASummary();
        List<PensionSummaryModel> GetPFASummaryForEmployees(int pensionFundId);
        Task<List<MyRemitaUploadModel>> GetRemitaReportAsync();
        Task<List<MyStaffPayeDeductionsModel>> GetStaffPayeDeductionByLocationAsync(string staffStateProvince);
        Task UpdateEmployeesPayrollAsync();
        Task UpdateEmployeesPayrollByEmpIdAsync(int empId);
        Task UpdateEmployeesPayrollByGradeLevelAsync(int gradeLevelId);
    }
}
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayrollLibrary.Data.PayrollJournal
{
    public interface IPayrollJournalData
    {
        void AddPayrollJournal(MyPayrollJournalTitleModel JournalTitle);
        void ClearOnetimeData();
        void DeleteExecutedPayroll(int Id);
        List<MyPayrollJournalTitleModel> GetPayrollJournalTitles();
    }
}
using NddcPayrollLibrary.Model.Employee;
using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayrollLibrary.Data.Payroll
{
    public interface IPayrollData
    {
        void AddBenefit(MyBenefitsModel Benefit);
        void AddSalaryScale(SalaryScale scale);
        List<SalaryScale> GetAllSalaryScale();
        List<MyBenefitsModel> GetBenefitsById(int SalaryScaleId);
        IEnumerable<SalaryScale> GetGradeLevels();
        decimal GetBasicSalary(int EmpId);
        decimal GetBenefits(int EmpId);
        decimal GetMonthlyGross(int empId);
        List<MyBenefitsTypeModel> GetBenefitTypes();
        List<MyLinkedBenefitsModel> GetLinkedBenefits(int gradeLevelId);
        void AddLinkedBenefits(MyLinkedBenefitsModel LinkBenefit);
        List<MySubsidiesModel> GetSubsidies(int gradeLevelId);
        void AddSubsidies(MySubsidiesModel Subsidy);
        decimal GetLinkedBenefitsAmount(int empId);
        MyBenefitsModel GetBenefitsByBenefitId(int GradeLevelID);
        void UpdateBenefit(MyBenefitsModel Benefit);
        void DeleteBenefit(int Id);
        MyLinkedBenefitsModel GetLinkedBenefitByLinkBenefitId(int linkedBenefitId);
        void UpdateLinkedBenefit(MyLinkedBenefitsModel LinkedBenefit);
        void DeleteLinkedBenefit(int Id);
        void AddPayrollJournal(MyPayrollJournalTitleModel JournalTitle);
        List<MyPayrollJournalTitleModel> GetPayrollJournalTitles();
        void DeleteExecutedPayroll(int Id);
    }
}
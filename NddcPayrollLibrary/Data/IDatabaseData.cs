using NddcPayrollLibrary.Model.Payroll;

namespace NddcPayrollLibrary.Data
{
    public interface IDatabaseData
    {
        void AddSalaryScale(SalaryScale scale);
        List<SalaryScale> GetAllSalaryScale();
        void AddBenefit(MyBenefitsModel Benefit);
        List<MyBenefitsModel> GetBenefitsById(int SalaryScaleId);
        IEnumerable<SalaryScale> GetGradeLevels();
    }
}
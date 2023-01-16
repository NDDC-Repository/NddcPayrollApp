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
    }
}
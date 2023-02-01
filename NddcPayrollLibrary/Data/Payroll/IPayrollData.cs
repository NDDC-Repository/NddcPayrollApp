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
        double GetBasicSalary(int EmpId);
        double GetBenefits(int EmpId);
        double GetMonthlyGross(int empId);
        List<MyBenefitsTypeModel> GetBenefitTypes();
        List<MyLinkedBenefitsModel> GetLinkedBenefits(int gradeLevelId);
        void AddLinkedBenefits(MyLinkedBenefitsModel LinkBenefit);
        List<MySubsidiesModel> GetSubsidies(int gradeLevelId);
        void AddSubsidies(MySubsidiesModel Subsidy);
    }
}
namespace NddcPayrollLibrary.Data.Calculations.Deductions
{
    public interface IDeductionData
    {
        decimal ApplyCompanyRelief(int empId);
        decimal ApplyStateRelief(decimal totalEarnings);
        decimal GetBasicSalary(int empId);
        decimal GetMonthlyGross(int empId);
        decimal GetPAYEAmount(int empId);
        decimal GetPensionAmount(int empId);
        decimal GetNHFAmount(int empId);
        decimal GetSSA(int empId);
        decimal GetJSA(int empId);
        decimal GetTotalDeductions(int empId);
        decimal GetEmployerPensionAmount(int empId);
    }
}
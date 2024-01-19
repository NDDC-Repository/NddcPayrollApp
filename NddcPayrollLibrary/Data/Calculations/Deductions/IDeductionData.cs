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
        
        void ClearCoopValues();
        void RecalculateManualForDeductions(string empCode);
        decimal GetPAYEAmountManual(decimal totalEarnings, decimal insurance, decimal pension, decimal nhf, int empId, decimal taxAdjustment);
        void ClearInsurance();
        decimal GetLoanPayment(int empId);
    }
}
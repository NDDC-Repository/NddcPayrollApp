namespace NddcPayrollLibrary.Data.Calculations.Allowance
{
    public interface IAllowanceData
    {
        decimal GetContStaffBasicSalary(int EmpId);
        decimal GetHousingAllowance(int empId);
        decimal GetPermStaffBasicSalary(int EmpId);
        decimal GetFurnitureAllowance(int empId);
        decimal GetTransportAllowance(int empId);
        decimal GetMealAllowance(int empId);
        decimal GetUtilityAllowance(int empId);
        decimal GetEducationAllowance(int empId);
        decimal GetSecurityAllowance(int empId);
        decimal GetDomesticServantAllowance(int empId);
        decimal GetMedicalAllowance(int empId);
        decimal GetDriverAllowance(int empId);
        decimal GetVehicleMaintenanceAllowance(int empId);
        decimal GetHazardAllowance(int empId);
        decimal GetSecretarialAllowance(int EmpId);
        decimal GetEntertainmentAllow(int empId);
        decimal GetNewspaperAllow(int empId);
    }
}
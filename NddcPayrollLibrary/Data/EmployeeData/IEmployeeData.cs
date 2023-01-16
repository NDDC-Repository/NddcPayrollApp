using NddcPayrollLibrary.Model.Employee;

namespace NddcPayrollLibrary.Data.EmployeeData
{
    public interface IEmployeeData
    {
        void AddEmployee(EmployeeModel Employee);
        List<EmployeeGridModel> GetAllEmployees();
    }
}
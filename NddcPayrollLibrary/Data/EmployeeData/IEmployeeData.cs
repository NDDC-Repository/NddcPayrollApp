﻿using NddcPayrollLibrary.Model.Employee;

namespace NddcPayrollLibrary.Data.EmployeeData
{
    public interface IEmployeeData
    {
        int AddEmployee(EmployeeModel Employee);
        List<EmployeeGridModel> GetAllEmployees(string name);
        void AddStatutoryDetails(MyStatutoryDetailsModel Employee);
        void AddAnalysisDetails(MyAnalysisDetailsModel AnalysisDetails);
        EmployeeModel GetEmployeeDetails(int EmpId);
    }
}
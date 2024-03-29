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
        MyStatutoryDetailsModel GetStatutoryDetails(int EmpId);
        void UpdateInsurance(string EmployeeCode, double Amount);
        decimal GetEmployeeInsurance(int EmpId);
        bool GetPensionStatus(int EmpId);
        void UpdateCooporative(string EmployeeCode, double cooporativeDed);
        void UpdateSecretarialAllow(string EmployeeCode, string secretarialAllow);
        void UpdateVoluntaryPension(string EmployeeCode, string voluntaryPension);
        int GetEmployeeCount();
        void UpdateEmployeePayroll(EmployeeModel employee);
        EmployeeModel GetAnalysisDetails(int EmpId);
        EmployeeLinksModel GetEmployeeLinks(int EmpId);
        void UpdateForLinkCalc(int gradeLevelId, string category, int EmpId);
        void SaveEmployeeLink(EmployeeLinksModel employee);
        void DeleteEmployee(int Id);
        void ArchiveEmployee(int Id, string ExitCondition, DateTime ExitDate);
        void UnArchiveEmployee(int Id);
        List<EmployeeGridModel> GetArchivedEmployees(string name);
        void UpdateEmployee(EmployeeModel employee);
        bool EmployeeExists(string EmployeeCode);
        List<EmployeeGridModel> GetAllEmployeesAddedThisMonth();
        int CountEmployeesAddedThisMonth();
        int CountEmployeesExitedThisMonth();
        List<EmployeeGridModel> GetExpiringContract();
        void UpdateNhf(string EmployeeCode, double nhfAmount);
        void UpdateAnalysisDetails(int JobTitleId, int DeptId, string Paypoint, int EmpId);
        MyPaySlipModel GetEmployeePaySlip(int empId, int payrollJournalId);
        List<MyPaySlipModel> GetEmployeePaySlipByDept(int departmentId, int payrollJournalId);
        void UpdateLeaveAllowance(string EmployeeCode, double leaveAllowance);
    }
}
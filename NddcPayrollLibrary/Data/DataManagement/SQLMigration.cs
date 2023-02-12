using NddcPayrollLibrary.Databases;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.DataManagement.DataMigration;
using NddcPayrollLibrary.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Data.DataManagement
{
    public class SQLMigration : IDataMigration
    {
        private const string connectionStringName = "SqlDb";
        private readonly ISqlDataAccess db;
        public List<MyEmployeeMigrationModel> MigrationEmployees { get; set; }

        public SQLMigration(ISqlDataAccess db)
        {
            this.db = db;
        }

        public void UpdateGradeLevel()
        {
            string empCode = string.Empty;
            string jobGrade = string.Empty;
            int gradeLevelId = 0;
            MigrationEmployees = db.LoadData<MyEmployeeMigrationModel, dynamic>("Select EmpCode, JobGrade From MigrateEmployees", new { }, connectionStringName, false).ToList();
            foreach (var employee in MigrationEmployees)
            {
                empCode = employee.EmpCode;
                jobGrade = employee.JobGrade;
                gradeLevelId = db.LoadData<int, dynamic>("Select Id From GradeLevel Where GradeLevel = @GradeLevel", new { GradeLevel = jobGrade }, connectionStringName, false).FirstOrDefault();
                db.SaveData("Update Employees Set GradeLevelId = @GradeLevelId Where EmployeeCode = @EmployeeCode ", new { GradeLevelId = gradeLevelId, EmployeeCode = empCode }, connectionStringName, false);
            }
        }
        public void UpdateDepartment()
        {
            string empCode = string.Empty;
            string departmentCode = string.Empty;
            int id = 0;
            MigrationEmployees = db.LoadData<MyEmployeeMigrationModel, dynamic>("Select EmpCode, Department From MigrateEmployees", new { }, connectionStringName, false).ToList();
            foreach (var employee in MigrationEmployees)
            {
                empCode = employee.EmpCode;
                departmentCode = employee.Department;
                id = db.LoadData<int, dynamic>("Select Id From Departments Where Code = @Code", new { Code = departmentCode }, connectionStringName, false).FirstOrDefault();
                db.SaveData("Update Employees Set DepartmentId = @DepartmentId Where EmployeeCode = @EmployeeCode ", new { DepartmentId = id, EmployeeCode = empCode }, connectionStringName, false);
            }
        }
        public void UpdateJobTitle()
        {
            string empCode = string.Empty;
            string jobTitleAbrv = string.Empty;
            int id = 0;
            MigrationEmployees = db.LoadData<MyEmployeeMigrationModel, dynamic>("Select EmpCode, JobTitle From MigrateEmployees", new { }, connectionStringName, false).ToList();
            foreach (var employee in MigrationEmployees)
            {
                empCode = employee.EmpCode;
                jobTitleAbrv = employee.JobTitle;
                id = db.LoadData<int, dynamic>("Select Id From JobTitles Where Abbreviation = @Abbrv", new { Abbrv = jobTitleAbrv }, connectionStringName, false).FirstOrDefault();
                db.SaveData("Update Employees Set JobTitleId = @JobTitleId Where EmployeeCode = @EmployeeCode ", new { JobTitleId = id, EmployeeCode = empCode }, connectionStringName, false);
            }
        }

        public void UpdateBankCode()
        {
            string empCode = string.Empty;
            string fullBankCode = string.Empty;
            string trimmedBankCode = string.Empty;
            int id = 0;
            MigrationEmployees = db.LoadData<MyEmployeeMigrationModel, dynamic>("Select EmployeeCode, BankCode From Employees", new { }, connectionStringName, false).ToList();
            foreach (var employee in MigrationEmployees)
            {
                empCode = employee.EmployeeCode;
                fullBankCode = employee.BankCode;
                trimmedBankCode = fullBankCode.Remove(0, 3);
                //id = db.LoadData<int, dynamic>("Select Id From JobTitles Where Abbreviation = @Abbrv", new { Abbrv = jobTitleAbrv }, connectionStringName, false).FirstOrDefault();
                db.SaveData("Update Employees Set BankCode = @BankCode Where EmployeeCode = @EmployeeCode ", new { BankCode = trimmedBankCode, EmployeeCode = empCode }, connectionStringName, false);
            }
        }

        public void UpdatePensionAdmin()
        {
            string empCode = string.Empty;
            string pensionCode = string.Empty;
            int id = 0;
            MigrationEmployees = db.LoadData<MyEmployeeMigrationModel, dynamic>("Select EmpCode, PensionCode From MigrateEmployees", new { }, connectionStringName, false).ToList();
            foreach (var employee in MigrationEmployees)
            {
                empCode = employee.EmpCode;
                pensionCode = employee.PensionCode;
                id = db.LoadData<int, dynamic>("Select Id From PensionAdministrators Where Code = @Code", new { Code = pensionCode }, connectionStringName, false).FirstOrDefault();
                db.SaveData("Update Employees Set PensionFundId = @PensionFundId Where EmployeeCode = @EmployeeCode ", new { PensionFundId = id, EmployeeCode = empCode }, connectionStringName, false);
            }
        }

        public void UpdatePaypoint()
        {
            string empCode = string.Empty;
            string paypoint = string.Empty;
            //int id = 0;
            MigrationEmployees = db.LoadData<MyEmployeeMigrationModel, dynamic>("Select EmpCode, State From MigrateEmployees", new { }, connectionStringName, false).ToList();
            foreach (var employee in MigrationEmployees)
            {
                empCode = employee.EmpCode;
                paypoint = employee.State;
                //id = db.LoadData<int, dynamic>("Select Id From PensionAdministrators Where Code = @Code", new { Code = pensionCode }, connectionStringName, false).FirstOrDefault();
                db.SaveData("Update Employees Set PayPoint = @PayPoint Where EmployeeCode = @EmployeeCode ", new { PayPoint = paypoint, EmployeeCode = empCode }, connectionStringName, false);
            }
        }
    }
}

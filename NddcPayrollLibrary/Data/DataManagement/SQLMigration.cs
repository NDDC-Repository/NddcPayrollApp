using NddcPayrollLibrary.Databases;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.DataManagement.DataMigration;
using NddcPayrollLibrary.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}

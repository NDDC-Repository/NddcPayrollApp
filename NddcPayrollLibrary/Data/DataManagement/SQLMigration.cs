using NddcPayrollLibrary.Data.Calculations.Deductions;
using NddcPayrollLibrary.Data.Reports;
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
        private readonly IDeductionData dedDb;
        private readonly IReportsData repDb;

        public List<MyEmployeeMigrationModel> MigrationEmployees { get; set; }

        public SQLMigration(ISqlDataAccess db, IDeductionData dedDb, IReportsData repDb)
        {
            this.db = db;
            this.dedDb = dedDb;
            this.repDb = repDb;
        }

        public void UpdateGradeLevel()
        {
            string empCode = string.Empty;
            string jobGrade = string.Empty;
            int gradeLevelId = 0;
            MigrationEmployees = db.LoadData<MyEmployeeMigrationModel, dynamic>("Select EmployeeCode, JobGrade From FebMigration", new { }, connectionStringName, false).ToList();
            foreach (var employee in MigrationEmployees)
            {
                empCode = employee.EmployeeCode;
                jobGrade = employee.JobGrade;
                gradeLevelId = db.LoadData<int, dynamic>("Select Id From GradeLevel Where GradeLevel = @GradeLevel", new { GradeLevel = jobGrade }, connectionStringName, false).FirstOrDefault();
                db.SaveData("Update FebMigration Set GradeLevelId = @GradeLevelId Where EmployeeCode = @EmployeeCode ", new { GradeLevelId = gradeLevelId, EmployeeCode = empCode }, connectionStringName, false);
            }
        }
        public void UpdateDepartment()
        {
            string empCode = string.Empty;
            string departmentCode = string.Empty;
            int id = 0;
            MigrationEmployees = db.LoadData<MyEmployeeMigrationModel, dynamic>("Select EmployeeCode, Department From FebMigration", new { }, connectionStringName, false).ToList();
            foreach (var employee in MigrationEmployees)
            {
                empCode = employee.EmployeeCode;
                departmentCode = employee.Department;
                id = db.LoadData<int, dynamic>("Select Id From Departments Where Code = @Code", new { Code = departmentCode }, connectionStringName, false).FirstOrDefault();
                db.SaveData("Update FebMigration Set DepartmentId = @DepartmentId Where EmployeeCode = @EmployeeCode ", new { DepartmentId = id, EmployeeCode = empCode }, connectionStringName, false);
            }
        }
        public void UpdateJobTitle()
        {
            string empCode = string.Empty;
            string jobTitleAbrv = string.Empty;
            int id = 0;
            MigrationEmployees = db.LoadData<MyEmployeeMigrationModel, dynamic>("Select EmployeeCode, JobTitleCode From FebMigration", new { }, connectionStringName, false).ToList();
            foreach (var employee in MigrationEmployees)
            {
                empCode = employee.EmployeeCode;
                jobTitleAbrv = employee.JobTitleCode;
                id = db.LoadData<int, dynamic>("Select Id From JobTitles Where Code = @Code", new { Code = jobTitleAbrv }, connectionStringName, false).FirstOrDefault();
                db.SaveData("Update FebMigration Set JobTitleId = @JobTitleId Where EmployeeCode = @EmployeeCode ", new { JobTitleId = id, EmployeeCode = empCode }, connectionStringName, false);
            }
        }

        public void UpdateBankCode()
        {
            string empCode = string.Empty;
            string fullBankCode = string.Empty;
            string trimmedBankCode = string.Empty;
            int id = 0;
            MigrationEmployees = db.LoadData<MyEmployeeMigrationModel, dynamic>("Select EmployeeCode, BankCode From FebMigration", new { }, connectionStringName, false).ToList();
            foreach (var employee in MigrationEmployees)
            {
                empCode = employee.EmployeeCode;
                fullBankCode = employee.BankCode;
                trimmedBankCode = fullBankCode.Remove(0, 3);
                //id = db.LoadData<int, dynamic>("Select Id From JobTitles Where Abbreviation = @Abbrv", new { Abbrv = jobTitleAbrv }, connectionStringName, false).FirstOrDefault();
                db.SaveData("Update FebMigration Set BankCode = @BankCode Where EmployeeCode = @EmployeeCode ", new { BankCode = trimmedBankCode, EmployeeCode = empCode }, connectionStringName, false);
            }
        }

        public void UpdateEmployerPension()
        {
            decimal empPensionAmt = 0.00M;
            string empCode = "";
            MigrationEmployees = db.LoadData<MyEmployeeMigrationModel, dynamic>("Select Id, EmployeeCode From Employees", new { }, connectionStringName, false).ToList();
            foreach (var employee in MigrationEmployees)
            {
                empPensionAmt = dedDb.GetEmployerPensionAmount(employee.Id);
                empCode = employee.EmployeeCode;
                //id = db.LoadData<int, dynamic>("Select Id From JobTitles Where Abbreviation = @Abbrv", new { Abbrv = jobTitleAbrv }, connectionStringName, false).FirstOrDefault();
                db.SaveData("Update Employees Set EmployerPension = @EmployerPension Where EmployeeCode = @EmployeeCode ", new { EmployeeCode = empCode, EmployerPension = empPensionAmt }, connectionStringName, false);
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

        public List<EmployeeModel> Employees { get; set; }
        public void MigrateEmployees()
        {


            MigrationEmployees = db.LoadData<MyEmployeeMigrationModel, dynamic>("Select EmployeeCode, PensionFundNumber, FirstName, LastName, TaxStateProvince, GradeLevelId, Category, DepartmentId, DateOfBirth, DateEngaged, BankName, BankCode, AccountNumber, Gender, MaritalStatus, JobTitleId, AccountName, PensionFundId  From FebMigration", new { }, connectionStringName, false).ToList();

            //Employee.CreatedBy = "Admin";
            //Employee.DateCreated = DateTime.Now;
            foreach (var employee in MigrationEmployees)
            {
                employee.CreatedBy = "Admin";
                employee.DateCreated = DateTime.Now;
                string SQL = "Update Employees Set BasicSalary = 0.00," +
               "Insurance = 0.00, SecretarialAllow = 0.00, LeaveAllow = 0.00, " +
               "ActingAllow = 0.00, ShiftAllow = 0.00, UniformAllow = 0.00, CooperativeDed = 0.00, " +
               "VoluntaryPension = 0.00, TransportAllow = 0.00, HousingAllow = 0.00, FurnitureAllow = 0.00," +
               " MealAllow = 0.00, UtilityAllow = 0.00, EducationAllow = 0.00, SecurityAllow = 0.00," +
               " MedicalAllow = 0.00, DomesticServantAllow = 0.00, DriverAllow = 0.00, VehicleAllow =0.00," +
               " HazardAllow = 0.00, Tax = 0.00, NHF = 0.00, JSA = 0.00, SSA = 0.00, TotalEarnings = 0.00, " +
               "TotalDeductions = 0.00, NetPay = 0.00, Pension = 0.00, TaxCalc = 'Automatic', Archived = 0, Arreas = 0.00, ExitCondition = '', EmployerPension = 0.00, EntertainmentAllow = 0.00, NewspaperAllow = 0.00, TaxAdjustment = 0.00 Where Id = @Id";

                db.SaveData("Insert Into Employees (EmployeeCode, PensionFundNumber, FirstName, LastName, TaxStateProvince, GradeLevelId, Category, DepartmentId, DateOfBirth, DateEngaged, BankCode, AccountNumber, Gender, MaritalStatus, JobTitleId, AccountName, PensionFundId, DateCreated, CreatedBy) " +
                    "values(@EmployeeCode, @PensionFundNumber, @FirstName, @LastName, @TaxStateProvince, @GradeLevelId, @Category, @DepartmentId, @DateOfBirth, @DateEngaged, @BankCode, @AccountNumber, @Gender, @MaritalStatus, @JobTitleId, @AccountName, @PensionFundId, @DateCreated, @CreatedBy)",
                    new { employee.EmployeeCode, employee.PensionFundNumber, employee.FirstName, employee.LastName, employee.TaxStateProvince, employee.GradeLevelId, employee.Category, employee.DepartmentId, employee.DateOfBirth, employee.DateEngaged, employee.BankCode, employee.AccountNumber, employee.Gender, employee.MaritalStatus, employee.JobTitleId, employee.AccountName, employee.PensionFundId, employee.DateCreated, employee.CreatedBy },
                    connectionStringName, false);

                int Id = db.LoadData<int, dynamic>("select Id from Employees where EmployeeCode = @EmployeeCode",
                    new { employee.EmployeeCode }, connectionStringName, false).First();

                db.SaveData(SQL,
                   new
                   {
                       Id
                   },
                   connectionStringName, false);

                repDb.UpdateEmployeesPayrollByEmpIdAsync(Id);


               
            }

        }
    }
}

﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using NddcPayrollLibrary.Data.Calculations.Allowance;
using NddcPayrollLibrary.Data.Calculations.Deductions;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Databases;
using NddcPayrollLibrary.Model.Company;
using NddcPayrollLibrary.Model.Employee;
using NddcPayrollLibrary.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Data.EmployeeData
{
    public class SQLEmployee : IEmployeeData
    {
        private readonly ISqlDataAccess db;
        private const string connectionStringName = "SqlDb";

        public SQLEmployee(ISqlDataAccess db)
        {
            this.db = db;
        }

        public bool EmployeeExists(string EmployeeCode)
        {
            string empCode = db.LoadData<string, dynamic>("select EmployeeCode from Employees where EmployeeCode = @EmployeeCode",
                new { EmployeeCode }, connectionStringName, false).FirstOrDefault();
            if (empCode == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public int AddEmployee(EmployeeModel Employee)
        {
            Employee.CreatedBy = "Admin";
            Employee.DateCreated = DateTime.Now;

            string SQL = "Update Employees Set BasicSalary = 0.00," +
               "Insurance = 0.00, SecretarialAllow = 0.00, LeaveAllow = 0.00, " +
               "ActingAllow = 0.00, ShiftAllow = 0.00, UniformAllow = 0.00, CooperativeDed = 0.00, " +
               "VoluntaryPension = 0.00, TransportAllow = 0.00, HousingAllow = 0.00, FurnitureAllow = 0.00," +
               " MealAllow = 0.00, UtilityAllow = 0.00, EducationAllow = 0.00, SecurityAllow = 0.00," +
               " MedicalAllow = 0.00, DomesticServantAllow = 0.00, DriverAllow = 0.00, VehicleAllow =0.00," +
               " HazardAllow = 0.00, Tax = 0.00, NHF = 0.00, JSA = 0.00, SSA = 0.00, TotalEarnings = 0.00, " +
               "TotalDeductions = 0.00, NetPay = 0.00, Pension = 0.00, TaxCalc = 'Automatic', Archived = 0, Arreas = 0.00, ExitCondition = '', EmployerPension = 0.00, EntertainmentAllow = 0.00, NewspaperAllow = 0.00, TaxAdjustment = 0.00, LoanPayment = 0.00 Where Id = @Id";

            db.SaveData("Insert Into Employees (EmployeeCode, Gender, MaritalStatus, FirstName, LastName, OtherNames, MaidenName," +
                " SpouseName, Email, Phone, DateOfBirth, Address, City, SID, Passport, EmploymentStatus, DateEngaged, ContactName, ContactPhone, CreatedBy, DateCreated) " +
                "values(@EmployeeCode, @Gender, @MaritalStatus, @FirstName, @LastName, @OtherNames, @MaidenName, @SpouseName, @Email, @Phone, " +
                "@DateOfBirth, @Address, @City, @SID, @Passport, @EmploymentStatus, @DateEngaged, @ContactName, @ContactPhone, @CreatedBy, @DateCreated)", 
                new { Employee.EmployeeCode, Employee.Gender, Employee.MaritalStatus, Employee.FirstName, Employee.LastName, Employee.OtherNames, 
                    Employee.MaidenName, Employee.SpouseName, Employee.Email, Employee.Phone, Employee.DateOfBirth, Employee.Address, Employee.City, 
                    Employee.SID, Employee.Passport, Employee.EmploymentStatus, Employee.DateEngaged, Employee.ContactName, Employee.ContactPhone, Employee.CreatedBy, Employee.DateCreated }, 
                connectionStringName, false);

            int Id = db.LoadData<int, dynamic>("select Id from Employees where EmployeeCode = @EmployeeCode",
                new { Employee.EmployeeCode }, connectionStringName, false).First();

            db.SaveData(SQL,
               new
               {
                   Id
               },
               connectionStringName, false);

           
            

            return Id;
        }
        public void UpdateEmployee(EmployeeModel employee)
        {
            employee.UpdatedBy = "Admin";
            employee.DateUpdated = DateTime.Now;
            //employee.DateOfBirth = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;
            //employee.DateEngaged = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;

            string SQL = "Update Employees Set EmployeeCode = @EmployeeCode, Gender = @Gender, MaritalStatus = @MaritalStatus, FirstName = @FirstName, LastName = @LastName, OtherNames = @OtherNames, MaidenName = @MaidenName, SpouseName = @SpouseName, Email = @Email, Phone = @Phone, DateOfBirth = @DateOfBirth, Address = @Address, City = @City, SID = @SID, Passport = @Passport, EmploymentStatus = @EmploymentStatus, DateEngaged = @DateEngaged, ContactName = @ContactName, ContactPhone = @ContactPhone, UpdatedBy = @UpdatedBy Where Id = @Id";
            db.SaveData(SQL, new { employee.EmployeeCode, employee.Gender, employee.MaritalStatus, employee.FirstName, employee.LastName, employee.OtherNames, employee.MaidenName, employee.SpouseName, employee.Email, employee.Phone, employee.DateOfBirth, employee.Address, employee.City, employee.SID, employee.Passport, employee.EmploymentStatus, employee.DateEngaged, employee.ContactName, employee.ContactPhone, employee.UpdatedBy, employee.Id }, connectionStringName, false);
        }
        public void DeleteEmployee(int Id)
        {
            db.SaveData("Delete Employees Where Id = @Id", new { Id }, connectionStringName, false);
        }

        public void ArchiveEmployee(int Id, string ExitConditon, DateTime ExitDate)
        {
            db.SaveData("Update Employees Set Archived = 1, ExitCondition = @ExitConditon, ExitDate = @ExitDate Where Id = @Id", new { Id, ExitConditon, ExitDate }, connectionStringName, false);
        }
        public void UnArchiveEmployee(int Id)
        {
            db.SaveData("Update Employees Set Archived = 0, ExitCondition = '' Where Id = @Id", new { Id }, connectionStringName, false);
        }
        public List<EmployeeGridModel> GetAllEmployees(string name)
        {
            string SQL = "SELECT Top 200 ROW_NUMBER() OVER (ORDER BY Employees.Id DESC) As SrNo, Employees.Id, Employees.EmployeeCode, Employees.Gender, Employees.FirstName, Employees.LastName, Employees.OtherNames, Employees.Email, Employees.Phone, Employees.Category, Employees.Archived, Employees.ExitCondition, Employees.ExitDate, GradeLevel.GradeLevel, (Departments.Code) As DepartmentName, JobTitles.Description FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = GradeLevel.Id LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id LEFT JOIN JobTitles ON Employees.JobTitleId = JobTitles.Id Where Employees.FirstName Like @Name Or Departments.DepartmentName Like @Name Or Employees.LastName Like @Name Or GradeLevel.GradeLevel Like @Name Or EmployeeCode Like @Name ORDER BY Employees.Id DESC";
            return db.LoadData<EmployeeGridModel, dynamic>(SQL, new { Name = "%" + name + "%"  }, connectionStringName, false).ToList();
        }
        public List<EmployeeGridModel> GetAllEmployeesAddedThisMonth()
        {
            DateTime month = DateTime.Now;
            string SQL = "SELECT ROW_NUMBER() OVER (ORDER BY Employees.Id DESC) As SrNo, Employees.Id, Employees.EmployeeCode, Employees.Gender, Employees.FirstName, Employees.LastName, Employees.Email, Employees.Phone, Employees.Category, Employees.Archived, Employees.ExitCondition, Employees.DateCreated, GradeLevel.GradeLevel, Departments.DepartmentName, JobTitles.Description FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = GradeLevel.Id LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id LEFT JOIN JobTitles ON Employees.JobTitleId = JobTitles.Id Where MONTH(Employees.DateCreated) = @Month And YEAR(Employees.DateCreated) = @Year ORDER BY Employees.Id DESC";
            return db.LoadData<EmployeeGridModel, dynamic>(SQL, new { Month = month.Month, Year = month.Year }, connectionStringName, false).ToList();
        }
        public int CountEmployeesAddedThisMonth()
        {
            DateTime month = DateTime.Now;
            string SQL = "select count(Id) from Employees where MONTH(DateCreated) = @Month  And YEAR(DateCreated) = @Year";
            return db.LoadData<int, dynamic>(SQL, new { Month = month.Month, Year = month.Year }, connectionStringName, false).FirstOrDefault();
        }
        public List<EmployeeGridModel> GetExpiringContract()
        {
            string SQL = "SELECT ROW_NUMBER() OVER (ORDER BY Employees.Id DESC) As SrNo, Employees.Id, Employees.EmployeeCode, Employees.Gender, Employees.FirstName, Employees.LastName, Employees.Email, Employees.Phone, Employees.Category, Employees.Archived, Employees.DateEngaged, DATEADD(month, 24, Employees.DateEngaged) AS ContExpiringDate, Employees.ExitCondition, Employees.DateCreated, GradeLevel.GradeLevel, Departments.DepartmentName, JobTitles.Description FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = GradeLevel.Id LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id LEFT JOIN JobTitles ON Employees.JobTitleId = JobTitles.Id Where Employees.Category = 'CONT' And DATEDIFF(month, Employees.DateEngaged, GETDATE()) >= 22 And DATEDIFF(month, Employees.DateEngaged, GETDATE()) <= 24 ORDER BY Employees.Id DESC";
            return db.LoadData<EmployeeGridModel, dynamic>(SQL, new { }, connectionStringName, false).ToList();
        }
        public int CountEmployeesExitedThisMonth()
        {
            DateTime month = DateTime.Now;
            string SQL = "select count(Id) from Employees where MONTH(ExitDate) = @Month  And YEAR(DateCreated) = @Year";
            return db.LoadData<int, dynamic>(SQL, new { Month = month.Month, Year = month.Year }, connectionStringName, false).FirstOrDefault();
        }
        public List<EmployeeGridModel> GetArchivedEmployees(string name)
        {
            string SQL = "SELECT Top 200 ROW_NUMBER() OVER (ORDER BY Employees.Id DESC) As SrNo, Employees.Id, Employees.EmployeeCode, Employees.Gender, Employees.FirstName, Employees.LastName, Employees.Email, Employees.Phone, Employees.Category, Employees.Archived, Employees.ExitCondition, Employees.ExitDate, GradeLevel.GradeLevel, (Departments.Code) As DepartmentName, JobTitles.Description FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = GradeLevel.Id LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id LEFT JOIN JobTitles ON Employees.JobTitleId = JobTitles.Id Where Archived = 1 ORDER BY Employees.Id DESC";
            return db.LoadData<EmployeeGridModel, dynamic>(SQL, new { Name = "%" + name + "%" }, connectionStringName, false).ToList();
        }

        public EmployeeModel GetEmployeeDetails(int EmpId)
        {
            //string SQL2 = "SELECT Employees.EmployeeCode, Employees.Gender, Employees.MaritalStatus, Employees.FirstName, Employees.LastName, Employees.OtherNames, Employees.SpouseName, Employees.Email, Employees.Phone, Employees.DateOfBirth, Employees.Address, Employees.City, Employees.SID, Employees.Passport, Employees.EmploymentStatus, Employees.DateEngaged, Employees.ContactName, Employees.ContactPhone, Employees.Category, GradeLevel.GradeLevel, Departments.DepartmentName, JobTitles.Description FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = GradeLevel.Id LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id LEFT JOIN JobTitles ON Employees.JobTitleId = JobTitles.Id Where Id = @Id";
            string SQL = "SELECT Id, EmployeeCode, Gender, MaritalStatus, FirstName, LastName, OtherNames, SpouseName, Email, Phone, DateOfBirth, Address, City, SID, Passport, EmploymentStatus, DateEngaged, ContactName, ContactPhone from Employees Where Id = @Id";
            return db.LoadData<EmployeeModel, dynamic>(SQL, new { Id = EmpId }, connectionStringName, false).First();
        }
        public MyPaySlipModel GetEmployeePaySlip(int empId, int payrollJournalId)
        {
            
            string SQL = "SELECT PayrollJournals.Id, PayrollJournals.FirstName, PayrollJournals.LastName, PayrollJournals.EmployeeCode, PayrollJournals.DepartmentId, PayrollJournals.PayPoint, JobTitles.Description, PayrollJournals.DateEngaged, PayrollJournals.AccountNumber, PayrollJournals.BankCode, PayrollJournals.TransportAllow, PayrollJournals.HousingAllow, PayrollJournals.FurnitureAllow, PayrollJournals.MealAllow, PayrollJournals.UtilityAllow, PayrollJournals.SecurityAllow, PayrollJournals.DomesticServantAllow, PayrollJournals.DriverAllow, PayrollJournals.MedicalAllow, PayrollJournals.EducationAllow, PayrollJournals.SecurityAllow, PayrollJournals.EntertainmentAllow, PayrollJournals.ActingAllow, PayrollJournals.ShiftAllow, PayrollJournals.UniformAllow, PayrollJournals.NewspaperAllow, PayrollJournals.DomesticServantAllow, PayrollJournals.DriverAllow, PayrollJournals.VehicleAllow As VehicleMaintenanceAllow, PayrollJournals.HazardAllow, PayrollJournals.Tax, PayrollJournals.NHF, PayrollJournals.JSA, PayrollJournals.LoanPayment, PayrollJournals.CooperativeDed, PayrollJournals.SSA, PayrollJournals.VoluntaryPension, PayrollJournals.Insurance, PayrollJournals.TotalEarnings, PayrollJournals.TotalDeductions, PayrollJournals.NetPay, PayrollJournals.BasicSalary, PayrollJournals.LeaveAllow, PayrollJournals.Arreas, PayrollJournals.Pension, PayrollJournalTitles.MonthYear As DateOfPayment, PayrollJournals.DepartmentName, PayrollJournals.GradeLevel As JobGrade, PayrollJournals.EmployeeId, JobTitles.Description As JobTitle FROM  PayrollJournals LEFT OUTER JOIN PayrollJournalTitles ON PayrollJournals.PayrollJournalTitleId = PayrollJournalTitles.Id LEFT OUTER JOIN JobTitles ON PayrollJournals.JobTitleId = JobTitles.Id WHERE PayrollJournals.EmployeeId = @empId AND PayrollJournals.PayrollJournalTitleId = @payrollJournalId";
            return db.LoadData<MyPaySlipModel, dynamic>(SQL, new { empId, payrollJournalId }, connectionStringName, false).FirstOrDefault();
        }
        public List<MyPaySlipModel> GetEmployeePaySlipByDept(int departmentId, int payrollJournalId)
        {

            string SQL = "SELECT PayrollJournals.Id, PayrollJournals.FirstName, PayrollJournals.LastName, PayrollJournals.EmployeeCode, PayrollJournals.DepartmentId, PayrollJournals.PayPoint, JobTitles.Description, PayrollJournals.DateEngaged, PayrollJournals.AccountNumber, PayrollJournals.BankCode, PayrollJournals.TransportAllow, PayrollJournals.HousingAllow, PayrollJournals.FurnitureAllow, PayrollJournals.MealAllow, PayrollJournals.UtilityAllow, PayrollJournals.SecurityAllow, PayrollJournals.DomesticServantAllow, PayrollJournals.DriverAllow, PayrollJournals.MedicalAllow, PayrollJournals.EducationAllow, PayrollJournals.SecurityAllow, PayrollJournals.SecretarialAllow, PayrollJournals.LeaveAllow, PayrollJournals.ActingAllow, PayrollJournals.ShiftAllow, PayrollJournals.UniformAllow, PayrollJournals.EntertainmentAllow, PayrollJournals.NewspaperAllow, PayrollJournals.DomesticServantAllow, PayrollJournals.DriverAllow, PayrollJournals.VehicleAllow As VehicleMaintenanceAllow, PayrollJournals.HazardAllow, PayrollJournals.Tax, PayrollJournals.NHF, PayrollJournals.JSA, PayrollJournals.CooperativeDed, PayrollJournals.SSA, PayrollJournals.VoluntaryPension, PayrollJournals.Insurance, PayrollJournals.LoanPayment, PayrollJournals.TotalEarnings, PayrollJournals.TotalDeductions, PayrollJournals.NetPay, PayrollJournals.BasicSalary, PayrollJournals.Pension, (PayrollJournalTitles.MonthYear) As DateOfPayment, PayrollJournals.DepartmentName, PayrollJournals.GradeLevel As JobGrade, PayrollJournals.EmployeeId, JobTitles.Description As JobTitle FROM  PayrollJournals LEFT OUTER JOIN PayrollJournalTitles ON PayrollJournals.PayrollJournalTitleId = PayrollJournalTitles.Id LEFT OUTER JOIN JobTitles ON PayrollJournals.JobTitleId = JobTitles.Id WHERE PayrollJournals.DepartmentId = @departmentId AND PayrollJournals.PayrollJournalTitleId = @payrollJournalId";
            return db.LoadData<MyPaySlipModel, dynamic>(SQL, new { departmentId , payrollJournalId }, connectionStringName, false).ToList();
        }
        public EmployeeModel GetAnalysisDetails(int EmpId)
        {
            //string SQL2 = "SELECT Employees.EmployeeCode, Employees.Gender, Employees.MaritalStatus, Employees.FirstName, Employees.LastName, Employees.OtherNames, Employees.SpouseName, Employees.Email, Employees.Phone, Employees.DateOfBirth, Employees.Address, Employees.City, Employees.SID, Employees.Passport, Employees.EmploymentStatus, Employees.DateEngaged, Employees.ContactName, Employees.ContactPhone, Employees.Category, GradeLevel.GradeLevel, Departments.DepartmentName, JobTitles.Description FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = GradeLevel.Id LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id LEFT JOIN JobTitles ON Employees.JobTitleId = JobTitles.Id Where Id = @Id";
            string SQL = "SELECT GradeLevelId, JobTitleId, Category, DepartmentId, PayPoint from Employees Where Id = @Id";
            return db.LoadData<EmployeeModel, dynamic>(SQL, new { Id = EmpId }, connectionStringName, false).First();
        }
        public decimal GetEmployeeInsurance(int EmpId)
        {
            string check = db.LoadData<string, dynamic>("SELECT Insurance from Employees Where Id = @Id", new { Id = EmpId }, connectionStringName, false).FirstOrDefault();
            if (check is null)
            {
                return 0.00M;
            }
            string SQL = "SELECT Insurance from Employees Where Id = @Id";
            return db.LoadData<decimal, dynamic>(SQL, new { Id = EmpId }, connectionStringName, false).First();
        }

        public int GetEmployeeCount()
        {
            //string check = db.LoadData<string, dynamic>("Select Count(Id) As EmpCount from Employees Where Archived = 0", new {  }, connectionStringName, false).FirstOrDefault();
            //if (check is null)
            //{
            //    return 0;
            //}
            string SQL = "Select Count(Id) As EmpCount from Employees Where Archived = 0";
            return db.LoadData<int, dynamic>(SQL, new {  }, connectionStringName, false).FirstOrDefault();
        }
        public bool GetPensionStatus(int EmpId)
        {
            string check = db.LoadData<string, dynamic>("SELECT PensionFundNumber from Employees Where Id = @Id", new { Id = EmpId }, connectionStringName, false).FirstOrDefault();
            if (check is null)
            {
                return false;
            }
            string SQL = "SELECT PensionFundNumber from Employees Where Id = @Id";
            string penNum = db.LoadData<string, dynamic>(SQL, new { Id = EmpId }, connectionStringName, false).First();
            if (!string.IsNullOrEmpty(penNum))
            {
                return true;
            }
            return false;
        }

        public void AddStatutoryDetails(MyStatutoryDetailsModel Employee)
        {
            Employee.DateCreated= DateTime.Now;

            string SQL = "update Employees set TaxStateProvince=@TaxStateProvince, TaxStatus=@TaxStatus, TaxOffice=@TaxOffice, TaxNumber=@TaxNumber, TaxStartDate=@TaxStartDate, NHFNumber=@NHFNumber, NHFStatus=@NHFStatus, " +
                "NHIS=@NHIS, NHISStatus=@NHISStatus, MedicalAidName=@MedicalAidName, MedicalAidNumber=@MedicalAidNumber, PensionFundId=@PensionFundId, PensionFundNumber=@PensionFundNumber, NSITFStatus=@NSITFStatus, ITFStatus=@ITFStatus where Id = @EmployeeId ";

            //db.SaveData("Insert Into StatutoryDetails (EmployeeId, SID, TaxStatus, TaxOffice, TaxNumber, TaxStartDate, NHFNumber, NHFStatus, " +
            //    "NHIS, NHISStatus, MedicalAidName, MedicalAidNumber, PensionFundId, PensionFundNumber, NSITFStatus, ITFStatus, CreatedBy, DateCreated) " +
            //    "values(@EmployeeId, @SID, @TaxStatus, @TaxOffice, @TaxNumber, @TaxStartDate, @NHFNumber, @NHFStatus, " +
            //    "@NHIS, @NHISStatus, @MedicalAidName, @MedicalAidNumber, @PensionFundId, @PensionFundNumber, @NSITFStatus, @ITFStatus, @CreatedBy, @DateCreated)",
            db.SaveData(SQL,
               new
                {
                    Employee.EmployeeId,
                    Employee.TaxStateProvince,
                    Employee.TaxStatus,
                    Employee.TaxOffice,
                    Employee.TaxNumber,
                    Employee.TaxStartDate,
                    Employee.NHFNumber,
                    Employee.NHFStatus,
                    Employee.NHIS,
                    Employee.NHISStatus,
                    Employee.MedicalAidName,
                    Employee.MedicalAidNumber,
                    Employee.PensionFundId,
                    Employee.PensionFundNumber,
                    Employee.NSITFSTatus,
                    Employee.ITFStatus
                },
                connectionStringName, false);

            
        }
        public MyStatutoryDetailsModel GetStatutoryDetails(int EmpId)
        {
            string SQL = "SELECT TaxStateProvince, TaxStatus, TaxOffice, TaxNumber, TaxStartDate, NHFNumber, NHFStatus, NHIS, NHISStatus," +
                " MedicalAidName, MedicalAidNumber, PensionFundId, PensionFundNumber, NSITFStatus, ITFStatus From Employees Where Id = @Id";
            return db.LoadData<MyStatutoryDetailsModel, dynamic>(SQL, new { Id = EmpId }, connectionStringName, false).First();
        }

        public void AddAnalysisDetails(MyAnalysisDetailsModel AnalysisDetails)
        {

            string SQL = "update Employees Set GradeLevelId = @GradeLevelId, JobTitleId = @JobTitleId, Category = @Category, DepartmentId = @DepartmentId, PayPoint = @PayPoint where Id = @EmployeeId";

            db.SaveData(SQL,
               new
               {
                   AnalysisDetails.GradeLevelId,
                   AnalysisDetails.JobTitleId,
                   AnalysisDetails.Category,
                   AnalysisDetails.DepartmentId,
                   AnalysisDetails.PayPoint,
                   AnalysisDetails.EmployeeId
               },
                connectionStringName, false);


        }

        public void UpdateAnalysisDetails(int JobTitleId, int DeptId, string Paypoint, int EmpId)
        {

            string SQL = "update Employees Set JobTitleId = @JobTitleId, DepartmentId = @DepartmentId, PayPoint = @PayPoint where Id = @EmployeeId";

            db.SaveData(SQL,
               new
               {
                   JobTitleId = JobTitleId, DepartmentId = DeptId, PayPoint = Paypoint, EmployeeId = EmpId
               },
                connectionStringName, false);


        }

        

        public void UpdateInsurance(string EmployeeCode, double Amount)
        {

            string SQL = "update Employees Set Insurance = @Insurance where EmployeeCode = @EmployeeCode";

            db.SaveData(SQL,
               new
               {
                   Insurance = Amount,
                   EmployeeCode = EmployeeCode
               },
                connectionStringName, false) ;


        }
        public void UpdateCooporative(string EmployeeCode, double cooporativeDed)
        {

            string SQL = "update Employees Set CooperativeDed = @CooperativeDed where EmployeeCode = @EmployeeCode";

            db.SaveData(SQL,
               new
               {
                   CooperativeDed = cooporativeDed,
                   EmployeeCode = EmployeeCode
               },
                connectionStringName, false);


        }
        public void UpdateLeaveAllowance(string EmployeeCode, double leaveAllowance)
        {

            string SQL = "Update Employees Set LeaveAllow = @LeaveAllow where EmployeeCode = @EmployeeCode";

            db.SaveData(SQL,
               new
               {
                   LeaveAllow = leaveAllowance,
                   EmployeeCode = EmployeeCode
               },
                connectionStringName, false);


        }
        public void UpdateNhf(string EmployeeCode, double nhfAmount)
        {

            string SQL = "update Employees Set NHF = @NHF where EmployeeCode = @EmployeeCode";

            db.SaveData(SQL,
               new
               {
                   NHF = nhfAmount,
                   EmployeeCode = EmployeeCode
               },
                connectionStringName, false);


        }
        public void UpdateSecretarialAllow(string EmployeeCode, string secretarialAllow)
        {

            string SQL = "update Employees Set SecretarialAllow = @SecretarialAllow where EmployeeCode = @EmployeeCode";

            db.SaveData(SQL,
               new
               {
                   SecretarialAllow = secretarialAllow,
                   EmployeeCode = EmployeeCode
               },
                connectionStringName, false);


        }
        public void UpdateVoluntaryPension(string EmployeeCode, string voluntaryPension)
        {

            string SQL = "update Employees Set VoluntaryPension = @VoluntaryPension where EmployeeCode = @EmployeeCode";

            db.SaveData(SQL,
               new
               {
                   Insurance = voluntaryPension,
                   EmployeeCode = EmployeeCode
               },
                connectionStringName, false);


        }
        public void UpdateEmployeePayroll(EmployeeModel employee)
        {

            string SQL = "update Employees Set TransportAllow = @TransportAllow, HousingAllow = @HousingAllow, FurnitureAllow = @FurnitureAllow, " +
                "MealAllow = @MealAllow, UtilityAllow = @UtilityAllow, EducationAllow = @EducationAllow, DomesticServantAllow = @DomesticServantAllow, " +
                "DriverAllow = @DriverAllow, VehicleAllow = @VehicleAllow, HazardAllow = @HazardAllow, Tax = @Tax, NHF = @NHF, JSA = @JSA, SSA = @SSA, " +
                "TotalEarnings = @TotalEarnings, Pension = @Pension, TotalDeductions = @TotalDeductions, NetPay = @NetPay, BasicSalary = @BasicSalary, MedicalAllow = @MedicalAllow, SecurityAllow = @SecurityAllow, EmployerPension = @EmployerPension, EntertainmentAllow = @EntertainmentAllow, NewspaperAllow = @NewspaperAllow where EmployeeCode = @EmployeeCode";

            db.SaveData(SQL,
               new
               {
                   EmployeeCode = employee.EmployeeCode,
                   TransportAllow = employee.TransportAllow,
                   HousingAllow = employee.HousingAllow,
                   FurnitureAllow = employee.FurnitureAllow,
                   MealAllow = employee.MealAllow,
                   UtilityAllow = employee.UtilityAllow,
                   EducationAllow = employee.EducationAllow,
                   DomesticServantAllow = employee.DomesticServantAllow,
                   DriverAllow = employee.DriverAllow,
                   VehicleAllow = employee.VehicleAllow,
                   HazardAllow = employee.HazardAllow,
                   Tax = employee.Tax,
                   NHF = employee.NHF,
                   JSA = employee.JSA,
                   SSA = employee.SSA,
                   TotalEarnings = employee.TotalEarnings,
                   TotalDeductions = employee.TotalDeductions,
                   NetPay = employee.NetPay,
                   BasicSalary = employee.BasicSalary,
                   MedicalAllow = employee.MedicalAllow,
                   SecurityAllow = employee.SecurityAllow,
                   Pension = employee.Pension,
                   EmployerPension = employee.EmployerPension,
                   EntertainmentAllow = employee.EntertainmentAllow,
                   NewspaperAllow = employee.NewspaperAllow
               },
                connectionStringName, false);


        }
        public EmployeeLinksModel GetEmployeeLinks(int EmpId)
        {
            string SQL = "SELECT Id, BasicSalary, GradeLevelId, JobTitleId, Category, Insurance, SecretarialAllow, LeaveAllow, ActingAllow, ShiftAllow, UniformAllow, CooperativeDed, VoluntaryPension, TransportAllow, HousingAllow, FurnitureAllow, MealAllow, UtilityAllow, EducationAllow, SecurityAllow, MedicalAllow, DomesticServantAllow, DriverAllow, VehicleAllow, HazardAllow, Tax, NHF, JSA, SSA, TotalEarnings, TotalDeductions, NetPay, Pension, TaxCalc, Arreas, EmployerPension, TaxAdjustment, EntertainmentAllow, NewspaperAllow, LoanPayment From Employees Where Id = @Id";
            return db.LoadData<EmployeeLinksModel, dynamic>(SQL, new { Id = EmpId }, connectionStringName, false).First();
        }

        public void UpdateForLinkCalc(int gradeLevelId, string category, int EmpId)
        {

            string SQL = "update Employees Set GradeLevelId = @GradeLevelId, Category = @Category where Id = @EmpId";

            db.SaveData(SQL,
               new
               {
                   GradeLevelId = gradeLevelId,
                   Category = category,
                   EmpId = EmpId
               },
                connectionStringName, false);


        }
        public void SaveEmployeeLink(EmployeeLinksModel employee)
        {
            string SQL = "Update Employees Set BasicSalary = @BasicSalary," +
                "Insurance = @Insurance, SecretarialAllow = @SecretarialAllow, LeaveAllow = @LeaveAllow, " +
                "ActingAllow = @ActingAllow, ShiftAllow = @ShiftAllow, UniformAllow = @UniformAllow, CooperativeDed = @CooperativeDed, " +
                "VoluntaryPension = @VoluntaryPension, TransportAllow = @TransportAllow, HousingAllow = @HousingAllow, FurnitureAllow = @FurnitureAllow," +
                " MealAllow = @MealAllow, UtilityAllow = @UtilityAllow, EducationAllow = @EducationAllow, SecurityAllow = @SecurityAllow," +
                " MedicalAllow = @MedicalAllow, DomesticServantAllow = @DomesticServantAllow, DriverAllow = @DriverAllow, VehicleAllow =@VehicleAllow," +
                " HazardAllow = @HazardAllow, Tax = @Tax, NHF = @NHF, JSA = @JSA, SSA = @SSA, TotalEarnings = @TotalEarnings, " +
                "TotalDeductions = @TotalDeductions, NetPay = @NetPay, Pension = @Pension, TaxCalc = @TaxCalc, Arreas = @Arreas, EmployerPension = @EmployerPension, TaxAdjustment = @TaxAdjustment, NewspaperAllow = @NewspaperAllow, EntertainmentAllow = @EntertainmentAllow, LoanPayment = @LoanPayment Where Id = @Id";
            db.SaveData(SQL,
                new
                {
                    employee.Id,
                    employee.BasicSalary,
                    employee.Insurance,
                    employee.SecretarialAllow,
                    employee.LeaveAllow,
                    employee.ActingAllow,
                    employee.ShiftAllow,
                    employee.UniformAllow,
                    employee.CooperativeDed,
                    employee.VoluntaryPension,
                    employee.TransportAllow,
                    employee.HousingAllow,
                    employee.FurnitureAllow,
                    employee.MealAllow,
                    employee.UtilityAllow,
                    employee.EducationAllow,
                    employee.SecurityAllow,
                    employee.MedicalAllow,
                    employee.DomesticServantAllow,
                    employee.DriverAllow,
                    employee.VehicleAllow,
                    employee.HazardAllow,
                    employee.Tax,
                    employee.NHF,
                    employee.JSA,
                    employee.SSA,
                    employee.TotalEarnings,
                    employee.TotalDeductions,
                    employee.NetPay,
                    employee.Pension,
                    employee.TaxCalc,
                    employee.Arreas,
                    employee.EmployerPension,
                    employee.TaxAdjustment,
                    employee.NewspaperAllow,
                    employee.EntertainmentAllow,
                    employee.LoanPayment
                },
                 connectionStringName, false);
        }

    }
}

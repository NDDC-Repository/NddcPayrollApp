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
               "TotalDeductions = 0.00, NetPay = 0.00, Pension = 0.00, TaxCalc = 'Automatic', Archived = 0, Arreas = 0.00, ExitCondition = '', EmployerPension = 0.00, EntertainmentAllow = 0.00, NewspaperAllow = 0.00, TaxAdjustment = 0.00 Where Id = @Id";

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
            db.SaveData(SQL, new { employee.EmployeeCode, employee.Gender, employee.MaritalStatus, employee.FirstName, employee.LastName, employee.OtherNames, employee.MaidenName, employee.SpouseName, employee.Email, employee.Phone, employee.DateOfBirth, employee.Address, employee.City, employee.SID, employee.Passport, employee.EmploymentStatus, employee.DateEngaged, employee.ContactName, employee.ContactPhone, employee.UpdatedBy }, connectionStringName, false);
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
            string SQL = "SELECT Top 20 ROW_NUMBER() OVER (ORDER BY Employees.Id DESC) As SrNo, Employees.Id, Employees.EmployeeCode, Employees.Gender, Employees.FirstName, Employees.LastName, Employees.Email, Employees.Phone, Employees.Category, Employees.Archived, Employees.ExitCondition, GradeLevel.GradeLevel, Departments.DepartmentName, JobTitles.Description FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = GradeLevel.Id LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id LEFT JOIN JobTitles ON Employees.JobTitleId = JobTitles.Id Where Employees.FirstName Like @Name Or Departments.DepartmentName Like @Name Or Employees.LastName Like @Name Or GradeLevel.GradeLevel Like @Name ORDER BY Employees.Id DESC";
            return db.LoadData<EmployeeGridModel, dynamic>(SQL, new { Name = "%" + name + "%"  }, connectionStringName, false).ToList();
        }
        public List<EmployeeGridModel> GetArchivedEmployees(string name)
        {
            string SQL = "SELECT Top 20 ROW_NUMBER() OVER (ORDER BY Employees.Id DESC) As SrNo, Employees.Id, Employees.EmployeeCode, Employees.Gender, Employees.FirstName, Employees.LastName, Employees.Email, Employees.Phone, Employees.Category, Employees.Archived, Employees.ExitCondition, GradeLevel.GradeLevel, Departments.DepartmentName, JobTitles.Description FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = GradeLevel.Id LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id LEFT JOIN JobTitles ON Employees.JobTitleId = JobTitles.Id Where Archived = 1 ORDER BY Employees.Id DESC";
            return db.LoadData<EmployeeGridModel, dynamic>(SQL, new { Name = "%" + name + "%" }, connectionStringName, false).ToList();
        }

        public EmployeeModel GetEmployeeDetails(int EmpId)
        {
            //string SQL2 = "SELECT Employees.EmployeeCode, Employees.Gender, Employees.MaritalStatus, Employees.FirstName, Employees.LastName, Employees.OtherNames, Employees.SpouseName, Employees.Email, Employees.Phone, Employees.DateOfBirth, Employees.Address, Employees.City, Employees.SID, Employees.Passport, Employees.EmploymentStatus, Employees.DateEngaged, Employees.ContactName, Employees.ContactPhone, Employees.Category, GradeLevel.GradeLevel, Departments.DepartmentName, JobTitles.Description FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = GradeLevel.Id LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id LEFT JOIN JobTitles ON Employees.JobTitleId = JobTitles.Id Where Id = @Id";
            string SQL = "SELECT Id, EmployeeCode, Gender, MaritalStatus, FirstName, LastName, OtherNames, SpouseName, Email, Phone, DateOfBirth, Address, City, SID, Passport, EmploymentStatus, DateEngaged, ContactName, ContactPhone from Employees Where Id = @Id";
            return db.LoadData<EmployeeModel, dynamic>(SQL, new { Id = EmpId }, connectionStringName, false).First();
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
            string check = db.LoadData<string, dynamic>("SELECT Count(Id) As EmpCount from Employees Where Archived = 0", new {  }, connectionStringName, false).FirstOrDefault();
            if (check is null)
            {
                return 0;
            }
            string SQL = "SELECT Count(Id) As EmpCount from Employees";
            return db.LoadData<int, dynamic>(SQL, new {  }, connectionStringName, false).First();
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

        public void UpdateInsurance(string EmployeeCode, string Amount)
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
        public void UpdateCooporative(string EmployeeCode, string cooporativeDed)
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
            string SQL = "SELECT Id, BasicSalary, GradeLevelId, JobTitleId, Category, Insurance, SecretarialAllow, LeaveAllow, ActingAllow, ShiftAllow, UniformAllow, CooperativeDed, VoluntaryPension, TransportAllow, HousingAllow, FurnitureAllow, MealAllow, UtilityAllow, EducationAllow, SecurityAllow, MedicalAllow, DomesticServantAllow, DriverAllow, VehicleAllow, HazardAllow, Tax, NHF, JSA, SSA, TotalEarnings, TotalDeductions, NetPay, Pension, TaxCalc, Arreas, EmployerPension, TaxAdjustment, EntertainmentAllow, NewspaperAllow From Employees Where Id = @Id";
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
                "TotalDeductions = @TotalDeductions, NetPay = @NetPay, Pension = @Pension, TaxCalc = @TaxCalc, Arreas = @Arreas, EmployerPension = @EmployerPension, TaxAdjustment = @TaxAdjustment, NewspaperAllow = @NewspaperAllow, EntertainmentAllow = @EntertainmentAllow Where Id = @Id";
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
                    employee.EntertainmentAllow
                },
                 connectionStringName, false);
        }

    }
}

using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Databases;
using NddcPayrollLibrary.Model.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Data.PayrollJournal
{
    public class SQLPayrollJournal : IPayrollJournalData
    {
        private const string connectionStringName = "SqlDb";
        private readonly ISqlDataAccess db;
        private readonly IReportsData repDb;

        public SQLPayrollJournal(ISqlDataAccess db, IReportsData repDb)
        {
            this.db = db;
            this.repDb = repDb;
        }
        public void AddPayrollJournal(MyPayrollJournalTitleModel JournalTitle)
        {
            DateTime DateCreated = DateTime.Now;
            string CreatedBy = "Admin";
            List<MyPayrollJournalModel> JournalList = new List<MyPayrollJournalModel>();

            int Month = JournalTitle.CurrentPeriod.Month;
            string MonthYear = JournalTitle.CurrentPeriod.ToString("Y");

            db.SaveData("Insert Into PayrollJournalTitles (JournalName, Month, MonthYear, CurrentPeriod, DateCreated, CreatedBy) values(@JournalName, @Month, @MonthYear, @CurrentPeriod, @DateCreated, @CreatedBy)", new { JournalTitle.JournalName, Month, MonthYear, JournalTitle.CurrentPeriod, DateCreated, CreatedBy }, connectionStringName, false);

            int Id = db.LoadData<int, dynamic>("select Id from PayrollJournalTitles where JournalName = @JournalName",
                new { JournalTitle.JournalName }, connectionStringName, false).First();

            string SQL = "Select Id, EmployeeCode, Gender, MaritalStatus, FirstName, LastName, OtherNames, MaidenName, SpouseName, Email, Phone, DateOfBirth, Address, City, SID, EmploymentStatus, DateEngaged, ContactName, ContactPhone, TaxStateProvince, TaxStatus, DateEngaged, ContactName, ContactPhone, TaxStateProvince, TaxStatus, TaxOffice, TaxNumber, TaxStartDate, NHFNumber, NHFStatus, NHIS, NHISStatus, MedicalAidName, MedicalAidNumber, PensionFundId, PensionFundNumber, NSITFStatus, ITFStatus, PaymentMethod, BankCode, AccountNumber, AccountName, PayInfo1, PayInfo2, SortCode, GradeLevelId, JobTitleId, Category, DepartmentId, PayPoint, CreatedBy, DateCreated, Insurance, SecretarialAllow, CooperativeDed, VoluntaryPension, TransportAllow, HousingAllow, FurnitureAllow, MealAllow, UtilityAllow, EducationAllow, SecurityAllow, MedicalAllow, DomesticServantAllow, DriverAllow, VehicleAllow, HazardAllow, Tax, NHF, JSA, SSA, TotalEarnings, TotalDeductions, NetPay, BasicSalary, MonthlyGross, Pension, LeaveAllow, ActingAllow, ShiftAllow, UniformAllow, TaxCalc, Arreas, EmployerPension, TaxAdjustment, EntertainmentAllow, NewspaperAllow From Employees Where Archived = 0";

            JournalList = db.LoadData<MyPayrollJournalModel, dynamic>(SQL, new { }, connectionStringName, false).ToList();

            string SQL2 = "Insert Into PayrollJournals (EmployeeId, Gender, MaritalStatus, FirstName, LastName, DateOfBirth, DateEngaged, TaxStateProvince, TaxOffice, TaxStartDate, PensionFundId, PensionFundNumber, EmployeeCode, BankCode, BankName, AccountNumber, GradeLevelId, JobTitleId, Category, DepartmentId, PayPoint, CreatedBy, Insurance, SecretarialAllow, CooperativeDed, VoluntaryPension, TransportAllow, HousingAllow, FurnitureAllow, MealAllow, UtilityAllow, EducationAllow, SecurityAllow, MedicalAllow, DomesticServantAllow, DriverAllow, VehicleAllow, HazardAllow, Tax, NHF, JSA, SSA, TotalEarnings, TotalDeductions, NetPay, BasicSalary, MonthlyGross, Pension, LeaveAllow, ActingAllow, ShiftAllow, UniformAllow, TaxCalc, Arreas, EmployerPension, TaxAdjustment, PayrollJournalTitleId, JournalName, Month, MonthYear, CurrentPeriod, EntertainmentAllow, NewspaperAllow, CategoryName, GradeLevel, DepartmentName) values(@EmployeeId, @Gender, @MaritalStatus, @FirstName, @LastName, @DateOfBirth, @DateEngaged, @TaxStateProvince, @TaxOffice, @TaxStartDate, @PensionFundId, @PensionFundNumber, @EmployeeCode, @BankCode, @BankName, @AccountNumber, @GradeLevelId, @JobTitleId, @Category, @DepartmentId, @PayPoint, @CreatedBy, @Insurance, @SecretarialAllow, @CooperativeDed, @VoluntaryPension, @TransportAllow, @HousingAllow, @FurnitureAllow, @MealAllow, @UtilityAllow, @EducationAllow, @SecurityAllow, @MedicalAllow, @DomesticServantAllow, @DriverAllow, @VehicleAllow, @HazardAllow, @Tax, @NHF, @JSA, @SSA, @TotalEarnings, @TotalDeductions, @NetPay, @BasicSalary, @MonthlyGross, @Pension, @LeaveAllow, @ActingAllow, @ShiftAllow, @UniformAllow, @TaxCalc, @Arreas, @EmployerPension, @TaxAdjustment, @PayrollJournalTitleId, @JournalName, @Month, @MonthYear, @CurrentPeriod, @EntertainmentAllow, @NewspaperAllow, @CategoryName, @GradeLevel, @DepartmentName)";
            //string SQL2 = "Insert Into PayrollJournals (EmployeeCode, Gender, MaritalStatus) values(@EmployeeCode, @Gender, @MaritalStatus)";

            string gradeLevel = "";
            string departmentName = "";
            string categoryName = "";
            string bankName = "";
            foreach (var item in JournalList)
            {
                gradeLevel = GetGradeLevel(item.GradeLevelId);
                departmentName = GetDeptName(item.DepartmentId);
                categoryName = GetCategory(item.Id);
                bankName = GetBankName(item.BankCode);

                db.SaveData(SQL2, new { EmployeeId = item.Id, Gender = item.Gender, MaritalStatus = item.MaritalStatus, FirstName = item.FirstName, LastName = item.LastName, DateOfBirth = item.DateOfBirth, DateEngaged = item.DateEngaged, TaxStateProvince = item.TaxStateProvince, TaxOffice = item.TaxOffice, TaxStartDate = item.TaxStartDate, PensionFundId = item.PensionFundId, PensionFundNumber = item.PensionFundNumber, EmployeeCode = item.EmployeeCode, BankCode = item.BankCode, BankName = bankName, AccountNumber = item.AccountNumber, GradeLevelId = item.GradeLevelId, JobTitleId = item.JobTitleId, Category = item.Category, DepartmentId = item.DepartmentId, PayPoint = item.PayPoint, CreatedBy = item.CreatedBy, DateCreated = DateCreated, Insurance = item.Insurance, SecretarialAllow = item.SecretarialAllow, CooperativeDed = item.CooperativeDed, VoluntaryPension = item.VoluntaryPension, TransportAllow = item.TransportAllow, HousingAllow = item.HousingAllow, FurnitureAllow = item.FurnitureAllow, MealAllow = item.MealAllow, UtilityAllow = item.UtilityAllow, EducationAllow = item.EducationAllow, SecurityAllow = item.SecurityAllow, MedicalAllow = item.MedicalAllow, DomesticServantAllow = item.DomesticServantAllow, DriverAllow = item.DriverAllow, VehicleAllow = item.VehicleAllow, HazardAllow = item.HazardAllow, Tax = item.Tax, NHF = item.NHF, JSA = item.JSA, SSA = item.SSA, TotalEarnings = item.TotalEarnings, TotalDeductions = item.TotalDeductions, NetPay = item.NetPay, BasicSalary = item.BasicSalary, MonthlyGross = item.MonthlyGross, Pension = item.Pension, LeaveAllow = item.LeaveAllow, ActingAllow = item.ActingAllow, ShiftAllow = item.ShiftAllow, UniformAllow = item.UniformAllow, TaxCalc = item.TaxCalc, Arreas = item.Arreas, EmployerPension = item.EmployerPension, TaxAdjustment = item.TaxAdjustment, PayrollJournalTitleId = Id, JournalName = JournalTitle.JournalName, Month = Month, MonthYear = MonthYear, CurrentPeriod = JournalTitle.CurrentPeriod, EntertainmentAllow = item.EntertainmentAllow, NewspaperAllow = item.NewspaperAllow, CategoryName = categoryName, GradeLevel = gradeLevel, DepartmentName = departmentName }, connectionStringName, false);
                //db.SaveData(SQL2, new { EmployeeCode = item.EmployeeCode,nkCode = item.BankCode, AccountNumber = item.AccountNumber, AccountName = item.AccountName, PayInfo1 = item.PayInfo1, PayInfo2 = item.PayInfo2, SortCode = item.SortCode, GradeLevelId = item.GradeLevelId, JobTitleId = item.JobTitleId, Category = item.Category, DepartmentId = item.DepartmentId, PayPoint = item.PayPoint, CreatedBy = item.CreatedBy, DateCreated = item.DateCreated, Insurance = item.Insurance, SecretarialAllow = item.SecretarialAllow, CooperativeDed = item.CooperativeDed, VoluntaryPension = item.VoluntaryPension, TransportAllow = item.TransportAllow, HousingAllow = item.HousingAllow, FurnintureAllow = item.FurnitureAllow, MealAllow = item.MealAllow, UtilityAllow = item.UtilityAllow, EducationAllow = item.EducationAllow, SecurityAllow = item.SecurityAllow, MedicalAllow = item.MedicalAllow, DomesticServantAllow = item.DomesticServantAllow, DriverAllow = item.DriverAllow, VehicleAllow = item.VehicleAllow, HazardAllow = item.HazardAllow, Tax = item.Tax, NHF = item.NHF, JSA = item.JSA, SSA = item.SSA, TotalEarnings = item.TotalEarnings, TotalDeductions = item.TotalDeductions, NetPay = item.NetPay, BasicSalary = item.BasicSalary, MonthlyGross = item.MonthlyGross, Pension = item.Pension, LeaveAllow = item.LeaveAllow, ActingAllow = item.ActingAllow, ShiftAllow = item.ShiftAllow, UniformAllow = item.UniformAllow, TaxCalc = item.TaxCalc, Arreas = item.Arreas, EmployerPension = item.EmployerPension, TaxAdjustment = item.TaxAdjustment, PayrollJournalTitleId = Id, JournalName = JournalTitle.JournalName, Month = Month, MonthYear = MonthYear, CurrentPeriod = JournalTitle.CurrentPeriod, JournalDate = JournalTitle.DateCreated }, connectionStringName, false);
                
                //if (item.Arreas > 0.00M)
                //{
                //    db.SaveData("Update Employees Set Arreas = 0.00 Where EmployeeCode = @EmployeeCode", new { item.EmployeeCode }, connectionStringName, false);
                //    repDb.UpdateEmployeesPayrollByEmpIdAsync(item.Id);
                //}
                //if (item.LeaveAllow > 0.00M)
                //{
                //    db.SaveData("Update Employees Set LeaveAllow = 0.00 Where EmployeeCode = @EmployeeCode", new { item.EmployeeCode }, connectionStringName, false);
                //    repDb.UpdateEmployeesPayrollByEmpIdAsync(item.Id);
                //}
            }
        }

        private string GetCategory(int EmpId)
        {
            return db.LoadData<string, dynamic>("select Category from Employees where Id = @EmpId",
                new { EmpId }, connectionStringName, false).First();
        }
        private string GetGradeLevel(int GradeLevelId)
        {
            return db.LoadData<string, dynamic>("select GradeLevel from GradeLevel where Id = @GradeLevelId",
                new { GradeLevelId }, connectionStringName, false).First();
        }
        private string GetDeptName(int DeptId)
        {
            return db.LoadData<string, dynamic>("select DepartmentName from Departments where Id = @DeptId",
                new { DeptId }, connectionStringName, false).First();
        }
        private string GetBankName(string Code)
        {
            return db.LoadData<string, dynamic>("select BankName from Banks where Code = @Code",
                new { Code }, connectionStringName, false).First();
        }

        public List<MyPayrollJournalTitleModel> GetPayrollJournalTitles()
        {
            return db.LoadData<MyPayrollJournalTitleModel, dynamic>("Select Top 12 ROW_NUMBER() OVER (ORDER BY Id DESC) As SrNo,  Id, JournalName, MonthYear, DateCreated From PayrollJournalTitles Order By Id DESC",
                new { }, connectionStringName, false);

        }

        public void DeleteExecutedPayroll(int Id)
        {
            db.SaveData("Delete PayrollJournals Where PayrollJournalTitleId = @Id", new { Id }, connectionStringName, false);
            db.SaveData("Delete PayrollJournalTitles Where Id = @Id", new { Id }, connectionStringName, false);
        }

    }
}

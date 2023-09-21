using NddcPayrollLibrary.Data.Calculations.Allowance;
using NddcPayrollLibrary.Data.Helper;
using NddcPayrollLibrary.Data.Reports;
using NddcPayrollLibrary.Databases;
using NddcPayrollLibrary.Model.Employee;
using NddcPayrollLibrary.Model.Enums;
using NddcPayrollLibrary.Model.Payroll;
using NddcPayrollLibrary.Model.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Data.Payroll
{
    public class SQLPayroll : IPayrollData
    {

        private const string connectionStringName = "SqlDb";
        private readonly ISqlDataAccess db;
        private readonly IAllowanceData allowDb;
        private readonly IHelperData helpDb;

        public SQLPayroll(ISqlDataAccess db, IAllowanceData allowDb, IHelperData helpDb)
        {
            this.db = db;
            this.allowDb = allowDb;
            this.helpDb = helpDb;
        }
        public void AddSalaryScale(SalaryScale scale)
        {
            db.SaveData("Insert Into SalaryScale (GradeLevel, Category, BasicSalary) values(@GradeLevel, @Category, @BasicSalary)", new { scale.GradeLevel, scale.Category, scale.BasicSalary }, connectionStringName, false);
        }

        public List<SalaryScale> GetAllSalaryScale()
        {
            return db.LoadData<SalaryScale, dynamic>("select ROW_NUMBER() OVER (ORDER BY Id ASC) As SrNo, Id, GradeLevel, Category, BasicSalary from SalaryScale", new { }, connectionStringName, false).ToList();
        }

        public void AddBenefit(MyBenefitsModel Benefit)
        {
            db.SaveData("Insert Into Benefits (GradeLevelID, BenefitTypeId, Percentage, Cycle) values(@GradeLevelID, @BenefitTypeId, @Percentage, @Cycle)", new { Benefit.GradeLevelID, Benefit.BenefitTypeId, Benefit.Percentage, Benefit.Cycle }, connectionStringName, false);
        }
        public void UpdateBenefit(MyBenefitsModel Benefit)
        {
            db.SaveData("Update Benefits Set BenefitTypeId = @BenefitTypeId, Percentage = @Percentage, Cycle = @Cycle Where Id = @Id", new { Benefit.BenefitTypeId, Benefit.Percentage, Benefit.Cycle, Benefit.Id }, connectionStringName, false);
        }
        public void DeleteBenefit(int Id)
        {
            db.SaveData("Delete Benefits Where Id = @Id", new { Id }, connectionStringName, false);
        }
        public List<MyBenefitsModel> GetBenefitsById(int GradeLevelID)
        {
            return db.LoadData<MyBenefitsModel, dynamic>("SELECT ROW_NUMBER() OVER (ORDER BY Benefits.Id ASC) As SrNo, Benefits.Id, Benefits.Cycle, Benefits.Percentage, BenefitType.BenefitType FROM  Benefits LEFT JOIN BenefitType ON Benefits.BenefitTypeId = BenefitType.Id Where Benefits.GradeLevelID = @GradeLevelID", new { GradeLevelID = GradeLevelID }, connectionStringName, false);
        }
        public MyBenefitsModel GetBenefitsByBenefitId(int benefitId)
        {
            return db.LoadData<MyBenefitsModel, dynamic>("SELECT Id, BenefitTypeId, GradeLevelId, Percentage, Cycle FROM  Benefits Where Id = @Id", new { Id = benefitId }, connectionStringName, false).First();
        }
        public IEnumerable<SalaryScale> GetGradeLevels()
        {
            return db.LoadData<SalaryScale, dynamic>("select ROW_NUMBER() OVER (ORDER BY Id ASC) As SrNo, Id, GradeLevel from SalaryScale", new { }, connectionStringName, false);
        }
        public List<MyBenefitsTypeModel> GetBenefitTypes()
        {
            return db.LoadData<MyBenefitsTypeModel, dynamic>("Select Id, BenefitType From BenefitType", new {  }, connectionStringName, false);
        }
        public List<MyLinkedBenefitsModel> GetLinkedBenefits(int gradeLevelId)
        {
            return db.LoadData<MyLinkedBenefitsModel, dynamic>("SELECT LinkedBenefits.Id, GradeLevel.GradeLevel, GradeLevel.Description, GradeLevel.BasicSalary, LinkedBenefits.MultiplyBy, GradeLevel.BasicSalary * LinkedBenefits.MultiplyBy AS Amount, BenefitType.BenefitType FROM  LinkedBenefits INNER JOIN BenefitType ON LinkedBenefits.BenefitTypeId = BenefitType.Id LEFT OUTER JOIN GradeLevel ON LinkedBenefits.LinkedGradeLevelId = GradeLevel.Id WHERE LinkedBenefits.GradeLevelId = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false);
        }
        public MyLinkedBenefitsModel GetLinkedBenefitByLinkBenefitId(int linkedBenefitId)
        {
            return db.LoadData<MyLinkedBenefitsModel, dynamic>("SELECT Id, GradeLevelId, LinkedGradeLevelId, BenefitTypeId, MultiplyBy, Amount, Cycle  FROM  LinkedBenefits Where Id = @Id", new { Id = linkedBenefitId }, connectionStringName, false).First();
        }
        public void UpdateLinkedBenefit(MyLinkedBenefitsModel LinkedBenefit)
        {
            db.SaveData("Update LinkedBenefits Set LinkedGradeLevelId = @LinkedGradeLevelId, BenefitTypeId = @BenefitTypeId, MultiplyBy = @MultiplyBy, Cycle = @Cycle Where Id = @Id", new { LinkedBenefit.Id, LinkedBenefit.LinkedGradeLevelId, LinkedBenefit.BenefitTypeId, LinkedBenefit.MultiplyBy, LinkedBenefit.Amount, LinkedBenefit.Cycle }, connectionStringName, false);
        }
        public void DeleteLinkedBenefit(int Id)
        {
            db.SaveData("Delete linkedBenefits Where Id = @Id", new { Id }, connectionStringName, false);
        }
        public void AddLinkedBenefits(MyLinkedBenefitsModel LinkBenefit)
        {
            db.SaveData("Insert Into LinkedBenefits (GradeLevelId, LinkedGradeLevelId, BenefitTypeId, MultiplyBy, Cycle) values(@GradeLevelId, @LinkedGradeLevelId, @BenefitTypeId, @MultiplyBy, @Cycle)", new { LinkBenefit.GradeLevelId, LinkBenefit.LinkedGradeLevelId, LinkBenefit.BenefitTypeId, LinkBenefit.MultiplyBy, LinkBenefit.Cycle }, connectionStringName, false);
        }
        public List<MySubsidiesModel> GetSubsidies(int gradeLevelId)
        {
            return db.LoadData<MySubsidiesModel, dynamic>("Select ROW_NUMBER() OVER (ORDER BY Id ASC) As SrNo, Id, SubsidyType, Amount, Cycle From Subsidies Where GradeLevelId = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false);
        }
        public MySubsidiesModel GetSubsidyBySubsId(int id)
        {
            return db.LoadData<MySubsidiesModel, dynamic>("Select Id, SubsidyType, Amount, Cycle, GradeLevelId From Subsidies Where Id = @Id", new { Id = id }, connectionStringName, false).FirstOrDefault();
        }
        public void AddSubsidies(MySubsidiesModel Subsidy)
        {
            db.SaveData("Insert Into Subsidies (SubsidyType, Amount, Cycle, GradeLevelId) values(@SubsidyType, @Amount, @Cycle, @GradeLevelId)", new { Subsidy.SubsidyType, Subsidy.Amount, Subsidy.Cycle, Subsidy.GradeLevelId }, connectionStringName, false);
        }
        public void UpdateSubsidy(MySubsidiesModel subsidy)
        {
            db.SaveData("Update Subsidies Set SubsidyType = @SubsidyType, Amount = @Amount, Cycle = @Cycle Where Id = @Id", new { SubsidyType = subsidy.SubsidyType, Amount = subsidy.Amount, Cycle = subsidy.Cycle, Id = subsidy.Id }, connectionStringName, false);
        }

        //Benefits Calculations
        private int GetGradeLevelId(int EmpId)
        {
            string check = db.LoadData<string, dynamic>("Select GradeLevelId From Employees Where Id = @Id", new { Id = EmpId }, connectionStringName, false).First();
            if (check == null)
            {
                return 0;
            }
            return db.LoadData<int, dynamic>("Select GradeLevelId From Employees Where Id = @Id", new { Id = EmpId }, connectionStringName, false).First();
        }
        private decimal GetArreas(int EmpId)
        {
            string check = db.LoadData<string, dynamic>("Select Arreas From Employees Where Id = @Id", new { Id = EmpId }, connectionStringName, false).First();
            if (check == null)
            {
                return 0.00M;
            }
            return db.LoadData<int, dynamic>("Select Arreas From Employees Where Id = @Id", new { Id = EmpId }, connectionStringName, false).First();
        }
        private decimal GetLeaveAllowance(int EmpId)
        {
            string check = db.LoadData<string, dynamic>("Select LeaveAllow From Employees Where Id = @Id", new { Id = EmpId }, connectionStringName, false).First();
            if (check == null)
            {
                return 0.00M;
            }
            return db.LoadData<int, dynamic>("Select LeaveAllow From Employees Where Id = @Id", new { Id = EmpId }, connectionStringName, false).First();
        }
        private decimal GetActingAllowance(int EmpId)
        {
            string check = db.LoadData<string, dynamic>("Select ActingAllow From Employees Where Id = @Id", new { Id = EmpId }, connectionStringName, false).FirstOrDefault();
            if (check == null)
            {
                return 0.00M;
            }
            return db.LoadData<int, dynamic>("Select ActingAllow From Employees Where Id = @Id", new { Id = EmpId }, connectionStringName, false).FirstOrDefault();
        }
        public decimal GetBasicSalary(int EmpId)
        {
            string check = string.Empty;
            string empCategory = GetEmployeeCategory(EmpId);
            if (empCategory == "PERM" || empCategory == "POLI" || empCategory == "MD" || empCategory == "EDP" || empCategory == "EDFA")
            {
                int GradeLevelId = GetGradeLevelId(EmpId);
                //check = db.LoadData<string, dynamic>("Select BasicSalary From GradeLevel Where Id = @Id", new { Id = GradeLevelId }, connectionStringName, false).First();
                //if (GradeLevelId == 0)
                //{
                //    return 0.00M;
                //}
                decimal basicSalary = db.LoadData<decimal, dynamic>("Select BasicSalary From GradeLevel Where Id = @Id", new { Id = GradeLevelId }, connectionStringName, false).First();
                return basicSalary;
            }
            else if (empCategory == "CONT")
            {
                int GradeLevelId = GetGradeLevelId(EmpId);
                check = db.LoadData<string, dynamic>("Select BasicSalary From GradeLevel Where Id = @Id", new { Id = GradeLevelId }, connectionStringName, false).First();
                if (GradeLevelId == 0)
                {
                    return 0.00M;
                }
                decimal basicSalary = db.LoadData<decimal, dynamic>("Select BasicSalary From GradeLevel Where Id = @Id", new { Id = GradeLevelId }, connectionStringName, false).First();
                return (80M/100M) * basicSalary;
            }
            else
            {
                return 0.00M;
            }
            
        }
        private int GetBenefitPercentage(int gradeLevelId)
        {
            string check = db.LoadData<string, dynamic>("Select Sum(Percentage) As Benefit From Benefits Where GradeLevelId = @GradeLevelId And Cycle = @Cycle", new { GradeLevelId = gradeLevelId, Cycle = "Monthly" }, connectionStringName, false).First();
            if (check is null)
            {
                return 0;
            }
            return db.LoadData<int, dynamic>("Select Sum(Percentage) As Benefit From Benefits Where GradeLevelId = @GradeLevelId And Cycle = @Cycle", new { GradeLevelId = gradeLevelId, Cycle = "Monthly" }, connectionStringName, false).First();
        }
        private int GetGrossBenefitPercentage(int gradeLevelId)
        {
            string check = db.LoadData<string, dynamic>("Select Sum(Percentage) As Benefit From Benefits Where GradeLevelId = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false).First();
            if (check is null)
            {
                return 0;
            }
            return db.LoadData<int, dynamic>("Select Sum(Percentage) As Benefit From Benefits Where GradeLevelId = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false).First();
        }
        public decimal GetLinkedBenefitsAmount(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);

            //use the method below to return 0.00 if the querry return null
            string check = db.LoadData<string, dynamic>("SELECT Sum(GradeLevel.BasicSalary * LinkedBenefits.MultiplyBy) FROM  LinkedBenefits LEFT JOIN GradeLevel ON LinkedBenefits.LinkedGradeLevelId = GradeLevel.Id WHERE LinkedBenefits.GradeLevelId = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();
            if (check is null)
            {
                return 0.00M;
            }
            decimal linkedMonthlyGross = db.LoadData<decimal, dynamic>("SELECT Sum(GradeLevel.MonthlyGross * LinkedBenefits.MultiplyBy) FROM  LinkedBenefits INNER JOIN GradeLevel ON LinkedBenefits.LinkedGradeLevelId = GradeLevel.Id WHERE LinkedBenefits.GradeLevelId = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false).First();
            //decimal annualBasic = linkedBasicSalary * 12;
            //int linkedGradeLevelId = db.LoadData<int, dynamic>("SELECT LinkedBenefits.LinkedGradeLevelId FROM  LinkedBenefits INNER JOIN GradeLevel ON LinkedBenefits.LinkedGradeLevelId = GradeLevel.Id WHERE LinkedBenefits.GradeLevelId = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();
            //decimal grossBenefitsPercenatage = GetGrossBenefitPercentage(linkedGradeLevelId);
            //decimal grossBenefitAmount = ((decimal)grossBenefitsPercenatage / (decimal)100) * (annualBasic);
            //decimal totalLinkedBenefit = (linkedBasicSalary) + (grossBenefitAmount / 12) + ((decimal)13500);
           
            return linkedMonthlyGross;
            
            
        }
        public decimal GetSubsidyAmount(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);

            string check = db.LoadData<string, dynamic>("SELECT Sum(Amount * 30) As Amount From Subsidies Where GradeLevelId = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();
            if (check is null)
            {
                return 0.00M;
            }
            return db.LoadData<decimal, dynamic>("SELECT Sum(Amount * 30) As Amount From Subsidies Where GradeLevelId = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();
        }
        private string GetEmployeeCategory(int empId)
        {
            return db.LoadData<string, dynamic>("Select Category From Employees Where Id = @Id", new { Id = empId }, connectionStringName, false).FirstOrDefault();
        }
        public decimal GetBenefits(int empId)
        {
            string staffCategory = db.LoadData<string, dynamic>("Select Category From Employees Where Id = @Id", new { Id = empId }, connectionStringName, false).FirstOrDefault();
            if (staffCategory == "PERM" || staffCategory == "POLI" || staffCategory == "MD" || staffCategory == "EDP" || staffCategory == "EDFA")
            {
                decimal linkedBenefits = 0.00M;
                int gradeLevelId = GetGradeLevelId(empId);
                int benefitPercentage = GetBenefitPercentage(gradeLevelId);
                decimal basicSalary = db.LoadData<decimal, dynamic>("Select BasicSalary From GradeLevel Where Id = @Id", new { Id = gradeLevelId }, connectionStringName, false).First();
                decimal annualBasic = basicSalary * 12;
                decimal benefitAmount = ((decimal)benefitPercentage / (decimal)100) * (annualBasic);
                decimal percentageBenefits = benefitAmount / 12;
                linkedBenefits = GetLinkedBenefitsAmount(empId);
                decimal subsidyAmount = GetSubsidyAmount(empId);
                decimal secretarialAllow = allowDb.GetSecretarialAllowance(empId);

                decimal totalBenefits = percentageBenefits + linkedBenefits + subsidyAmount + secretarialAllow;
                return totalBenefits;
            }
            else if (staffCategory == "CONT")
            {
                decimal housing = allowDb.GetHousingAllowance(empId);
                decimal furniture = allowDb.GetFurnitureAllowance(empId);
                decimal transport = allowDb.GetTransportAllowance(empId);
                decimal meal = allowDb.GetMealAllowance(empId);
                decimal utility = allowDb.GetUtilityAllowance(empId);
                decimal education = allowDb.GetEducationAllowance(empId);
                decimal security = allowDb.GetSecurityAllowance(empId);
                decimal domestic = allowDb.GetDomesticServantAllowance(empId);
                decimal medical = allowDb.GetMedicalAllowance(empId);
                decimal driver = allowDb.GetDriverAllowance(empId);
                decimal vehicle = allowDb.GetVehicleMaintenanceAllowance(empId);
                decimal hazard = allowDb.GetHazardAllowance(empId);
                decimal totalBenefits = (housing + furniture + transport + meal + utility + education + security + domestic + medical + driver
                    + vehicle + hazard);
                return totalBenefits;

                //decimal linkedBenefits = 0.00M;
                //int gradeLevelId = GetGradeLevelId(empId);
                //int benefitPercentage = GetBenefitPercentage(gradeLevelId);
                ////decimal basicSalary = db.LoadData<decimal, dynamic>("Select BasicSalary From GradeLevel Where Id = @Id", new { Id = gradeLevelId }, connectionStringName, false).First();
                //decimal basicSalary = GetBasicSalary(empId);
                //decimal annualBasic = basicSalary * 12;
                //decimal benefitAmount = ((decimal)benefitPercentage / (decimal)100) * (annualBasic);
                //decimal percentageBenefits = benefitAmount / 12;
                //linkedBenefits = GetLinkedBenefitsAmount(empId);
                //decimal subsidyAmount = GetSubsidyAmount(empId);

                //decimal totalBenefits = percentageBenefits + linkedBenefits + subsidyAmount;
                //return totalBenefits;
            }

            return 0.00M;
            
        }
        public decimal GetMonthlyGross(int empId)
        {
            decimal arreas = GetArreas(empId);
            decimal leaveAllow = GetLeaveAllowance(empId);
            decimal actingAllow = GetActingAllowance(empId);
            decimal basicSalary = GetBasicSalary(empId);
            decimal benefitsAmount = GetBenefits(empId);
            decimal monthlyGross = (basicSalary + benefitsAmount + arreas + leaveAllow);
            return monthlyGross;
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

            string SQL = "Select Id, EmployeeCode, Gender, MaritalStatus, FirstName, LastName, OtherNames, MaidenName, SpouseName, Email, Phone, DateOfBirth, Address, City, SID, EmploymentStatus, DateEngaged, ContactName, ContactPhone, TaxStateProvince, TaxStatus, DateEngaged, ContactName, ContactPhone, TaxStateProvince, TaxStatus, TaxOffice, TaxNumber, TaxStartDate, NHFNumber, NHFStatus, NHIS, NHISStatus, MedicalAidName, MedicalAidNumber, PensionFundId, PensionFundNumber, NSITFStatus, ITFStatus, PaymentMethod, BankCode, AccountNumber, AccountName, PayInfo1, PayInfo2, SortCode, GradeLevelId, JobTitleId, Category, DepartmentId, PayPoint, CreatedBy, DateCreated, Insurance, SecretarialAllow, CooperativeDed, VoluntaryPension, TransportAllow, HousingAllow, FurnitureAllow, MealAllow, UtilityAllow, EducationAllow, SecurityAllow, MedicalAllow, DomesticServantAllow, DriverAllow, VehicleAllow, HazardAllow, Tax, NHF, JSA, SSA, TotalEarnings, TotalDeductions, NetPay, BasicSalary, MonthlyGross, Pension, LeaveAllow, ActingAllow, ShiftAllow, UniformAllow, TaxCalc, Arreas, EmployerPension, TaxAdjustment From Employees Where Archived = 0";

            JournalList = db.LoadData<MyPayrollJournalModel, dynamic>(SQL, new { }, connectionStringName, false).ToList();

            string SQL2 = "Insert Into PayrollJournals (EmployeeId, Gender, MaritalStatus, FirstName, LastName, DateOfBirth, DateEngaged, TaxStateProvince, TaxOffice, TaxStartDate, PensionFundId, PensionFundNumber, EmployeeCode, BankCode, AccountNumber, GradeLevelId, JobTitleId, Category, DepartmentId, PayPoint, CreatedBy, Insurance, SecretarialAllow, CooperativeDed, VoluntaryPension, TransportAllow, HousingAllow, FurnitureAllow, MealAllow, UtilityAllow, EducationAllow, SecurityAllow, MedicalAllow, DomesticServantAllow, DriverAllow, VehicleAllow, HazardAllow, Tax, NHF, JSA, SSA, TotalEarnings, TotalDeductions, NetPay, BasicSalary, MonthlyGross, Pension, LeaveAllow, ActingAllow, ShiftAllow, UniformAllow, TaxCalc, Arreas, EmployerPension, TaxAdjustment, PayrollJournalTitleId, JournalName, Month, MonthYear, CurrentPeriod) values(@EmployeeId, @Gender, @MaritalStatus, @FirstName, @LastName, @DateOfBirth, @DateEngaged, @TaxStateProvince, @TaxOffice, @TaxStartDate, @PensionFundId, @PensionFundNumber, @EmployeeCode, @BankCode, @AccountNumber, @GradeLevelId, @JobTitleId, @Category, @DepartmentId, @PayPoint, @CreatedBy, @Insurance, @SecretarialAllow, @CooperativeDed, @VoluntaryPension, @TransportAllow, @HousingAllow, @FurnitureAllow, @MealAllow, @UtilityAllow, @EducationAllow, @SecurityAllow, @MedicalAllow, @DomesticServantAllow, @DriverAllow, @VehicleAllow, @HazardAllow, @Tax, @NHF, @JSA, @SSA, @TotalEarnings, @TotalDeductions, @NetPay, @BasicSalary, @MonthlyGross, @Pension, @LeaveAllow, @ActingAllow, @ShiftAllow, @UniformAllow, @TaxCalc, @Arreas, @EmployerPension, @TaxAdjustment, @PayrollJournalTitleId, @JournalName, @Month, @MonthYear, @CurrentPeriod)";
            //string SQL2 = "Insert Into PayrollJournals (EmployeeCode, Gender, MaritalStatus) values(@EmployeeCode, @Gender, @MaritalStatus)";

            foreach (var item in JournalList)
            {
                db.SaveData(SQL2, new { EmployeeId = item.Id, Gender = item.Gender, MaritalStatus = item.MaritalStatus, FirstName = item.FirstName , LastName = item.LastName, DateOfBirth = item.DateOfBirth, DateEngaged = item.DateEngaged, TaxStateProvince = item.TaxStateProvince, TaxOffice = item.TaxOffice, TaxStartDate = item.TaxStartDate, PensionFundId = item.PensionFundId, PensionFundNumber = item.PensionFundNumber, EmployeeCode = item.EmployeeCode, BankCode = item.BankCode, AccountNumber = item.AccountNumber, GradeLevelId = item.GradeLevelId, JobTitleId = item.JobTitleId, Category = item.Category, DepartmentId = item.DepartmentId, PayPoint = item.PayPoint, CreatedBy = item.CreatedBy, DateCreated = DateCreated, Insurance = item.Insurance, SecretarialAllow = item.SecretarialAllow, CooperativeDed = item.CooperativeDed, VoluntaryPension = item.VoluntaryPension, TransportAllow = item.TransportAllow, HousingAllow = item.HousingAllow, FurnitureAllow = item.FurnitureAllow, MealAllow = item.MealAllow, UtilityAllow = item.UtilityAllow, EducationAllow = item.EducationAllow, SecurityAllow = item.SecurityAllow, MedicalAllow = item.MedicalAllow, DomesticServantAllow = item.DomesticServantAllow, DriverAllow = item.DriverAllow, VehicleAllow = item.VehicleAllow, HazardAllow = item.HazardAllow, Tax = item.Tax, NHF = item.NHF, JSA = item.JSA, SSA = item.SSA, TotalEarnings = item.TotalEarnings, TotalDeductions = item.TotalDeductions, NetPay = item.NetPay, BasicSalary = item.BasicSalary, MonthlyGross = item.MonthlyGross, Pension = item.Pension, LeaveAllow = item.LeaveAllow, ActingAllow = item.ActingAllow, ShiftAllow = item.ShiftAllow, UniformAllow = item.UniformAllow, TaxCalc = item.TaxCalc, Arreas = item.Arreas, EmployerPension = item.EmployerPension, TaxAdjustment = item.TaxAdjustment, PayrollJournalTitleId = Id, JournalName = JournalTitle.JournalName, Month = Month, MonthYear = MonthYear, CurrentPeriod = JournalTitle.CurrentPeriod }, connectionStringName, false);
                //db.SaveData(SQL2, new { EmployeeCode = item.EmployeeCode,nkCode = item.BankCode, AccountNumber = item.AccountNumber, AccountName = item.AccountName, PayInfo1 = item.PayInfo1, PayInfo2 = item.PayInfo2, SortCode = item.SortCode, GradeLevelId = item.GradeLevelId, JobTitleId = item.JobTitleId, Category = item.Category, DepartmentId = item.DepartmentId, PayPoint = item.PayPoint, CreatedBy = item.CreatedBy, DateCreated = item.DateCreated, Insurance = item.Insurance, SecretarialAllow = item.SecretarialAllow, CooperativeDed = item.CooperativeDed, VoluntaryPension = item.VoluntaryPension, TransportAllow = item.TransportAllow, HousingAllow = item.HousingAllow, FurnintureAllow = item.FurnitureAllow, MealAllow = item.MealAllow, UtilityAllow = item.UtilityAllow, EducationAllow = item.EducationAllow, SecurityAllow = item.SecurityAllow, MedicalAllow = item.MedicalAllow, DomesticServantAllow = item.DomesticServantAllow, DriverAllow = item.DriverAllow, VehicleAllow = item.VehicleAllow, HazardAllow = item.HazardAllow, Tax = item.Tax, NHF = item.NHF, JSA = item.JSA, SSA = item.SSA, TotalEarnings = item.TotalEarnings, TotalDeductions = item.TotalDeductions, NetPay = item.NetPay, BasicSalary = item.BasicSalary, MonthlyGross = item.MonthlyGross, Pension = item.Pension, LeaveAllow = item.LeaveAllow, ActingAllow = item.ActingAllow, ShiftAllow = item.ShiftAllow, UniformAllow = item.UniformAllow, TaxCalc = item.TaxCalc, Arreas = item.Arreas, EmployerPension = item.EmployerPension, TaxAdjustment = item.TaxAdjustment, PayrollJournalTitleId = Id, JournalName = JournalTitle.JournalName, Month = Month, MonthYear = MonthYear, CurrentPeriod = JournalTitle.CurrentPeriod, JournalDate = JournalTitle.DateCreated }, connectionStringName, false);
                if (item.Arreas > 0M)
                {
                    db.SaveData("Update Employees Set Arreas = 0.00 Where EmployeeCode = @EmployeeCode", new { item.EmployeeCode }, connectionStringName, false);
                    //helpDb.ExecuteUpdateForEmployee(item.EmployeeId);
                }
            }
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

       public decimal GetSumOfTotalEarnings()
        {
            return db.LoadData<decimal, dynamic>("select sum(TotalEarnings) from Employees where Archived = 0",
               new { }, connectionStringName, false).FirstOrDefault();
        }
        public decimal GetSumOfNetPay()
        {
            return db.LoadData<decimal, dynamic>("select sum(NetPay) from Employees where Archived = 0",
               new { }, connectionStringName, false).FirstOrDefault();
        }
        public decimal GetSumOfTax()
        {
            return db.LoadData<decimal, dynamic>("select sum(Tax) from Employees where Archived = 0",
               new { }, connectionStringName, false).FirstOrDefault();
        }
        public decimal GetSumOfCooperative()
        {
            return db.LoadData<decimal, dynamic>("select sum(CooperativeDed) from Employees where Archived = 0",
               new { }, connectionStringName, false).FirstOrDefault();
        }
        public decimal GetSumOfBasicSalary()
        {
            return db.LoadData<decimal, dynamic>("select sum(BasicSalary) from Employees where Archived = 0",
               new { }, connectionStringName, false).FirstOrDefault();
        }

        public void AddArrears(int EmpId)
        {
            int daysCount = db.LoadData<int, dynamic>("select DATEDIFF(day, DateEngaged, GETDATE()) from Employees Where Id = @Id",
               new { Id = EmpId }, connectionStringName, false).FirstOrDefault();
            decimal basicSalary = GetMonthlyGross(EmpId);
            decimal dailyPay = basicSalary / 30M;
            
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int remainingDaysInMonth = daysInMonth - DateTime.Today.Day;
            int totalDaysCount = daysCount + remainingDaysInMonth;
            int arrearsDays = (totalDaysCount - daysInMonth) - 2;

            decimal arrearsAmount = dailyPay * arrearsDays;

            db.SaveData("Update Employees Set Arreas = @Arreas Where Id = @Id", new { Arreas = arrearsAmount, Id = EmpId }, connectionStringName, false);

        }
        public void InsertArrearsAmount(double ArreasAmount, string EmpCode)
        {
            db.SaveData("Update Employees Set Arreas = @Arreas Where EmployeeCode = @EmployeeCode", new { Arreas = ArreasAmount, EmployeeCode = EmpCode }, connectionStringName, false);
        }
        public void UpdateGradeLevel(int gradeLevelId, string EmpCode)
        {
            db.SaveData("Update Employees Set GradeLevelId = @GradeLevelId Where EmployeeCode = @EmployeeCode", new { GradeLevelId = gradeLevelId, EmployeeCode = EmpCode }, connectionStringName, false);
        }
    }
}
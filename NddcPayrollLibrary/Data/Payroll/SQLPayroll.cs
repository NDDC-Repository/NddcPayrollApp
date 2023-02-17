using NddcPayrollLibrary.Data.Calculations.Allowance;
using NddcPayrollLibrary.Databases;
using NddcPayrollLibrary.Model.Employee;
using NddcPayrollLibrary.Model.Enums;
using NddcPayrollLibrary.Model.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Data.Payroll
{
    public class SQLPayroll : IPayrollData
    {

        private const string connectionStringName = "SqlDb";
        private readonly ISqlDataAccess db;
        private readonly IAllowanceData allowDb;

        public SQLPayroll(ISqlDataAccess db, IAllowanceData allowDb)
        {
            this.db = db;
            this.allowDb = allowDb;
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
        public List<MyBenefitsModel> GetBenefitsById(int GradeLevelID)
        {
            return db.LoadData<MyBenefitsModel, dynamic>("SELECT ROW_NUMBER() OVER (ORDER BY Benefits.Id ASC) As SrNo, Benefits.Id, Benefits.Cycle, Benefits.Percentage, BenefitType.BenefitType FROM  Benefits LEFT JOIN BenefitType ON Benefits.BenefitTypeId = BenefitType.Id Where Benefits.GradeLevelID = @GradeLevelID", new { GradeLevelID = GradeLevelID }, connectionStringName, false);
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
        public void AddLinkedBenefits(MyLinkedBenefitsModel LinkBenefit)
        {
            db.SaveData("Insert Into LinkedBenefits (GradeLevelId, LinkedGradeLevelId, BenefitTypeId, MultiplyBy, Cycle) values(@GradeLevelId, @LinkedGradeLevelId, @BenefitTypeId, @MultiplyBy, @Cycle)", new { LinkBenefit.GradeLevelId, LinkBenefit.LinkedGradeLevelId, LinkBenefit.BenefitTypeId, LinkBenefit.MultiplyBy, LinkBenefit.Cycle }, connectionStringName, false);
        }
        public List<MySubsidiesModel> GetSubsidies(int gradeLevelId)
        {
            return db.LoadData<MySubsidiesModel, dynamic>("Select ROW_NUMBER() OVER (ORDER BY Id ASC) As SrNo, Id, SubsidyType, Amount, Cycle From Subsidies Where GradeLevelId = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false);
        }
        public void AddSubsidies(MySubsidiesModel Subsidy)
        {
            db.SaveData("Insert Into Subsidies (SubsidyType, Amount, Cycle, GradeLevelId) values(@SubsidyType, @Amount, @Cycle, @GradeLevelId)", new { Subsidy.SubsidyType, Subsidy.Amount, Subsidy.Cycle, Subsidy.GradeLevelId }, connectionStringName, false);
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
        public decimal GetBasicSalary(int EmpId)
        {
            string check = string.Empty;
            string empCategory = GetEmployeeCategory(EmpId);
            if (empCategory == "PERM")
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
            if (staffCategory == "PERM")
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

                decimal totalBenefits = percentageBenefits + linkedBenefits + subsidyAmount;
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
            decimal basicSalary = GetBasicSalary(empId);
            decimal benefitsAmount = GetBenefits(empId);
            decimal monthlyGross = (basicSalary + benefitsAmount);
            return monthlyGross;
        }
    }
}
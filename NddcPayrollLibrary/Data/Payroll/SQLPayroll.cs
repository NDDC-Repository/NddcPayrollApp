using NddcPayrollLibrary.Databases;
using NddcPayrollLibrary.Model.Employee;
using NddcPayrollLibrary.Model.Enums;
using NddcPayrollLibrary.Model.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Data.Payroll
{
    public class SQLPayroll : IPayrollData
    {

        private const string connectionStringName = "SqlDb";
        private readonly ISqlDataAccess db;

        public SQLPayroll(ISqlDataAccess db)
        {
            this.db = db;
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
            return db.LoadData<int, dynamic>("Select GradeLevelId From Employees Where Id = @Id", new { Id = EmpId }, connectionStringName, false).First();
        }
        public double GetBasicSalary(int EmpId)
        {
            int GradeLevelId = GetGradeLevelId(EmpId);
            return db.LoadData<double, dynamic>("Select BasicSalary From GradeLevel Where Id = @Id", new { Id = GradeLevelId }, connectionStringName, false).First();
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
        public double GetLinkedBenefitsAmount(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);
           //use the method below to return 0.00 if the querry return null
            string check = db.LoadData<string, dynamic>("SELECT Sum(GradeLevel.BasicSalary * LinkedBenefits.MultiplyBy) FROM  LinkedBenefits INNER JOIN GradeLevel ON LinkedBenefits.LinkedGradeLevelId = GradeLevel.Id WHERE LinkedBenefits.GradeLevelId = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();
            if (check is null)
            {
                return 0.00;
            }
            var result = db.LoadData<double, dynamic>("SELECT Sum(GradeLevel.BasicSalary * LinkedBenefits.MultiplyBy) FROM  LinkedBenefits INNER JOIN GradeLevel ON LinkedBenefits.LinkedGradeLevelId = GradeLevel.Id WHERE LinkedBenefits.GradeLevelId = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();
           
            return result;
        }
        public double GetSubsidyAmount(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);

            string check = db.LoadData<string, dynamic>("SELECT Sum(Amount * 30) As Amount From Subsidies Where GradeLevelId = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();
            if (check is null)
            {
                return 0.00;
            }
            return db.LoadData<double, dynamic>("SELECT Sum(Amount * 30) As Amount From Subsidies Where GradeLevelId = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();
        }
        public double GetBenefits(int empId)
        {
            double linkedBenefits = 0.00;
            int gradeLevelId = GetGradeLevelId(empId);
            int benefitPercentage = GetBenefitPercentage(gradeLevelId);
            double basicSalary = db.LoadData<double, dynamic>("Select BasicSalary From GradeLevel Where Id = @Id", new { Id = gradeLevelId }, connectionStringName, false).First();
            double annualBasic = basicSalary * 12;
            double benefitAmount = ((double)benefitPercentage / (double)100) * (annualBasic);
            double percentageBenefits = benefitAmount / 12;
            linkedBenefits = GetLinkedBenefitsAmount(empId);
            double subsidyAmount = GetSubsidyAmount(empId);

            double totalBenefits = percentageBenefits + linkedBenefits + subsidyAmount;
            return totalBenefits;
        }
        public double GetMonthlyGross(int empId)
        {
            double basicSalary = GetBasicSalary(empId);
            double benefitsAmount = GetBenefits(empId);
            double monthlyGross = (basicSalary + benefitsAmount);
            return monthlyGross;
        }
    }
}
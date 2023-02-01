using NddcPayrollLibrary.Databases;
using NddcPayrollLibrary.Model.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Data
{
    public class SqlData : IDatabaseData
    {
        private readonly ISqlDataAccess db;
        private const string connectionStringName = "SqlDb";

        public SqlData(ISqlDataAccess db)
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
            db.SaveData("Insert Into Benefits (SalaryScaleId, Benefit, Percentage) values(@SalaryScaleId, @Benefit, @Percentage)", new { Benefit.GradeLevelID, Benefit.BenefitTypeId, Benefit.Percentage }, connectionStringName, false);
        }
        public List<MyBenefitsModel> GetBenefitsById(int SalaryScaleId)
        {
            return db.LoadData<MyBenefitsModel, dynamic>("select ROW_NUMBER() OVER (ORDER BY Id ASC) As SrNo, Id, Benefit, Percentage from Benefits where SalaryScaleId = @SalaryScaleId", new { SalaryScaleId = SalaryScaleId }, connectionStringName, false);
        }

        public IEnumerable<SalaryScale> GetGradeLevels()
        {
            return db.LoadData<SalaryScale, dynamic>("select ROW_NUMBER() OVER (ORDER BY Id ASC) As SrNo, Id, GradeLevel from SalaryScale", new { }, connectionStringName, false);
        }
    }
}
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Data.Calculations.Deductions
{
    public class SQLDeduction : IDeductionData
    {
        private readonly ISqlDataAccess db;
        private readonly IPayrollData payDb;
        private const string connectionStringName = "SqlDb";

        public SQLDeduction(ISqlDataAccess db, IPayrollData payDb)
        {
            this.db = db;
            this.payDb = payDb;
        }

        private int GetGradeLevelId(int EmpId)
        {
            return db.LoadData<int, dynamic>("Select GradeLevelId From Employees Where Id = @Id", new { Id = EmpId }, connectionStringName, false).First();
        }

        public decimal GetMonthlyGross(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);
            return db.LoadData<int, dynamic>("Select MonthlyGross From GradeLevel Where Id = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false).First();
        }
        public decimal GetBasicSalary(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);
            return db.LoadData<int, dynamic>("Select BasicSalary From GradeLevel Where Id = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false).First();
        }
        public decimal GetPensionAmount(int empId)
        {
            decimal totalEarnings = GetMonthlyGross(empId) * 12;
            decimal pensionAmount = ((decimal)8 / (decimal)100) * totalEarnings;
            return pensionAmount;
        }
        public decimal GetNHFAmount(int empId)
        {
            decimal annualBasic = GetBasicSalary(empId) * 12;
            decimal NHFAmount = ((decimal)2.5 / (decimal)100) * annualBasic;
            return NHFAmount;
        }
        public decimal ApplyStateRelief(decimal totalEarnings)
        {
            decimal firstStateReliefAmount = 0.00M;
            decimal secondStateReliefAmount = 0.00M;

            decimal firstStateReliefValue = ((decimal)1 / (decimal)100) * totalEarnings;
            if (firstStateReliefValue > 200000M)
            {
                firstStateReliefAmount = firstStateReliefValue;
            }
            else if (firstStateReliefValue <= 200000M)
            {
                firstStateReliefAmount = 200000.00M;
            }

            secondStateReliefAmount = ((decimal)20 / (decimal)100) * totalEarnings;
            decimal totalStateRelief = firstStateReliefAmount + secondStateReliefAmount;

            return totalStateRelief;

        }
        public decimal ApplyCompanyRelief(int empId)
        {
            //decimal totalEarnings = payDb.GetMonthlyGross(empId) * 12;
            //decimal pensionAmount = ((decimal)8 / (decimal)100) * totalEarnings;
            //decimal annualBasic = GetBasicSalary(empId) * 12;
            //decimal NHFAmount = ((decimal)2.5 / (decimal)100) * annualBasic;

            decimal pensionAmount = GetPensionAmount(empId);
            decimal NHFAmount = GetNHFAmount(empId);

            return pensionAmount + NHFAmount;
        }
        private decimal GetCRATotal(int empId)
        {
            decimal relief = ApplyCompanyRelief(empId);
            decimal originalEarnings = payDb.GetMonthlyGross(empId) * 12M;
            return originalEarnings - relief;
        }
        public decimal GetPAYEAmount(int empId)
        {
            decimal OriginalTotalEarnings = payDb.GetMonthlyGross(empId) * (decimal)12;
            decimal craTotalEarnings = GetCRATotal(empId);
            decimal stateReliefAmount = ApplyStateRelief(craTotalEarnings);
            decimal companyReliefAmount = ApplyCompanyRelief(empId);
            decimal totalRelief = stateReliefAmount + companyReliefAmount;
            decimal taxabaleIncome = OriginalTotalEarnings - totalRelief;

            decimal levelTax = taxabaleIncome;
            decimal taxValue = 0.00M;
            //decimal finalLevel = 0.00M;

            decimal finalAmount = 0.00M;

            if (levelTax >= 300000.00M)
            {
                taxValue = 21000.00M;
                levelTax = levelTax - 300000.00M;
            }
            if (levelTax >= 300000.00M)
            {
                taxValue = taxValue + 33000.00M;
                levelTax = levelTax - 300000.00M;

            }
           if (levelTax >= 500000.00M)
            {
                taxValue = taxValue + 75000.00M;
                levelTax = levelTax - 500000.00M;
            }
            if (levelTax >= 500000.00M)
            {
                taxValue = taxValue + 95000.00M;
                levelTax = levelTax - 500000.00M;
            }
            if (levelTax >= 1600000.00M)
            {
                taxValue = taxValue + 336000.00M;
                levelTax = levelTax - 1600000.00M;
            }
            if (levelTax >= 3200000.00M)
            {
                finalAmount = ((decimal)24 / (decimal)100) * levelTax;
            }

            decimal totalTax = (finalAmount + taxValue) / (decimal)12;

            return totalTax;
        }
    }
}

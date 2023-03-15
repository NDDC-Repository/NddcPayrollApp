using NddcPayrollLibrary.Data.EmployeeData;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Databases;
using NddcPayrollLibrary.Model.Enums;
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
        private readonly IEmployeeData empDb;
        private const string connectionStringName = "SqlDb";

        public SQLDeduction(ISqlDataAccess db, IPayrollData payDb, IEmployeeData empDb)
        {
            this.db = db;
            this.payDb = payDb;
            this.empDb = empDb;
        }

        private int GetGradeLevelId(int EmpId)
        {
            return db.LoadData<int, dynamic>("Select GradeLevelId From Employees Where Id = @Id", new { Id = EmpId }, connectionStringName, false).First();
        }
        private string GetEmpCategory(int empId)
        {
            return db.LoadData<string, dynamic>("Select Category From Employees Where Id = @Id", new { Id = empId }, connectionStringName, false).FirstOrDefault();
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
        public decimal GetJSA(int empId)
        {
            string category = GetEmpCategory(empId);
            int gradeLevelId = GetGradeLevelId(empId);
            string rank = db.LoadData<string, dynamic>("Select Rank From GradeLevel Where Id = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false).First();

            if (rank == "JS" && category == "PERM")
            {
                decimal basicSalary = GetBasicSalary(empId);
                return 2M/100M * basicSalary;
            }
            else
            {
                return 0.00M;
            }
        }
        public decimal GetSSA(int empId)
        {
            string category = GetEmpCategory(empId);
            int gradeLevelId = GetGradeLevelId(empId);
            string rank = db.LoadData<string, dynamic>("Select Rank From GradeLevel Where Id = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false).First();

            if (rank == "SS" && category == "PERM")
            {
                decimal basicSalary = GetBasicSalary(empId);
                return 2M / 100M * basicSalary;
            }
            else
            {
                return 0.00M;
            }
        }
        public decimal GetCooperative(int empId)
        {
            
            return db.LoadData<decimal, dynamic>("Select CooperativeDed From Employees Where Id = @Id", new { Id = empId }, connectionStringName, false).First();

        }
        public decimal GetVoluntaryPension(int empId)
        {

            return db.LoadData<decimal, dynamic>("Select VoluntaryPension From Employees Where Id = @Id", new { Id = empId }, connectionStringName, false).First();

        }
        public decimal GetTaxAdjustment(int empId)
        {

            return db.LoadData<decimal, dynamic>("Select TaxAdjustment From Employees Where Id = @Id", new { Id = empId }, connectionStringName, false).First();

        }
        public decimal GetPensionAmount(int empId)
        {
            string category = GetEmpCategory(empId);
            

            if (GetEmpCategory(empId) == "PERM")
            {
                if (empDb.GetPensionStatus(empId))
                {
                    decimal totalEarnings = GetMonthlyGross(empId) * 12M;
                    decimal pensionAmount = ((decimal)8 / (decimal)100) * totalEarnings;
                    return pensionAmount / 12M;
                }
                return 0.00M;
            }
            else
            {
                return 0.00M;
            }
            
        }
        public decimal GetEmployerPensionAmount(int empId)
        {
            string category = GetEmpCategory(empId);

            if (GetEmpCategory(empId) == "PERM")
            {
                if (empDb.GetPensionStatus(empId))
                {
                    decimal totalEarnings = GetMonthlyGross(empId) * 12;
                    decimal pensionAmount = ((decimal)22.5 / (decimal)100) * totalEarnings;
                    return pensionAmount / 12M;
                }
                return 0.00M;
            }
            else
            {
                return 0.00M;
            }

        }
        public decimal GetNHFAmount(int empId)
        {
            if (GetEmpCategory(empId) == "PERM")
            {
                decimal annualBasic = GetBasicSalary(empId) * 12;
                decimal NHFAmount = ((decimal)2.5 / (decimal)100) * annualBasic;
                return NHFAmount / 12M;
            }
            else
            {
                return 0.00M;
            }
            
        }
        
        public decimal GetAnnualPensionAmount(int empId)
        {
            decimal totalEarnings = GetPensionAmount(empId) * 12M;
            decimal voluntaryPension = GetVoluntaryPension(empId) * 12M;
            decimal totalPension = totalEarnings + voluntaryPension;
            //decimal pensionAmount = ((decimal)8 / (decimal)100) * totalEarnings;
            return totalPension;
        }
        public decimal GetAnnualNHFAmount(int empId)
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
            decimal insurance = empDb.GetEmployeeInsurance(empId) * 12M;
            decimal pensionAmount = GetAnnualPensionAmount(empId);
            decimal NHFAmount = GetAnnualNHFAmount(empId);

            return pensionAmount + NHFAmount + insurance;
        }
        private decimal GetCRATotal(int empId)
        {
            decimal relief = ApplyCompanyRelief(empId);
            decimal originalEarnings = payDb.GetMonthlyGross(empId) * 12M;
            return originalEarnings - relief;
        }
        public decimal GetPAYEAmount(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);
            string rank = db.LoadData<string, dynamic>("Select Rank From GradeLevel Where Id = @GradeLevelId", new { GradeLevelId = gradeLevelId }, connectionStringName, false).First();
            
            if (GetEmpCategory(empId) == "PERM" || GetEmpCategory(empId) == "POLI" || rank == "EXEC")
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
                    if (levelTax < 300000M)
                    {
                        levelTax = levelTax + 300000M;
                        taxValue = 7M / 100M * levelTax;
                    }
                }
                if (levelTax >= 300000.00M)
                {
                    taxValue = taxValue + 33000.00M;
                    levelTax = levelTax - 300000.00M;
                    if (levelTax < 500000M)
                    {
                        //levelTax = levelTax + 300000M;
                        //taxValue = (taxValue - 33000M) + (11M / 100M * levelTax);

                        //levelTax = levelTax + 300000M;
                        taxValue = (taxValue) + (15M / 100M * levelTax);
                    }

                }
                if (levelTax >= 500000.00M)
                {
                    taxValue = taxValue + 75000.00M;
                    levelTax = levelTax - 500000.00M;
                    if (levelTax < 500000M)
                    {
                        //levelTax = levelTax + 500000M;
                        //taxValue = (taxValue - 75000M) + (15M / 100M * levelTax);

                        taxValue = (taxValue) + (19M / 100M * levelTax);
                    }
                }
                if (levelTax >= 500000.00M)
                {
                    taxValue = taxValue + 95000.00M;
                    levelTax = levelTax - 500000.00M;
                    if (levelTax < 1600000M)
                    {
                        //levelTax = levelTax + 500000M;
                        //taxValue = (taxValue - 95000M) + (19M / 100M * levelTax);

                        taxValue = (taxValue) + (21M / 100M * levelTax);
                    }
                }
                if (levelTax >= 1600000.00M)
                {
                    taxValue = taxValue + 336000.00M;
                    levelTax = levelTax - 1600000.00M;
                    if (levelTax < 3200000M)
                    {
                        //levelTax = levelTax + 1600000M;
                        //taxValue = (taxValue - 336000M) + (21M / 100M * levelTax);

                        taxValue = (taxValue) + (24M / 100M * levelTax);
                    }
                }
                if (levelTax >= 3200000.00M)
                {
                    //taxValue = taxValue + 768000M;
                    //levelTax = levelTax - 3200000M;
                    finalAmount = ((decimal)24 / (decimal)100) * levelTax;
                }

                decimal taxAdjustment = GetTaxAdjustment(empId);
                decimal totalTax = ((finalAmount + taxValue) / (decimal)12) - (taxAdjustment);

                return totalTax;
            }
            else if (GetEmpCategory(empId) == "CONT")
            {
                decimal OriginalTotalEarnings = payDb.GetMonthlyGross(empId) * (decimal)12;
                //decimal craTotalEarnings = GetCRATotal(empId);
                decimal stateReliefAmount = ApplyStateRelief(OriginalTotalEarnings);
                //decimal companyReliefAmount = ApplyCompanyRelief(empId);
                //decimal totalRelief = stateReliefAmount + companyReliefAmount;
                decimal taxabaleIncome = OriginalTotalEarnings - stateReliefAmount;

                decimal levelTax = taxabaleIncome;
                decimal taxValue = 0.00M;
                //decimal finalLevel = 0.00M;

                decimal finalAmount = 0.00M;

                if (levelTax >= 300000.00M)
                {
                    taxValue = 21000.00M;
                    levelTax = levelTax - 300000.00M;
                    if (levelTax < 300000M)
                    {
                        levelTax = levelTax + 300000M;
                        taxValue = 7M / 100M * levelTax;
                    }
                }
                if (levelTax >= 300000.00M)
                {
                    taxValue = taxValue + 33000.00M;
                    levelTax = levelTax - 300000.00M;
                    if (levelTax < 500000M)
                    {
                        //levelTax = levelTax + 300000M;
                        //taxValue = (taxValue - 33000M) + (11M / 100M * levelTax);

                        taxValue = (taxValue) + (15M / 100M * levelTax);
                    }

                }
                if (levelTax >= 500000.00M)
                {
                    taxValue = taxValue + 75000.00M;
                    levelTax = levelTax - 500000.00M;
                    if (levelTax < 500000M)
                    {
                        //levelTax = levelTax + 500000M;
                        //taxValue = (taxValue - 75000M) + (15M / 100M * levelTax);

                        taxValue = (taxValue) + (19M / 100M * levelTax);
                    }
                }
                if (levelTax >= 500000.00M)
                {
                    taxValue = taxValue + 95000.00M;
                    levelTax = levelTax - 500000.00M;
                    if (levelTax < 1600000M)
                    {
                        //levelTax = levelTax + 500000M;
                        //taxValue = (taxValue - 95000M) + (19M / 100M * levelTax);

                        taxValue = (taxValue) + (21M / 100M * levelTax);
                    }
                }
                if (levelTax >= 1600000.00M)
                {
                    taxValue = taxValue + 336000.00M;
                    levelTax = levelTax - 1600000.00M;
                    if (levelTax < 3200000M)
                    {
                        //levelTax = levelTax + 1600000M;
                        //taxValue = (taxValue - 336000M) + (21M / 100M * levelTax);

                        taxValue = (taxValue) + (24M / 100M * levelTax);
                    }
                }
                if (levelTax >= 3200000.00M)
                {
                    finalAmount = ((decimal)24 / (decimal)100) * levelTax;
                }

                decimal totalTax = (finalAmount + taxValue) / (decimal)12;

                return totalTax;
            }
            else
            {
                return 0.00M;
            }
            
        }

        public decimal GetTotalDeductions(int empId)
        {
            decimal totalDeductions = GetPAYEAmount(empId) + GetNHFAmount(empId) + GetPensionAmount(empId) + GetJSA(empId) + GetSSA(empId) + GetCooperative(empId) + GetVoluntaryPension(empId);
            return totalDeductions;
        }
    }
}

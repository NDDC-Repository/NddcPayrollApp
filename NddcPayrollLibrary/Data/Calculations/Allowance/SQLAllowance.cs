using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NddcPayrollLibrary.Data.Calculations.Allowance
{
    public class SQLAllowance : IAllowanceData
    {
        private readonly ISqlDataAccess db;
        //private readonly IPayrollData payDb;
        private const string connectionStringName = "SqlDb";

        public SQLAllowance(ISqlDataAccess db)
        {
            this.db = db;
            //this.payDb = payDb;
        }

        private int GetGradeLevelId(int EmpId)
        {
            return db.LoadData<int, dynamic>("Select GradeLevelId From Employees Where Id = @Id", new { Id = EmpId }, connectionStringName, false).First();
        }
        private string GetEmpCategory(int empId)
        {
            return db.LoadData<string, dynamic>("Select Category From Employees Where Id = @Id", new { Id = empId }, connectionStringName, false).FirstOrDefault();
        }

        public decimal GetPermStaffBasicSalary(int EmpId)
        {
            int GradeLevelId = GetGradeLevelId(EmpId);
            return db.LoadData<decimal, dynamic>("Select BasicSalary From GradeLevel Where Id = @Id", new { Id = GradeLevelId }, connectionStringName, false).First();
        }
        public decimal GetContStaffBasicSalary(int EmpId)
        {
            int GradeLevelId = GetGradeLevelId(EmpId);
            decimal permBasic = db.LoadData<decimal, dynamic>("Select BasicSalary From GradeLevel Where Id = @Id", new { Id = GradeLevelId }, connectionStringName, false).First();
            decimal contBasic = ((decimal)80 / (decimal)100) * permBasic;
            return contBasic;
        }
        public decimal GetSecretarialAllowance(int EmpId)
        {
            return db.LoadData<decimal, dynamic>("Select SecretarialAllow From Employees Where Id = @Id", new { Id = EmpId }, connectionStringName, false).First();
            
        }
        public decimal GetHousingAllowance(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);
            int benefitTypeId = db.LoadData<int, dynamic>("Select Id From BenefitType Where BenefitType = @BenefitType", new { BenefitType = "Housing" }, connectionStringName, false).FirstOrDefault();
            string check = db.LoadData<string, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { BenefitTypeId = benefitTypeId, GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();

            if (GetEmpCategory(empId) == "CONT")
            {

                if (check is null)
                {
                    return 0.00M;
                }

                int housingPerc = db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                decimal annualBasicSalary = GetContStaffBasicSalary(empId) * 12;
                decimal housingAllowance = (housingPerc / 100M) * annualBasicSalary;
                return housingAllowance / 12;
            }
            else
            {
                return 0.00M;
            }




        }
        public decimal GetFurnitureAllowance(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);
            int benefitTypeId = db.LoadData<int, dynamic>("Select Id From BenefitType Where BenefitType = @BenefitType", new { BenefitType = "Furniture" }, connectionStringName, false).FirstOrDefault();
            string check = db.LoadData<string, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { BenefitTypeId = benefitTypeId, GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();

            if (GetEmpCategory(empId) == "CONT")
            {

                if (check is null)
                {
                    return 0.00M;
                }

                int furniturePerc= db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                decimal annualBasicSalary = GetContStaffBasicSalary(empId) * 12;
                decimal furnitureAllowanceAmount = (furniturePerc / (100M)) * annualBasicSalary;
                return furnitureAllowanceAmount / 12;
            }
            else
            {
                return 0.00M;
            }
        }
        public decimal GetTransportAllowance(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);
            int benefitTypeId = db.LoadData<int, dynamic>("Select Id From BenefitType Where BenefitType = @BenefitType", new { BenefitType = "Transportation" }, connectionStringName, false).FirstOrDefault();
            string check = db.LoadData<string, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { BenefitTypeId = benefitTypeId, GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();
            string empCategory = GetEmpCategory(empId);
            if (empCategory == "CONT")
            {

                if (check is null)
                {
                    return 0.00M;
                }

                int transportPerc = db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                decimal annualBasicSalary = GetContStaffBasicSalary(empId) * 12;
                decimal transportAllowanceAmount = (transportPerc / (decimal)100) * annualBasicSalary;
                return transportAllowanceAmount / 12;
            }
            else if (empCategory == "PERM")
            {

                if (check is null)
                {
                    return 0.00M;
                }

                int transportPerc = db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                decimal annualBasicSalary = GetPermStaffBasicSalary(empId) * 12;
                decimal transportAllowanceAmount = (transportPerc / (decimal)100) * annualBasicSalary;
                return transportAllowanceAmount / 12;
                
            }
            else
            {


                return 0.00M;
            }
            
        }
        public decimal GetMealAllowance(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);
           
            string check = db.LoadData<string, dynamic>("SELECT (Amount * 30) As Amount From Subsidies Where GradeLevelId = @GradeLevelId And SubsidyType = @SubsidyType", new { GradeLevelId = gradeLevelId, SubsidyType = "Meal" }, connectionStringName, false).FirstOrDefault();
            string empCategory = GetEmpCategory(empId);
            
            if (empCategory == "CONT")
            {

                if (check is null)
                {
                    return 0.00M;
                }
                decimal mealAllowance = db.LoadData<decimal, dynamic>("SELECT (Amount * 30) As Amount From Subsidies Where GradeLevelId = @GradeLevelId And SubsidyType = @SubsidyType", new { GradeLevelId = gradeLevelId, SubsidyType = "Meal" }, connectionStringName, false).FirstOrDefault();
                return (80M / 100M) * mealAllowance;
            }
            else if (empCategory == "PERM")
            {
                if (check is null)
                {
                    return 0.00M;
                }
                decimal mealAllowance = db.LoadData<decimal, dynamic>("SELECT (Amount * 30) As Amount From Subsidies Where GradeLevelId = @GradeLevelId And SubsidyType = @SubsidyType", new { GradeLevelId = gradeLevelId, SubsidyType = "Meal" }, connectionStringName, false).FirstOrDefault();
                return mealAllowance;
            }
            else
            {


                return 0.00M;
            }

        }

        public decimal GetUtilityAllowance(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);
            int benefitTypeId = db.LoadData<int, dynamic>("Select Id From BenefitType Where BenefitType = @BenefitType", new { BenefitType = "Utility" }, connectionStringName, false).FirstOrDefault();
            string check = db.LoadData<string, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { BenefitTypeId = benefitTypeId, GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();
            string empCategory = GetEmpCategory(empId);
            if (empCategory == "CONT")
            {

                if (check is null)
                {
                    return 0.00M;
                }

                int utilityPerc = db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                decimal annualBasicSalary = GetContStaffBasicSalary(empId) * 12;
                decimal utilityAllowanceAmount = (utilityPerc / (decimal)100) * annualBasicSalary;
                return utilityAllowanceAmount / 12;
            }
            else if (empCategory == "PERM")
            {

                if (check is null)
                {
                    return 0.00M;
                }

                int utilityPerc = db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                decimal annualBasicSalary = GetPermStaffBasicSalary(empId) * 12;
                decimal utilityAllowanceAmount = (utilityPerc / (decimal)100) * annualBasicSalary;
                return utilityAllowanceAmount / 12;

            }
            else
            {


                return 0.00M;
            }

        }

        public decimal GetEducationAllowance(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);
            int benefitTypeId = db.LoadData<int, dynamic>("Select Id From BenefitType Where BenefitType = @BenefitType", new { BenefitType = "Education" }, connectionStringName, false).FirstOrDefault();
            string check = db.LoadData<string, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { BenefitTypeId = benefitTypeId, GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();
            string empCategory = GetEmpCategory(empId);
            if (empCategory == "CONT")
            {

                if (check is null)
                {
                    return 0.00M;
                }

                int educationPerc = db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                decimal annualBasicSalary = GetContStaffBasicSalary(empId) * 12;
                decimal educationAllowanceAmount = (educationPerc / (decimal)100) * annualBasicSalary;
                return educationAllowanceAmount / 12;
            }
            else if (empCategory == "PERM")
            {

                if (check is null)
                {
                    return 0.00M;
                }

                int educationPerc = db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                decimal annualBasicSalary = GetPermStaffBasicSalary(empId) * 12;
                decimal educationAllowanceAmount = (educationPerc / (decimal)100) * annualBasicSalary;
                return educationAllowanceAmount / 12;

            }
            else
            {


                return 0.00M;
            }

        }

        public decimal GetSecurityAllowance(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);
            int benefitTypeId = db.LoadData<int, dynamic>("Select Id From BenefitType Where BenefitType = @BenefitType", new { BenefitType = "Security" }, connectionStringName, false).FirstOrDefault();
            string check = db.LoadData<string, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { BenefitTypeId = benefitTypeId, GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();
            string empCategory = GetEmpCategory(empId);
            if (empCategory == "CONT")
            {

                if (check is null)
                {
                    return 0.00M;
                }

                int securityPerc = db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                decimal annualBasicSalary = GetContStaffBasicSalary(empId) * 12;
                decimal securityAllowanceAmount = (securityPerc / (decimal)100) * annualBasicSalary;
                return securityAllowanceAmount / 12;
            }
            else if (empCategory == "PERM")
            {

                if (check is null)
                {
                    return 0.00M;
                }

                int securityPerc = db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                decimal annualBasicSalary = GetPermStaffBasicSalary(empId) * 12;
                decimal securityAllowanceAmount = (securityPerc / (decimal)100) * annualBasicSalary;
                return securityAllowanceAmount / 12;

            }
            else
            {


                return 0.00M;
            }

        }

        public decimal GetDomesticServantAllowance(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);
            int benefitTypeId = db.LoadData<int, dynamic>("Select Id From BenefitType Where BenefitType = @BenefitType", new { BenefitType = "Domestic Servant" }, connectionStringName, false).FirstOrDefault();
            string check = db.LoadData<string, dynamic>("SELECT (GradeLevel.MonthlyGross * LinkedBenefits.MultiplyBy) FROM  LinkedBenefits LEFT JOIN GradeLevel ON LinkedBenefits.LinkedGradeLevelId = GradeLevel.Id WHERE LinkedBenefits.GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
            string empCategory = GetEmpCategory(empId);

            if (empCategory == "CONT")
            {

                if (check is null)
                {
                    return 0.00M;
                }
                decimal domeasticServantAllowance = db.LoadData<decimal, dynamic>("SELECT (GradeLevel.MonthlyGross * LinkedBenefits.MultiplyBy) FROM  LinkedBenefits INNER JOIN GradeLevel ON LinkedBenefits.LinkedGradeLevelId = GradeLevel.Id WHERE LinkedBenefits.GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                return (80M / 100M) * domeasticServantAllowance;
            }
            else if (empCategory == "PERM")
            {
                if (check is null)
                {
                    return 0.00M;
                }
                decimal domeasticServantAllowance = db.LoadData<decimal, dynamic>("SELECT (GradeLevel.MonthlyGross * LinkedBenefits.MultiplyBy) FROM  LinkedBenefits INNER JOIN GradeLevel ON LinkedBenefits.LinkedGradeLevelId = GradeLevel.Id WHERE LinkedBenefits.GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                return domeasticServantAllowance;
            }
            else
            {


                return 0.00M;
            }

        }

        public decimal GetMedicalAllowance(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);
            int benefitTypeId = db.LoadData<int, dynamic>("Select Id From BenefitType Where BenefitType = @BenefitType", new { BenefitType = "Medical" }, connectionStringName, false).FirstOrDefault();
            string check = db.LoadData<string, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { BenefitTypeId = benefitTypeId, GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();
            string empCategory = GetEmpCategory(empId);
            if (empCategory == "CONT")
            {

                if (check is null)
                {
                    return 0.00M;
                }

                int medicalPerc = db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                decimal annualBasicSalary = GetContStaffBasicSalary(empId) * 12M;
                decimal medicalAllowanceAmount = (medicalPerc / (decimal)100) * annualBasicSalary;
                return medicalAllowanceAmount / 12M;

                //return 0.00M;
            }
            else if (empCategory == "PERM")
            {

                if (check is null)
                {
                    return 0.00M;
                }

                int medicalPerc = db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                decimal annualBasicSalary = GetPermStaffBasicSalary(empId) * 12M;
                decimal medicalAllowanceAmount = (medicalPerc / (decimal)100) * annualBasicSalary;
                return medicalAllowanceAmount / 12M;
            }
            else
            {


                return 0.00M;
            }

        }

        public decimal GetDriverAllowance(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);
            int benefitTypeId = db.LoadData<int, dynamic>("Select Id From BenefitType Where BenefitType = @BenefitType", new { BenefitType = "Driver" }, connectionStringName, false).FirstOrDefault();
            string check = db.LoadData<string, dynamic>("SELECT (GradeLevel.MonthlyGross * LinkedBenefits.MultiplyBy) FROM  LinkedBenefits INNER JOIN GradeLevel ON LinkedBenefits.LinkedGradeLevelId = GradeLevel.Id WHERE LinkedBenefits.GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
            string empCategory = GetEmpCategory(empId);

            if (empCategory == "CONT")
            {

                if (check is null)
                {
                    return 0.00M;
                }
                decimal driverAllowance = db.LoadData<decimal, dynamic>("SELECT (GradeLevel.MonthlyGross * LinkedBenefits.MultiplyBy) FROM  LinkedBenefits INNER JOIN GradeLevel ON LinkedBenefits.LinkedGradeLevelId = GradeLevel.Id WHERE LinkedBenefits.GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                return (80M / 100M) * driverAllowance;
            }
            else if (empCategory == "PERM")
            {
                if (check is null)
                {
                    return 0.00M;
                }
                decimal driverAllowance = db.LoadData<decimal, dynamic>("SELECT (GradeLevel.MonthlyGross * LinkedBenefits.MultiplyBy) FROM  LinkedBenefits INNER JOIN GradeLevel ON LinkedBenefits.LinkedGradeLevelId = GradeLevel.Id WHERE LinkedBenefits.GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                return driverAllowance;
            }
            else
            {


                return 0.00M;
            }

        }

        public decimal GetVehicleMaintenanceAllowance(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);
            int benefitTypeId = db.LoadData<int, dynamic>("Select Id From BenefitType Where BenefitType = @BenefitType", new { BenefitType = "Vehicle Maintenance" }, connectionStringName, false).FirstOrDefault();
            string check = db.LoadData<string, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { BenefitTypeId = benefitTypeId, GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();
            string empCategory = GetEmpCategory(empId);
            if (empCategory == "CONT")
            {

                if (check is null)
                {
                    return 0.00M;
                }

                int vehicleMaintenancePerc = db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                decimal annualBasicSalary = GetContStaffBasicSalary(empId) * 12;
                decimal vehicleAllowanceAmount = (vehicleMaintenancePerc / (decimal)100) * annualBasicSalary;
                return vehicleAllowanceAmount / 12;
            }
            else if (empCategory == "PERM")
            {

                if (check is null)
                {
                    return 0.00M;
                }

                int vehicleMaintenancePerc = db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                decimal annualBasicSalary = GetPermStaffBasicSalary(empId) * 12;
                decimal vehicleAllowanceAmount = (vehicleMaintenancePerc / (decimal)100) * annualBasicSalary;
                return vehicleAllowanceAmount / 12;

                //int vehicleMaintenancePerc = db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                //decimal annualBasicSalary = GetPermStaffBasicSalary(empId) * 12;
                //decimal vehicleAllowanceAmount = (vehicleMaintenancePerc / (decimal)100) * annualBasicSalary;
                //return vehicleAllowanceAmount / 12;

            }
            else
            {


                return 0.00M;
            }

        }

        public decimal GetHazardAllowance(int empId)
        {
            int gradeLevelId = GetGradeLevelId(empId);
            int benefitTypeId = db.LoadData<int, dynamic>("Select Id From BenefitType Where BenefitType = @BenefitType", new { BenefitType = "Hazard" }, connectionStringName, false).FirstOrDefault();
            string check = db.LoadData<string, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { BenefitTypeId = benefitTypeId, GradeLevelId = gradeLevelId }, connectionStringName, false).FirstOrDefault();
            string empCategory = GetEmpCategory(empId);
            if (empCategory == "CONT")
            {

                if (check is null)
                {
                    return 0.00M;
                }

                int hazardAllowancePerc = db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                decimal annualBasicSalary = GetContStaffBasicSalary(empId) * 12;
                decimal hazardAllowanceAmount = (hazardAllowancePerc / (decimal)100) * annualBasicSalary;
                return hazardAllowanceAmount / 12;
            }
            else if (empCategory == "PERM")
            {

                if (check is null)
                {
                    return 0.00M;
                }

                int hazardAllowancePerc = db.LoadData<int, dynamic>("Select Percentage From Benefits Where GradeLevelId = @GradeLevelId And BenefitTypeId = @BenefitTypeId", new { GradeLevelId = gradeLevelId, BenefitTypeId = benefitTypeId }, connectionStringName, false).FirstOrDefault();
                decimal annualBasicSalary = GetPermStaffBasicSalary(empId) * 12;
                decimal hazardAllowanceAmount = (hazardAllowancePerc / (decimal)100) * annualBasicSalary;
                return hazardAllowanceAmount / 12;

            }
            else
            {


                return 0.00M;
            }

        }
    }
}

using NddcPayrollLibrary.Data.Calculations.Allowance;
using NddcPayrollLibrary.Data.Calculations.Deductions;
using NddcPayrollLibrary.Data.Payroll;
using NddcPayrollLibrary.Databases;
using NddcPayrollLibrary.Model.Employee;
using NddcPayrollLibrary.Model.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NddcPayrollLibrary.Data.Reports
{
    public class SQLReports : IReportsData
    {
        private const string connectionStringName = "SqlDb";
        private readonly ISqlDataAccess db;
        private readonly IPayrollData payDb;
        private readonly IAllowanceData allowDb;
        private readonly IDeductionData dedDb;

        public SQLReports(ISqlDataAccess db, IPayrollData payDb, IAllowanceData allowDb, IDeductionData dedDb)
        {
            this.db = db;
            this.payDb = payDb;
            this.allowDb = allowDb;
            this.dedDb = dedDb;
        }

        public List<MyPayRollListModel> GetPayrollListReport()
        {
            
            //MyPayRollListModel reportModel;
            List<MyPayRollListModel> Reports = new List<MyPayRollListModel>();
            string SQL = "SELECT ROW_NUMBER() OVER (ORDER BY Employees.Id ASC) As SrNo, Employees.Id, Employees.EmployeeCode, Employees.FirstName, Employees.LastName, Employees.Email, Employees.Category, GradeLevel.GradeLevel, Departments.DepartmentName FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = GradeLevel.Id LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id LEFT JOIN JobTitles ON Employees.JobTitleId = JobTitles.Id ORDER BY Employees.Id ASC";
            
            Reports = db.LoadData<MyPayRollListModel, dynamic>(SQL, new { }, connectionStringName, false).ToList();
            foreach (var item in Reports)
            {
                item.BasicSalary = payDb.GetBasicSalary(item.Id);
                item.TransportAllowance = allowDb.GetTransportAllowance(item.Id);
                //item.TransportAllowance = (50M / 100M) * item.BasicSalary;
                item.HousingAllowance = allowDb.GetHousingAllowance(item.Id);
                item.FurnitureAllowance = allowDb.GetFurnitureAllowance(item.Id);
                item.MealAllowance = allowDb.GetMealAllowance(item.Id);
                item.UtilityAllowance = allowDb.GetUtilityAllowance(item.Id);
                item.EducationAllowance = allowDb.GetEducationAllowance(item.Id);
                //item.EducationAllowance = (20M/100M) * item.BasicSalary;
                item.SecurityAllowance = allowDb.GetSecurityAllowance(item.Id);
                item.DomesticServantAllowance = allowDb.GetDomesticServantAllowance(item.Id);
                item.MedicalAllowance = allowDb.GetMedicalAllowance(item.Id);
                //item.MedicalAllowance = (15M/100M) * item.BasicSalary;
                item.VehicleMaintenanceAllowance = allowDb.GetVehicleMaintenanceAllowance(item.Id);
                item.HazardAllowance = allowDb.GetHazardAllowance(item.Id);
                //item.HazardAllowance = (20M/100M) * item.BasicSalary;
                item.DriversAllowance = allowDb.GetDriverAllowance(item.Id);

                item.TaxDeduction = dedDb.GetPAYEAmount(item.Id);
                item.NHFDeduction = dedDb.GetNHFAmount(item.Id);
                item.PensionDeduction = dedDb.GetPensionAmount(item.Id);
                item.JSADeduction = dedDb.GetJSA(item.Id);
                item.SSADeduction = dedDb.GetSSA(item.Id);
                item.TotalEarnings = payDb.GetMonthlyGross(item.Id);
                item.TotalDeductions = dedDb.GetTotalDeductions(item.Id);
                item.NetPay = (item.TotalEarnings) - (item.TotalDeductions);
            }

            return Reports;
        }
    }
}

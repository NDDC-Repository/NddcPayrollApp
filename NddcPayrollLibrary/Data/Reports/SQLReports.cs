using NddcPayrollLibrary.Data.Calculations.Allowance;
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

        public SQLReports(ISqlDataAccess db, IPayrollData payDb, IAllowanceData allowDb)
        {
            this.db = db;
            this.payDb = payDb;
            this.allowDb = allowDb;
        }

        public List<MyPayRollListModel> GetPayrollListReport()
        {
            //MyPayRollListModel reportModel;
            List<MyPayRollListModel> Reports = new List<MyPayRollListModel>();
            string SQL = "SELECT TOP 200 ROW_NUMBER() OVER (ORDER BY Employees.Id ASC) As SrNo, Employees.Id, Employees.EmployeeCode, Employees.FirstName, Employees.LastName, Employees.Email, Employees.Category, GradeLevel.GradeLevel, Departments.DepartmentName FROM Employees LEFT JOIN GradeLevel ON Employees.GradeLevelId = GradeLevel.Id LEFT JOIN Departments ON Employees.DepartmentId = Departments.Id LEFT JOIN JobTitles ON Employees.JobTitleId = JobTitles.Id ORDER BY Employees.Id ASC";
            
            Reports = db.LoadData<MyPayRollListModel, dynamic>(SQL, new { }, connectionStringName, false).ToList();
            foreach (var item in Reports)
            {
                item.BasicSalary = payDb.GetBasicSalary(item.Id);
                item.TransportAllowance = allowDb.GetTransportAllowance(item.Id);
            }

            return Reports;
        }
    }
}
